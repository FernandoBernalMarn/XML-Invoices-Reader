using LectorFacturasXML.Entidades;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LectorFacturasXML
{
    /// <summary>
    /// Class that contain methods to manage xml files loaded on string.
    /// </summary>
    public static class FromString
    {
        /// <summary>
        /// Return data of xml in a string.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Factura GetData(string xmlString, string fileName)
        {
            try
            {
                string xmlName = string.Empty;
            
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);

                if (!string.IsNullOrEmpty(fileName))
                    xmlName = fileName;
                else
                    xmlName = "Invoice-" + DateTime.Now.ToString("MM-dd-yyyy-H-mm-ss");

                return LeerFacturaXML.ObtenerDatosFactura(xmlDoc, xmlName);
            }
            catch (Exception ex) { throw (ex); }
        }
    }
}
