using System;

namespace LectorFacturasXML.Entidades
{
    /// <summary>
    /// Entidad que representa los atributos generales de la factura.
    /// </summary>
    public class FacturaXML
    {
        public string Certificado { get; set; }
        public string CondicionesPago { get; set; }
        public DateTime Fecha { get; set; }
        public string Folio { get; set; }
        public string FormaDePago { get; set; }
        public string LugarExpedicion { get; set; }
        public string MetodoDePago { get; set; }
        public string Moneda { get; set; }
        public string NoCertificado { get; set; }
        public string Serie { get; set; }
        public string Sello { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}

