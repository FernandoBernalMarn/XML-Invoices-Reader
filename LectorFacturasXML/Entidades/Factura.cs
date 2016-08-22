
using System.Collections.Generic;

namespace LectorFacturasXML.Entidades
{
    /// <summary>
    /// Entidad que contiene toda la información de la factura.
    /// </summary>
    public class Factura
    {
        public Factura()
        {
            Conceptos = new List<Concepto>();
        }

        public string NombreArchivo { get; set; }
        public List<Concepto> Conceptos { get; set; }
        public Emisor Emisor { get; set; }
        public FacturaXML DatosFactura { get; set; }
        public Impuestos Impuestos { get; set; }
        public Receptor Receptor { get; set; }
    }
}
