using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAPPLE.Shared.Model
{
    public class PedidoDTO
    {
        public int Id_GVA { get; set; }
        public int ID_GVA43_TALON_PED { get; set; }
        public string NRO_PEDIDO { get; set; }
        public int ESTADO { get; set; }
        public DateTime FECHA_PEDIDO { get; set; }
        public int? ID_GVA14 { get; set; }
        public bool ES_CLIENTE_HABITUAL { get; set; }
        public int? ID_GVA01 { get; set; }
        public int? ID_GVA23 { get; set; }
        public int? ID_STA22 { get; set; }
        public int? ID_GVA24 { get; set; }
        public string ID_MONEDA { get; set; }
        public decimal COTIZACION { get; set; }
        public int? ID_GVA10 { get; set; }
        public string ID_DIRECCION_ENTREGA { get; set; }
        public DateTime FECHA_ENTREGA { get; set; }
        public string ID_ASIENTO_MODELO_GV { get; set; }
        public string ID_GVA81 { get; set; }
        public string ID_GVA43_TALONARIO_FACTURA { get; set; }
        public string NRO_ORDEN_COMPRA { get; set; }
        public DateTime FECHA_ORDEN_COMPRA { get; set; }
        public string ID_SUCURSAL_DESTINO { get; set; }
        public bool COMPROMETE_STOCK { get; set; }
        public string ID_ACTIVIDAD_EMPRESA_AFIP { get; set; }
        public string ACTIVIDAD_COMPROBANTE_AFIP { get; set; }
        public string ID_TIPO_DOCUMENTO_GV { get; set; }
        public string NUMERO_DOCUMENTO_PAGADOR { get; set; }
        public string LEYENDA_1 { get; set; }
        public string LEYENDA_2 { get; set; }
        public string LEYENDA_3 { get; set; }
        public string LEYENDA_4 { get; set; }
        public string LEYENDA_5 { get; set; }
        public decimal PORCENTAJE_DESCUENTO_GENERAL { get; set; }
        public decimal IMPORTE_DESCUENTO_GENERAL { get; set; }
        public decimal PORCENTAJE_RECARGO_GENERAL { get; set; }
        public decimal IMPORTE_RECARGO_GENERAL { get; set; }
        public bool APLICA_DESCUENTO_CLIENTE { get; set; }
        public bool CALCULA_PROMOCIONES { get; set; }
        public bool VALIDA_LIMITE_CREDITO { get; set; }
        public string ID_SBA01 { get; set; }
        public string OBSERVACIONES { get; set; }
        public List<RenglonDTO> RENGLON_DTO { get; set; }
        public List<ClienteOcasionalDTO> CLIENTE_OCASIONAL_DTO { get; set; }
        public List<NotaPedidoDTO> NOTA_PEDIDO_DTO { get; set; }
    }

    public class RenglonDTO
    {
        public int ID_STA11 { get; set; }
        public string DESCRIPCION_ARTICULO { get; set; }
        public string DESCRIPCION_ADICIONAL_ARTICULO { get; set; }
        public int ID_STA22 { get; set; }
        public string MODULO_UNIDAD_MEDIDA { get; set; }
        public decimal CANTIDAD_PEDIDA { get; set; }
        public decimal CANTIDAD_A_FACTURAR { get; set; }
        public decimal CANTIDAD_A_DESCARGAR { get; set; }
        public decimal CANTIDAD_PENDIENTE_A_FACTURAR { get; set; }
        public decimal PRECIO { get; set; }
        public decimal PORCENTAJE_BONIFICACION { get; set; }
        public decimal IMPORTE { get; set; }
        public int ID_GVA81 { get; set; }
        public string OBSERVACIONES { get; set; }
        public List<PlanEntregaDTO> PLAN_DE_ENTREGA_DTO { get; set; }
        public List<DescripcionAdicionalDTO> DESCRIPCION_ADICIONAL_DTO { get; set; }
    }

    public class PlanEntregaDTO
    {
        public DateTime FECHA_DE_ENTREGA { get; set; }
        public decimal CANTIDAD { get; set; }
    }

    public class DescripcionAdicionalDTO
    {
        public string DESCRIPCION { get; set; }
        public string DESCRIPCION_ADICIONAL { get; set; }
    }

    public class ClienteOcasionalDTO
    {
        public int ID_TIPO_DOCUMENTO_GV { get; set; }
        public string? NRO_DOCUMENTO { get; set; }
        public string? RAZON_SOCIAL { get; set; }
        public string? DOMICILIO { get; set; }
        public string? LOCALIDAD { get; set; }
        public string? CODIGO_POSTAL { get; set; }
        public int ID_GVA18_PROVINCIA { get; set; }
        public string? ACTIVIDAD { get; set; }
        public string? IDENTIFICACION_TRIBUTARIA { get; set; }
        public string? REGIMEN_INGRESOS_BRUTOS { get; set; }
        public string? NRO_INGRESOS_BRUTOS { get; set; }
        public string? E_MAIL { get; set; }
        public string? WEB_CLIENTE { get; set; }
        public string? NUMERO_INSCRIPCION_RG1817 { get; set; }
        public DateTime FECHA_VENCIMIENTO_INSCRIPCION_RG1817 { get; set; }
        public int ID_CATEGORIA_IVA { get; set; }
        public int ID_GVA41_ALICUOTA_NO_CATEGORIZADA { get; set; }
        public bool CALCULA_PERCEPCION_IVA { get; set; }
        public decimal PORCENTAJE_EXCLUSION { get; set; }
        public bool LIQUIDA_IMPUESTOS_INTERNOS { get; set; }
        public bool DISCRIMINA_IMPUESTOS_INTERNOS { get; set; }
        public bool CALCULA_PERCEPCION_IMPUESTOS_INTERNOS { get; set; }
        public int ID_GVA41_ALICUOTA_FIJA_PERCEPCION_IIBB { get; set; }
        public bool LIQUIDA_PERCEPCION_INGRESOS_BRUTOS { get; set; }
        public bool CONSIDERA_IVA_BASE_CALCULO_IIBB { get; set; }
        public int ID_GVA41_ALICUOTA_ADICIONAL_PERCEPCION_IIBB { get; set; }
        public bool CONSIDERA_IVA_BASE_CALCULO_IIBB_ADIC { get; set; }
        public bool LIQUIDA_PERCEPCION_INGRESOS_BRUTOS_59_98 { get; set; }
        public int ID_GVA41_ALICUOTA_FIJA_PERCEPCION_IIBB_59_98 { get; set; }
        public bool INCLUYE_IMPUESTOS_INTERNOS { get; set; }
        public int ID_GVA151 { get; set; }
        public int ID_GVA150 { get; set; }
        public string? DIRECCION_ENTREGA { get; set; }
        public string? LOCALIDAD_ENTREGA { get; set; }
        public string? CODIGO_POSTAL_ENTREGA { get; set; }
        public int ID_GVA18_PROVINCIA_ENTREGA { get; set; }
        public string? TELEFONO1_ENTREGA { get; set; }
        public string? TELEFONO2_ENTREGA { get; set; }
    }

    public class NotaPedidoDTO
    {
        public string? MENSAJE { get; set; }
    }

}
