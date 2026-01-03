using GAPPLE.Shared.Structs;

namespace GAPPLE.Shared.Helpers
{
    public static class TiposComprobantes
    {
        public static string Remito => "RTO";
        public static string Factura => "FAC";
        public static string NotaCredito => "CRE";
        public static string NotaDebito => "DEB";
        public static string FacturaCreditoElectronica => "FCE";
        public static string NotaCreditoElectronica => "NCE";
        public static string NotaDebitoElectronica => "NDE";
        public static string Automatico => "AUT";

        public static IEnumerable<TipoComprobante> ObtenerTiposComprobante()
        {
            return [
                new(Remito, "Remito"),
                new(Factura, "Factura"),
                new(NotaCredito, "Nota de crédito"),
                new(NotaDebito, "Nota de débito"),
                new(FacturaCreditoElectronica, "Factura de crédito electrónica"),
                new(NotaCreditoElectronica, "Nota de crédito electrónica"),
                new(NotaDebitoElectronica, "Nota de débito electrónica")
                ];
        }

        public static List<TipoComprobante> ObtenerTiposComprobante(params string[] args) =>
            [.. ObtenerTiposComprobante().Where(x => args.Contains(x.IdTipoComprobante))];

        public static IEnumerable<TipoComprobante> ObtenerTiposComprobanteParaNC()
        {
            return [
                new(Factura, "Factura"),
                new(NotaCredito, "Nota de crédito")
                ];
        }

        public static IEnumerable<TipoComprobante> ObtenerTiposComprobanteParaND()
        {
            return [
                new(Factura, "Factura"),
                new(NotaCredito, "Nota de crédito")
                ];
        }

        //public static IEnumerable<TipoComprobante> ObtenerTiposComprobanteParaOP()
        //{
        //    return [
        //        new(Factura, "Factura"),
        //        //new("CRE","Nota de crédito"),
        //        new(NotaDebito, "Nota de débito")
        //        ];
        //}

        public static TipoComprobante ObtenerTipoComprobante(string value)
        {
            return ObtenerTiposComprobante().FirstOrDefault(x => x.IdTipoComprobante == value);
        }

        //public static List<char> ObtenerLetras()
        //{
        //    return ['A', 'B', 'C', 'M', 'R'];
        //}

        //public static List<char> ObtenerLetrasPorCategoria(string tipoIVA)
        //{
        //    return tipoIVA switch
        //    {
        //        "RI" => ['A', 'M', 'B'],
        //        "MT" => ['C'],
        //        "EX" => ['B', 'C'],
        //        "CF" => ['B'],
        //        _ => null
        //    };
        //}
    }
}
