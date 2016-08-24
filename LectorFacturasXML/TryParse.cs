using System.Xml;

namespace LectorFacturasXML
{
    public static class TryParse
    {
        /// <summary>
        /// Metodo to validate if the xml string contains a valid formato.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static bool TryParseFromString(string xmlString)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlString);
                return true;
            }
            catch (XmlException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo to validate if the given file contains a valid formato.
        /// </summary>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public static bool TryParseFromLocalFile(string localPath)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(localPath);
                return true;
            }
            catch (XmlException ex)
            {
                return false;
            }
        }
    }
}
