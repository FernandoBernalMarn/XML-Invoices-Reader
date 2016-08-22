
namespace LectorFacturasXML.Entidades
{
    /// <summary>
    /// Entidad que representa al emisor.
    /// </summary>
    public class Emisor
    {
        public Domicilio DomicilioFisal { get; set; }
        public Domicilio ExpedidoEn { get; set; }
        public string Nombre { get; set; }
        public RegimenFiscal RegimenFiscal { get; set; }
        public string RFC { get; set; }
    }
}
