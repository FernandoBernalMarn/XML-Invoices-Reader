using LectorFacturasXML.Entidades;
using System;
using System.IO;
using System.Xml;

namespace LectorFacturasXML
{
    /// <summary>
    /// Class that contain methods to manage local xml files.
    /// </summary>
    public static class FromLocalFile
    {
        /// <summary>
        /// Return data from local xml file.
        /// </summary>
        /// <param name="filePath">Local path in which is the file.</param>
        /// <returns></returns>
        public static Factura GetData(string filePath)
        {
            try
            {
                var path = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(path))
                    throw new DirectoryNotFoundException("El direcotrio no existe.");

                if (!File.Exists(filePath))
                    throw new FileNotFoundException("El archivo " + Path.GetFileName(filePath) + " no existe.", Path.GetFileName(filePath));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                return ReadXML.GetData(xmlDoc, Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
