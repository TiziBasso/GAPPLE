-- SCRUM-149: Marca de "Armado" para dividir Expedicion en Aprobados / Impresos / Armados
-- Aplicar ANTES de desplegar la version de la app que envia @pArmado.

IF COL_LENGTH('dbo.PedidosCabecera', 'Armado') IS NULL
    ALTER TABLE dbo.PedidosCabecera ADD Armado BIT NOT NULL CONSTRAINT DF_PedidosCabecera_Armado DEFAULT 0;
GO

CREATE OR ALTER PROCEDURE [dbo].[prc_get_PedidosCabeceraExpedicion]
  @pCodOrden NVARCHAR(50) = null,
  @pIdEstado int = null
AS
BEGIN
    select (case when pc2.idpedido is null then CAST(PC1.IdPedido as nvarchar)
            else CAST(PC1.IdPedido as nvarchar)+', '+ CAST(PC2.IdPedido as nvarchar) END) as 'IdPedidos',
            ISNULL(LEFT(PC1.CodigoOrden, 1), '') + ',' + ISNULL(LEFT(PC2.CodigoOrden, 1), '') as 'LetrasOrdenes',
            RIGHT(PC1.CodigoOrden, 8) as Orden,
            (case when pc2.CodigoTango is null then CAST(PC1.CodigoTango as nvarchar)
            else CAST(PC1.CodigoTango as nvarchar)+', '+ CAST(PC2.CodigoTango as nvarchar) END) as 'CodigoTango',
            PC1.FechaEntrega, PC1.AltaRegistro, PC1.Linea, PC1.CodigoCliente, c.RazonSocial, c.CUIT, c.CategoriaIVA CondicionIVA, (PC1.CantidadLineas + ISNULL(PC2.CantidadLineas, 0)) as Articulos,
            PC1.EntregarEn, T.Descripcion Transporte, '' as Zona, PC1.Observaciones Observaciones, PC1.ObservacionesZentra ObservacionesZentra, PC1.AltaUsuario Vendedor, PC1.Impreso,
            /* --- SCRUM-149: la orden esta armada cuando lo estan todos sus pedidos (F y X) --- */
            CAST(CASE WHEN PC1.Armado = 1 AND ISNULL(PC2.Armado, 1) = 1 THEN 1 ELSE 0 END AS BIT) as Armado,
            /* --- SCRUM-77: unidades de probador del pedido (F + X) --- */
            ISNULL((
                SELECT SUM(ISNULL(PD.CantidadProbador, 0))
                FROM   dbo.PedidosDetalle PD
                WHERE  PD.CodigoOrden IN (PC1.CodigoOrden, PC2.CodigoOrden)
            ), 0) as CantidadProbadores
    from PedidosCabecera PC1
    inner join Clientes c on pc1.CodigoCliente = c.CodigoCliente
    LEFT join Transportes T on pc1.CodTransporte = T.CodigoTango
    LEFT join PedidosCabecera PC2 on RIGHT(PC1.CodigoOrden, 8) = RIGHT(PC2.CodigoOrden, 8) and PC1.IdPedido <> PC2.IdPedido and PC1.IdEstado = PC2.IdEstado
    where (PC2.IdPedido is null or PC1.IdPedido < PC2.IdPedido)
        and ((@pIdEstado IS null and pc1.IdEstado = 3) or pc1.IdEstado = @pIdEstado)
        and (@pCodOrden is null or @pCodOrden = RIGHT(PC1.CodigoOrden, 8))
    ORDER by pc1.IdPedido
END
GO

CREATE OR ALTER PROCEDURE [dbo].[prc_upd_PedidosCabecera]
    @pIdPedido              int,
    @pIdEstado              int = null,
    @pImpreso               bit = null,
    @pAprobadoVentas        bit = null,
    @pAprobadoFinanzas      bit = null,
    @pAprobadoContaduria    bit = null,
    @pObservacionesCancelacion NVARCHAR(100) = null,
    @pArmado                bit = null,
    @pEdicionUsuario        nvarchar(50)
AS
BEGIN
    IF @pIdEstado = 3
    BEGIN
    select top 10 * from PedidosCabecera
        UPDATE PedidosCabecera
        SET
            EdicionUsuario = @pEdicionUsuario,
            EdicionRegistro = GETDATE(),
            IdEstado = @pIdEstado,
            FechaAprobacion = GETDATE(),
            Armado = 0 -- SCRUM-149: al (re)aprobar el pedido vuelve a estar sin armar
        WHERE IdPedido = @pIdPedido;

        WITH StockOrdenado AS (
        SELECT
            COD_ARTICU,
            CANT_STOCK,
            ROW_NUMBER() OVER (PARTITION BY COD_ARTICU ORDER BY ID_STA19 DESC) AS rn
        FROM Independiente.dbo.STA19
        where COD_DEPOSI = '01'
        )
        UPDATE pd
        SET pd.CantidadAprobada =
            CASE
                WHEN s.CANT_STOCK >= pd.Cantidad THEN pd.Cantidad
                ELSE 0
            END,
            pd.CantidadProbadorAprobada =
            CASE
                WHEN s.CANT_STOCK >= pd.Cantidad THEN pd.CantidadProbador
                WHEN s.CANT_STOCK <= 0 THEN 0
                ELSE pd.CantidadProbador
            END
        FROM PedidosDetalle pd
        INNER JOIN PedidosCabecera pc ON pc.CodigoOrden = pd.CodigoOrden
        INNER JOIN StockOrdenado s
            ON pd.CodProducto COLLATE Modern_Spanish_CI_AS = s.COD_ARTICU COLLATE Modern_Spanish_CI_AS
        WHERE pc.IdPedido = @pIdPedido AND s.rn = 1; -- Solo usamos el stock con el ID_STA19 más alto
    END
    ELSE
    BEGIN
        UPDATE PedidosCabecera
        SET
        EdicionUsuario = @pEdicionUsuario,
            EdicionRegistro = GETDATE(),
            IdEstado = CASE WHEN @pIdEstado IS NULL THEN IdEstado ELSE @pIdEstado END,
            Impreso = CASE WHEN @pImpreso IS NULL THEN Impreso ELSE @pImpreso END,
            AprobadoContaduria = CASE WHEN @pAprobadoContaduria IS NULL THEN AprobadoContaduria ELSE @pAprobadoContaduria END,
            AprobadoFinanzas = CASE WHEN @pAprobadoFinanzas IS NULL THEN AprobadoFinanzas ELSE @pAprobadoFinanzas END,
            AprobadoVentas = CASE WHEN @pAprobadoVentas IS NULL THEN AprobadoVentas ELSE @pAprobadoVentas END,
            ObservacionesCancelacion = CASE WHEN @pObservacionesCancelacion IS NULL THEN ObservacionesCancelacion ELSE @pObservacionesCancelacion END,
            Armado = CASE WHEN @pArmado IS NULL THEN Armado ELSE @pArmado END
        WHERE IdPedido = @pIdPedido;
    END
END
GO
