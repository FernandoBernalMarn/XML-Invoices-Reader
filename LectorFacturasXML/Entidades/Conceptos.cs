using System.Collections.Generic;

namespace LectorFacturasXML.Entidades
{
    /// <summary>
    /// Entidad que contiene el listado de conceptos de la factura (Nodo conceptos).
    /// </summary>
    public class Conceptos
    {
        public Conceptos()
        {
            ListaConceptos = new List<Concepto>();
        }

        public List<Concepto> ListaConceptos { get; set; }
    }
}
