
namespace LectorFacturasXML.Entidades
{
    /// <summary>
    /// Entidad que representa un Traslado (Impuesto).
    /// </summary>
    public class Traslado
    {
        public decimal Importe { get; set; }
        public string Impuesto { get; set; }
        public decimal Tasa { get; set; }
    }
}
