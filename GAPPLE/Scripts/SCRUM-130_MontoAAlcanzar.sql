-- SCRUM-130: Monto a alcanzar en Acuerdos
-- Aplicar ANTES de desplegar la version de la app que envia @pMontoAAlcanzar.

IF COL_LENGTH('dbo.Acuerdos', 'MontoAAlcanzar') IS NULL
    ALTER TABLE dbo.Acuerdos ADD MontoAAlcanzar DECIMAL(18, 2) NULL;
GO

CREATE OR ALTER PROCEDURE [dbo].[prc_get_Acuerdos]
    @pIdAcuerdo int = null,
    @pFechaDesde date = null,
    @pFechaHasta date = null,
    @pIdCliente int = null,
    @pCodCliente NVARCHAR(8) = null,
    @pRazonSocial NVARCHAR(50) = NULL,
    @pCUIT  NVARCHAR(13) = null,
    @pLinea NVARCHAR(50) = null,
    @pIdEstado int = null
AS
BEGIN
    SELECT A.IdAcuerdo, A.IdCliente, Cli.RazonSocial, Cli.CodigoCliente, Cli.CUIT,
            A.Linea, A.Condicion, A.FechaDesde, A.FechaHasta, A.IdEstado, E.Descripcion DescripcionEstado,
            A.AltaRegistro, A.AltaUsuario, A.EdicionRegistro, A.EdicionUsuario, (select COUNT(1) from AcuerdosMontos where IdAcuerdo = A.IdAcuerdo) MontosCargados,
            ISNULL((select SUM(Monto) from AcuerdosMontos where IdAcuerdo = A.IdAcuerdo), 0) TotalCargado,
            A.MontoAAlcanzar
    FROM Acuerdos A
    INNER JOIN Clientes Cli on A.IdCliente = Cli.IdCliente
    INNER JOIN Estados E on A.IdEstado = E.IdEstado
    WHERE (
        @pIdAcuerdo IS NULL
        AND ((@pFechaDesde is null AND @pFechaHasta is null) or (A.FechaDesde <= @pFechaHasta AND A.FechaHasta >= @pFechaDesde))
        AND (@pIdCliente IS NULL OR A.IdCliente = @pIdCliente)
        AND (@pCodCliente IS NULL OR Cli.CodigoCliente LIKE @pCodCliente)
        AND (@pRazonSocial IS NULL OR Cli.RazonSocial LIKE @pRazonSocial)
        AND (@pCUIT IS NULL OR Cli.CUIT LIKE @pCUIT)
        AND (@pLinea IS NULL OR A.Linea LIKE @pLinea)
        AND (@pIdEstado IS NULL OR A.IdEstado = @pIdEstado)
    )
    OR (A.IdAcuerdo = @pIdAcuerdo)
END
GO

CREATE OR ALTER PROCEDURE [dbo].[prc_ins_Acuerdo]
    @pIdCliente int,
    @pLinea NVARCHAR(50) = null,
    @pCondicion NVARCHAR(150),
    @pFechaDesde DATETIME,
    @pFechaHasta DATETIME,
    @pIdEstado int,
    @pMontoAAlcanzar DECIMAL(18, 2) = null,
    @pAltaUsuario NVARCHAR(50)
AS
BEGIN
    DECLARE @IdAcuerdo INT;

    insert into Acuerdos (IdCliente, Linea, Condicion, FechaDesde, FechaHasta, IdEstado, MontoAAlcanzar, AltaRegistro, AltaUsuario)
    values (@pIdCliente, @pLinea, @pCondicion, @pFechaDesde, @pFechaHasta, @pIdEstado, @pMontoAAlcanzar, GETDATE(), @pAltaUsuario)

    SET @IdAcuerdo = SCOPE_IDENTITY();

    SELECT @IdAcuerdo AS IdAcuerdo;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[prc_upd_Acuerdo]
    @pIdAcuerdo int,
    @pLinea NVARCHAR(50) = null,
    @pCondicion NVARCHAR(150) = null,
    @pFechaDesde DATETIME = null,
    @pFechaHasta DATETIME = null,
    @pIdEstado int = null,
    @pMontoAAlcanzar DECIMAL(18, 2) = null,
    @pEdicionUsuario NVARCHAR(50)
AS
BEGIN
    update Acuerdos
    set
        Linea = ISNULL(@pLinea, Linea),
        Condicion = ISNULL(@pCondicion, Condicion),
        FechaDesde = ISNULL(@pFechaDesde, FechaDesde),
        FechaHasta = ISNULL(@pFechaHasta, FechaHasta),
        IdEstado = ISNULL(@pIdEstado, IdEstado),
        MontoAAlcanzar = ISNULL(@pMontoAAlcanzar, MontoAAlcanzar),
        EdicionRegistro = GETDATE(),
        EdicionUsuario = @pEdicionUsuario
    where IdAcuerdo = @pIdAcuerdo

END
GO
