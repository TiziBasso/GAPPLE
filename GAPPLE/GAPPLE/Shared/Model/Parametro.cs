using System;
using System.Xml;

namespace Integra.Web.Shared.Model
{
    public class Parametro : ICloneable
    {
        public string Aplicaion { get; set; }

        public string IdParametro { get; set; }
        
        public string Descripcion { get; set; }
        
        public float? ValorNumerico { get; set; }
        
        public string ValorAlfanumerico { get; set; }
        
        public XmlDocument ValorXML { get; set; }

        public bool HuboCambios { get; set; }

        public Parametro Original { get; set; }

        public object Clone() => MemberwiseClone();
    }
}
