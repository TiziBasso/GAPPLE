-- Listas de precios cargadas desde Excel y asignadas por cliente y vendedor
--  * ListasPrecioImportadas: cada lista (ej. "Lista 174") con el Excel original que se subio.
--  * ListasPrecioImportadasDetalle: los precios por SKU leidos del Excel. Al subir una nueva version se reemplazan.
--  * ListasPrecioPorClienteVendedor: que listas ve cada vendedor para cada cliente. Un mismo cliente puede tener
--    listas distintas segun el vendedor, y los vendedores solo ven las de los clientes que tienen asignados en VendedoresPorCliente.
--    Los administradores (perfil 1) cuentan como vendedores de todos los clientes, igual que en /clientes/vendedores.
-- Aplicar ANTES de desplegar la version de la app que usa /clientes/listas-precio y /listas-precio/mis-listas.
-- Se puede volver a correr: si ya se habia aplicado una version anterior, la actualiza (ej. la columna Rubro pasa a Familia).

IF OBJECT_ID('dbo.ListasPrecioImportadas', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ListasPrecioImportadas
    (
        IdListaPrecio    INT IDENTITY(1, 1) NOT NULL CONSTRAINT PK_ListasPrecioImportadas PRIMARY KEY,
        Nombre           NVARCHAR(150)  NOT NULL CONSTRAINT UQ_ListasPrecioImportadas_Nombre UNIQUE,
        VigenciaDesde    DATE           NULL,
        NombreArchivo    NVARCHAR(260)  NULL,
        Archivo          VARBINARY(MAX) NULL,
        AltaUsuario      NVARCHAR(50)   NULL,
        AltaRegistro     DATETIME       NOT NULL CONSTRAINT DF_ListasPrecioImportadas_AltaRegistro DEFAULT GETDATE(),
        EdicionUsuario   NVARCHAR(50)   NULL,
        EdicionRegistro  DATETIME       NULL
    );
END
GO

IF OBJECT_ID('dbo.ListasPrecioImportadasDetalle', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ListasPrecioImportadasDetalle
    (
        IdListaPrecio          INT            NOT NULL,
        Orden                  INT            NOT NULL,
        Familia                NVARCHAR(150)  NULL,
        SKU                    NVARCHAR(50)   NOT NULL,
        Descripcion            NVARCHAR(250)  NULL,
        Tono                   NVARCHAR(50)   NULL,
        EAN                    NVARCHAR(50)   NULL,
        Contenido              NVARCHAR(100)  NULL,
        UnidadesPorCaja        INT            NULL,
        CostoNetoSinIva        DECIMAL(18, 2) NULL,
        CostoNetoConIva        DECIMAL(18, 2) NULL,
        PrecioSugeridoPublico  DECIMAL(18, 2) NULL,
        CONSTRAINT PK_ListasPrecioImportadasDetalle PRIMARY KEY (IdListaPrecio, Orden),
        CONSTRAINT FK_ListasPrecioImportadasDetalle_Lista FOREIGN KEY (IdListaPrecio)
            REFERENCES dbo.ListasPrecioImportadas (IdListaPrecio) ON DELETE CASCADE
    );
END
GO

-- Una version anterior de este script llamaba "Rubro" a la familia del articulo
IF COL_LENGTH('dbo.ListasPrecioImportadasDetalle', 'Rubro') IS NOT NULL
    EXEC sp_rename 'dbo.ListasPrecioImportadasDetalle.Rubro', 'Familia', 'COLUMN';
GO

-- Un borrador previo de este script creaba esta tabla con CodListaPrecio (listas de Tango). Si quedo creada asi, se recrea.
IF COL_LENGTH('dbo.ListasPrecioPorClienteVendedor', 'CodListaPrecio') IS NOT NULL
    DROP TABLE dbo.ListasPrecioPorClienteVendedor;
GO

IF OBJECT_ID('dbo.ListasPrecioPorClienteVendedor', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ListasPrecioPorClienteVendedor
    (
        IdCliente      INT           NOT NULL,
        IdUsuario      INT           NOT NULL,
        IdListaPrecio  INT           NOT NULL,
        AltaUsuario    NVARCHAR(50)  NULL,
        AltaRegistro   DATETIME      NOT NULL CONSTRAINT DF_ListasPrecioPorClienteVendedor_AltaRegistro DEFAULT GETDATE(),
        CONSTRAINT PK_ListasPrecioPorClienteVendedor PRIMARY KEY (IdCliente, IdUsuario, IdListaPrecio),
        CONSTRAINT FK_ListasPrecioPorClienteVendedor_Lista FOREIGN KEY (IdListaPrecio)
            REFERENCES dbo.ListasPrecioImportadas (IdListaPrecio) ON DELETE CASCADE
    );

    CREATE INDEX IX_ListasPrecioPorClienteVendedor_IdUsuario ON dbo.ListasPrecioPorClienteVendedor (IdUsuario);
    CREATE INDEX IX_ListasPrecioPorClienteVendedor_IdListaPrecio ON dbo.ListasPrecioPorClienteVendedor (IdListaPrecio);
END
GO

-- Un tipo no se puede modificar: si quedo con la columna vieja (Rubro) se borra junto con el SP que lo usa,
-- y los dos se vuelven a crear mas abajo
IF EXISTS (SELECT 1
           FROM sys.table_types T
           INNER JOIN sys.columns C ON C.object_id = T.type_table_object_id
           WHERE T.schema_id = SCHEMA_ID('dbo') AND T.name = 'tt_ListaPrecioImportadaDetalle' AND C.name = 'Rubro')
BEGIN
    IF OBJECT_ID('dbo.prc_ins_ListaPrecioImportada', 'P') IS NOT NULL
        DROP PROCEDURE dbo.prc_ins_ListaPrecioImportada;
    DROP TYPE dbo.tt_ListaPrecioImportadaDetalle;
END
GO

-- Si hace falta cambiar este tipo, primero hay que borrar prc_ins_ListaPrecioImportada (lo usa)
IF TYPE_ID('dbo.tt_ListaPrecioImportadaDetalle') IS NULL
    CREATE TYPE dbo.tt_ListaPrecioImportadaDetalle AS TABLE
    (
        Orden                  INT            NOT NULL,
        Familia                NVARCHAR(150)  NULL,
        SKU                    NVARCHAR(50)   NOT NULL,
        Descripcion            NVARCHAR(250)  NULL,
        Tono                   NVARCHAR(50)   NULL,
        EAN                    NVARCHAR(50)   NULL,
        Contenido              NVARCHAR(100)  NULL,
        UnidadesPorCaja        INT            NULL,
        CostoNetoSinIva        DECIMAL(18, 2) NULL,
        CostoNetoConIva        DECIMAL(18, 2) NULL,
        PrecioSugeridoPublico  DECIMAL(18, 2) NULL
    );
GO

/* ---------------------------------- Listas ---------------------------------- */

CREATE OR ALTER PROCEDURE [dbo].[prc_get_ListasPrecioImportadas]
    @pIdListaPrecio INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT L.IdListaPrecio, L.Nombre, L.VigenciaDesde, L.NombreArchivo,
           (SELECT COUNT(1) FROM dbo.ListasPrecioImportadasDetalle D WHERE D.IdListaPrecio = L.IdListaPrecio) AS CantidadArticulos,
           (SELECT COUNT(DISTINCT A.IdCliente) FROM dbo.ListasPrecioPorClienteVendedor A WHERE A.IdListaPrecio = L.IdListaPrecio) AS CantidadClientes,
           ISNULL(L.EdicionUsuario, L.AltaUsuario) AS UsuarioActualizacion,
           ISNULL(L.EdicionRegistro, L.AltaRegistro) AS FechaActualizacion
    FROM dbo.ListasPrecioImportadas L
    WHERE @pIdListaPrecio IS NULL OR L.IdListaPrecio = @pIdListaPrecio
    ORDER BY L.Nombre;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[prc_get_ListasPrecioImportadasDetalle]
    @pIdListaPrecio INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Orden, Familia, SKU, Descripcion, Tono, EAN, Contenido, UnidadesPorCaja,
           CostoNetoSinIva, CostoNetoConIva, PrecioSugeridoPublico
    FROM dbo.ListasPrecioImportadasDetalle
    WHERE IdListaPrecio = @pIdListaPrecio
    ORDER BY Orden;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[prc_get_ListaPrecioImportadaArchivo]
    @pIdListaPrecio INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Nombre, NombreArchivo, Archivo
    FROM dbo.ListasPrecioImportadas
    WHERE IdListaPrecio = @pIdListaPrecio;
END
GO

-- Alta (@pIdListaPrecio NULL) o reemplazo completo de una lista: cabecera, archivo y todos sus precios.
-- Las asignaciones a clientes/vendedores se mantienen al reemplazar.
CREATE OR ALTER PROCEDURE [dbo].[prc_ins_ListaPrecioImportada]
    @pIdListaPrecio  INT = NULL,
    @pNombre         NVARCHAR(150),
    @pVigenciaDesde  DATE = NULL,
    @pNombreArchivo  NVARCHAR(260) = NULL,
    @pArchivo        VARBINARY(MAX) = NULL,
    @pUsuario        NVARCHAR(50) = NULL,
    @pDetalle        dbo.tt_ListaPrecioImportadaDetalle READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS (SELECT 1 FROM dbo.ListasPrecioImportadas
               WHERE Nombre = @pNombre AND (@pIdListaPrecio IS NULL OR IdListaPrecio <> @pIdListaPrecio))
    BEGIN
        RAISERROR(N'Ya existe otra lista de precios llamada "%s"', 16, 1, @pNombre);
        RETURN;
    END

    IF @pIdListaPrecio IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.ListasPrecioImportadas WHERE IdListaPrecio = @pIdListaPrecio)
    BEGIN
        RAISERROR(N'La lista de precios que se quiere actualizar ya no existe', 16, 1);
        RETURN;
    END

    BEGIN TRAN;

    IF @pIdListaPrecio IS NULL
    BEGIN
        INSERT INTO dbo.ListasPrecioImportadas (Nombre, VigenciaDesde, NombreArchivo, Archivo, AltaUsuario)
        VALUES (@pNombre, @pVigenciaDesde, @pNombreArchivo, @pArchivo, @pUsuario);

        SET @pIdListaPrecio = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        UPDATE dbo.ListasPrecioImportadas
        SET Nombre = @pNombre,
            VigenciaDesde = @pVigenciaDesde,
            NombreArchivo = @pNombreArchivo,
            Archivo = @pArchivo,
            EdicionUsuario = @pUsuario,
            EdicionRegistro = GETDATE()
        WHERE IdListaPrecio = @pIdListaPrecio;

        DELETE FROM dbo.ListasPrecioImportadasDetalle WHERE IdListaPrecio = @pIdListaPrecio;
    END

    INSERT INTO dbo.ListasPrecioImportadasDetalle
        (IdListaPrecio, Orden, Familia, SKU, Descripcion, Tono, EAN, Contenido, UnidadesPorCaja,
         CostoNetoSinIva, CostoNetoConIva, PrecioSugeridoPublico)
    SELECT @pIdListaPrecio, Orden, Familia, SKU, Descripcion, Tono, EAN, Contenido, UnidadesPorCaja,
           CostoNetoSinIva, CostoNetoConIva, PrecioSugeridoPublico
    FROM @pDetalle;

    COMMIT;

    SELECT @pIdListaPrecio AS IdListaPrecio;
END
GO

-- Borra la lista con sus precios y sus asignaciones (ON DELETE CASCADE)
CREATE OR ALTER PROCEDURE [dbo].[prc_del_ListaPrecioImportada]
    @pIdListaPrecio INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.ListasPrecioImportadas WHERE IdListaPrecio = @pIdListaPrecio;
END
GO

/* ------------------------- Asignacion cliente / vendedor ------------------------- */

-- VendedorAsignado indica si el usuario sigue asignado al cliente en VendedoresPorCliente. Los administradores no figuran
-- en esa tabla (son vendedores de todos los clientes): eso lo resuelve la aplicacion.
CREATE OR ALTER PROCEDURE [dbo].[prc_get_ListasPrecioPorClienteVendedor]
    @pIdCliente  INT = NULL,
    @pIdUsuario  INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT A.IdCliente, C.CodigoCliente, C.RazonSocial, A.IdUsuario, A.IdListaPrecio, L.Nombre AS NombreLista, L.VigenciaDesde,
           CAST(CASE WHEN EXISTS (SELECT 1
                                  FROM dbo.VendedoresPorCliente V
                                  WHERE V.IdCliente = A.IdCliente AND V.IdUsuario = A.IdUsuario)
                     THEN 1 ELSE 0 END AS BIT) AS VendedorAsignado
    FROM dbo.ListasPrecioPorClienteVendedor A
    INNER JOIN dbo.ListasPrecioImportadas L ON L.IdListaPrecio = A.IdListaPrecio
    INNER JOIN dbo.Clientes C ON C.IdCliente = A.IdCliente
    WHERE (@pIdCliente IS NULL OR A.IdCliente = @pIdCliente)
      AND (@pIdUsuario IS NULL OR A.IdUsuario = @pIdUsuario)
    ORDER BY C.RazonSocial, A.IdUsuario, L.Nombre;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[prc_del_ListasPrecioPorClienteVendedor]
    @pIdCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.ListasPrecioPorClienteVendedor WHERE IdCliente = @pIdCliente;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[prc_ins_ListasPrecioPorClienteVendedor]
    @pIdCliente      INT,
    @pIdUsuario      INT,
    @pIdListaPrecio  INT,
    @pAltaUsuario    NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.ListasPrecioPorClienteVendedor
                   WHERE IdCliente = @pIdCliente AND IdUsuario = @pIdUsuario AND IdListaPrecio = @pIdListaPrecio)
        INSERT INTO dbo.ListasPrecioPorClienteVendedor (IdCliente, IdUsuario, IdListaPrecio, AltaUsuario)
        VALUES (@pIdCliente, @pIdUsuario, @pIdListaPrecio, @pAltaUsuario);
END
GO
