using LectorFacturasXML.Entidades;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace LectorFacturasXML
{
    public class LeerFacturaXML
    {
        /// <summary>
        /// Return data to xml file on XmlDocument data type.
        /// </summary>
        /// <param name="factura"> XmlDocument that contain info. </param>
        /// <param name="nameFile"> File name to set into Factura.NombreArchivo </param>
        /// <returns></returns>
        public static Factura ObtenerDatosFactura(XmlDocument factura, string nameFile)
        {
            try
            {
                Factura datos = new Factura();

                var FirstNode = factura.GetElementsByTagName("cfdi:Comprobante");
                var ChilNodes = FirstNode[0].ChildNodes;

                datos.NombreArchivo = nameFile;

                datos.DatosFactura = new FacturaXML() {
                    Certificado = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("certificado")).Select(x => x.Value).FirstOrDefault(),
                    CondicionesPago = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("condicionesdepago")).Select(x => x.Value).FirstOrDefault(),
                    Fecha = DateTime.Parse(FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("fecha")).Select(x => x.Value).FirstOrDefault()),
                    Folio = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("folio")).Select(x => x.Value).FirstOrDefault(),
                    FormaDePago = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("formadepago")).Select(x => x.Value).FirstOrDefault(),
                    LugarExpedicion = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("lugarexpedicion")).Select(x => x.Value).FirstOrDefault(),
                    MetodoDePago = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("metododepago")).Select(x => x.Value).FirstOrDefault(),
                    Moneda = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("certificado")).Select(x => x.Value).FirstOrDefault(),
                    NoCertificado = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("nocertificado")).Select(x => x.Value).FirstOrDefault(),
                    Serie = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("serie")).Select(x => x.Value).FirstOrDefault(),
                    Sello = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("sello")).Select(x => x.Value).FirstOrDefault(),
                    Subtotal = FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("subtotal")).Select(x => x.Value).FirstOrDefault() != null ? decimal.Parse(FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("subtotal")).Select(x => x.Value).FirstOrDefault()) : 0.0m,
                    Total = decimal.Parse(FirstNode[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("total")).Select(x => x.Value).FirstOrDefault())
                };

                if (ChilNodes.Count > 0)
                {
                    for (int i = 0; i < ChilNodes.Count; i++)
                    {
                        var Node = ChilNodes[i];

                        switch (Node.LocalName)
                        {
                            case "Emisor":
                                datos.Emisor = new Emisor() {
                                    RFC = Node.Attributes["rfc"].Value ,
                                    Nombre = Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("nombre")).Select(x => x.Value).FirstOrDefault() != null ? Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("nombre")).Select(x => x.Value).FirstOrDefault().Replace("'", " ") : string.Empty
                                }; 

                                CargarDomicilioFiscalEmisor(ref datos, Node.OuterXml);
                                break;
                            case "Receptor":
                                datos.Receptor = new Receptor() {
                                    RFC = Node.Attributes["rfc"].Value,
                                    Nombre = Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("nombre")).Select(x => x.Value).FirstOrDefault() != null ? Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("nombre")).Select(x => x.Value).FirstOrDefault().Replace("'", " ") : string.Empty
                                };

                                CargarDomicilioReceptor(ref datos, Node.OuterXml);
                                break;
                            case "Impuestos":
                                datos.Impuestos = new Impuestos() {
                                    TotalImpuestosRetenidos = Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("totalimpuestosretenidos")).Select(x => x.Value).FirstOrDefault() != null ? decimal.Parse(Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("totalimpuestosretenidos")).Select(x => x.Value).FirstOrDefault()) : 0.0m,
                                    TotalImpuestosTrasladados = Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("totalimpuestostrasladados")).Select(x => x.Value).FirstOrDefault() != null ? decimal.Parse(Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("totalimpuestostrasladados")).Select(x => x.Value).FirstOrDefault()) : 0.0m
                                };

                                CargarImpuestos(ref datos, Node.OuterXml);
                                break;
                            case "Conceptos":
                                CargarConceptos(ref datos, Node.OuterXml);
                                break;
                        }
                    }
                }

                return datos;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Método que se encarga de agregar los impuestos (Traslados y/o Retenciones).
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="outerXml"></param>
        private static void CargarImpuestos(ref Factura factura, string outerXml)
        {
            try
            {
                XmlDocument DocXml = new XmlDocument();
                DocXml.LoadXml(outerXml);

                var retenciones = DocXml.GetElementsByTagName("cfdi:Retenciones");

                if (retenciones.Count > 0)
                {
                    foreach (XmlNode ChildNode in retenciones[0].ChildNodes)
                    {
                        factura.Impuestos.Retenciones.Add(new Retencion()
                        {
                            Importe = decimal.Parse(ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("importe")).Select(x => x.Value).First()),
                            Impuesto = ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("impuesto")).Select(x => x.Value).First()
                        });
                    }
                }
                
                var traslados = DocXml.GetElementsByTagName("cfdi:Traslados");

                if (traslados.Count > 0)
                {
                    foreach (XmlNode ChildNode in traslados[0].ChildNodes)
                    {
                        factura.Impuestos.Traslados.Add(new Traslado()
                        {
                            Importe = decimal.Parse(ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("importe")).Select(x => x.Value).First()),
                            Impuesto = ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("impuesto")).Select(x => x.Value).First(),
                            Tasa = decimal.Parse(ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("tasa")).Select(x => x.Value).First())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Carga los datos que se encuentran dentro del apartado "Conceptos" en la factura.
        /// </summary>
        /// <param name="factura"></param>
        /// <param name="outerXml"></param>
        private static void CargarConceptos(ref Factura factura, string outerXml)
        {
            try
            {
                XmlDocument DocXml = new XmlDocument();
                DocXml.LoadXml(outerXml);

                var Node = DocXml.GetElementsByTagName("cfdi:Conceptos");

                if(Node.Count > 0)
                {
                    foreach (XmlNode ChildNode in Node[0].ChildNodes)
                    {
                        factura.Conceptos.Add(new Concepto()
                        {
                            Cantidad = ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("cantidad")).Select(x => x.Value).FirstOrDefault() != null ? decimal.Parse(ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("cantidad")).Select(x => x.Value).First()) : 0.0m,
                            Descripcion = ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("descripcion")).Select(x => x.Value).FirstOrDefault(),
                            Unidad = ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("unidad")).Select(x => x.Value).FirstOrDefault(),
                            ValorUnitario = ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("valorunitario")).Select(x => x.Value).FirstOrDefault() != null ? decimal.Parse(ChildNode.Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("valorunitario")).Select(x => x.Value).First()) : 0.0m
                        });
                    }
                }                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Carga los datos que se encuentran dentro del apartado "Domiclio" en la seccion "Receptor" de la factura.
        /// </summary>
        /// <param name="factura"></param>
        /// <param name="outerXml"></param>
        private static void CargarDomicilioReceptor(ref Factura factura, string outerXml)
        {
            try
            {
                XmlDocument DocXML = new XmlDocument();
                DocXML.LoadXml(outerXml);

                var domicilioReceptor = DocXML.GetElementsByTagName("cfdi:Domicilio");

                if (domicilioReceptor.Count > 0)
                {
                    factura.Receptor.Domicilio = new Domicilio() {
                        Calle = domicilioReceptor[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("calle")).Select(x => x.Value).FirstOrDefault(),
                        CodigoPotal = domicilioReceptor[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("codigopostal")).Select(x => x.Value).FirstOrDefault(),
                        Colonia = domicilioReceptor[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("colonia")).Select(x => x.Value).FirstOrDefault(),
                        Estado = domicilioReceptor[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("estado")).Select(x => x.Value).FirstOrDefault(),
                        Localidad = domicilioReceptor[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("localidad")).Select(x => x.Value).FirstOrDefault(),
                        Municipio = domicilioReceptor[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("municipio")).Select(x => x.Value).FirstOrDefault(),
                        NoExterior = domicilioReceptor[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("noexterior")).Select(x => x.Value).FirstOrDefault(),
                        NoInterior = domicilioReceptor[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("nointerior")).Select(x => x.Value).FirstOrDefault(),
                        Pais = domicilioReceptor[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("pais")).Select(x => x.Value).FirstOrDefault()
                    };
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Carga los datos que se encuentran dentro del apartado "DomiclioFiscal" en la seccion "Emisor" de la factura.
        /// </summary>
        /// <param name="factura"></param>
        /// <param name="outerXml"></param>
        private static void CargarDomicilioFiscalEmisor(ref Factura factura, string outerXml)
        {
            try
            {
                XmlDocument DocXML = new XmlDocument();
                DocXML.LoadXml(outerXml);

                var domicilioFiscal = DocXML.GetElementsByTagName("cfdi:DomicilioFiscal");

                if (domicilioFiscal.Count > 0)
                {
                    factura.Emisor.DomicilioFisal = new Domicilio() {
                        Calle = domicilioFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("calle")).Select(x => x.Value).FirstOrDefault(),
                        CodigoPotal = domicilioFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("codigopostal")).Select(x => x.Value).FirstOrDefault(),
                        Colonia = domicilioFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("colonia")).Select(x => x.Value).FirstOrDefault(),
                        Estado = domicilioFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("estado")).Select(x => x.Value).FirstOrDefault(),
                        Localidad = domicilioFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("localidad")).Select(x => x.Value).FirstOrDefault(),
                        Municipio = domicilioFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("municipio")).Select(x => x.Value).FirstOrDefault(),
                        NoExterior = domicilioFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("noexterior")).Select(x => x.Value).FirstOrDefault(),
                        NoInterior = domicilioFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("nointerior")).Select(x => x.Value).FirstOrDefault(),
                        Pais = domicilioFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("pais")).Select(x => x.Value).FirstOrDefault()
                    };
                }

                var expedidoEn = DocXML.GetElementsByTagName("cfdi:ExpedidoEn");

                if (expedidoEn.Count > 0)
                {
                    factura.Emisor.ExpedidoEn = new Domicilio()
                    {
                        Calle = expedidoEn[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("calle")).Select(x => x.Value).FirstOrDefault(),
                        CodigoPotal = expedidoEn[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("codigopostal")).Select(x => x.Value).FirstOrDefault(),
                        Colonia = expedidoEn[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("colonia")).Select(x => x.Value).FirstOrDefault(),
                        Estado = expedidoEn[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("estado")).Select(x => x.Value).FirstOrDefault(),
                        Localidad = expedidoEn[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("localidad")).Select(x => x.Value).FirstOrDefault(),
                        Municipio = expedidoEn[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("municipio")).Select(x => x.Value).FirstOrDefault(),
                        NoExterior = expedidoEn[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("noexterior")).Select(x => x.Value).FirstOrDefault(),
                        NoInterior = expedidoEn[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("nointerior")).Select(x => x.Value).FirstOrDefault(),
                        Pais = expedidoEn[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("pais")).Select(x => x.Value).FirstOrDefault()
                    };
                }

                var regimenFiscal = DocXML.GetElementsByTagName("cfdi:RegimenFiscal");

                if (regimenFiscal.Count > 0)
                {
                    factura.Emisor.RegimenFiscal = new RegimenFiscal() { Regimen = regimenFiscal[0].Attributes.Cast<XmlAttribute>().Where(x => x.Name.ToLower().Equals("regimen")).Select(x => x.Value).FirstOrDefault() };
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
