using System.Collections.Generic;

namespace LectorFacturasXML.Entidades
{
    /// <summary>
    /// Entidad que representa el nodo Impuestos.
    /// </summary>
    public class Impuestos
    {
        public Impuestos()
        {
            Retenciones = new List<Retencion>();
            Traslados = new List<Traslado>();
        }
    
        public List<Retencion> Retenciones { get; set; }
        public List<Traslado> Traslados { get; set; }
        public decimal TotalImpuestosRetenidos { get; set; }
        public decimal TotalImpuestosTrasladados { get; set; }
    }
}
