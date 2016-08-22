
namespace LectorFacturasXML.Entidades
{
    /// <summary>
    /// Entidad que representa el atributo Domicilio.
    /// </summary>
    public class Domicilio
    {
        public string Calle { get; set; }
        public string CodigoPotal { get; set; }
        public string Colonia { get; set; }
        public string Estado { get; set; }
        public string Localidad { get; set; }
        public string Municipio { get; set; }
        public string NoExterior { get; set; }
        public string NoInterior { get; set; }
        public string Pais { get; set; }

        /// <summary>
        /// Retorna la información del domicilio en una sola línea.
        /// </summary>
        /// <returns></returns>
        private string domicilioCompleto() {
            string domicilio = string.Empty;

            domicilio += Calle + ", ";
            domicilio += !string.IsNullOrEmpty(NoInterior) ?  "num. Interior " + NoInterior + ", " : string.Empty;
            domicilio += !string.IsNullOrEmpty(NoExterior) ? "num. Exterior " + NoExterior + ", " : string.Empty;
            domicilio += !string.IsNullOrEmpty(Colonia) ? Colonia + ", " : string.Empty;
            domicilio += CodigoPotal + ", ";
            domicilio += Municipio + ", ";
            domicilio += !string.IsNullOrEmpty(Localidad) ? Localidad + ", " : string.Empty;
            domicilio += Estado + ", ";
            domicilio += Pais + ", ";

            return domicilio;
        }

        public string DomicilioCompleto {
            get { return domicilioCompleto(); }
        }
    }
}
