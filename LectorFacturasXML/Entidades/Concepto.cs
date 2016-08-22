
namespace LectorFacturasXML.Entidades
{
    /// <summary>
    /// Entidad que representa un concepto dentro de la factura.
    /// </summary>
    public class Concepto
    {
        public decimal Cantidad { get; set; }
        public string Descripcion { get; set; }
        public string Unidad { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
