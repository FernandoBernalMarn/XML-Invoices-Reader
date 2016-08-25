# XML-Invoices-Reader.
This library provide an easy way to read invoices on .xml files, based on the Anexo 20 of the Servicio de Administración Tributarioa (SAT - México).

_For more information go to [Wiki](https://github.com/FernandoBernalMarn/XML-Invoices-Reader/wiki)_

## Installation
[Nuget](https://www.nuget.org/packages/XML-Invoices-Reader/) (_version 2.0.0_)
```
PM > Install-Package XML-Invoices-Reader
```

##Entities Structure:
This is the structure to Entities, contains the most significant attributes of the XML files, the attributes mark as "Required" is the minium information that appear in the entities.

### Concepto:
```
Cantidad (Required, Type: decimal).
Unidad (Required, Type: string).
Desceipcion (Required, Type: string).
ValorUnitario (Required, Type: decimal).
```

### Conceptos:
```
ListaConceptos (Optional, Type: List <Concepto>).
```
	
### Domicilio:
```
Calle (Required, Type: string).
CodigoPotal (Required, Type: string).
Colonia (Optional, Type: string).
Estado (Required, Type: string).
Localidad (Optional, Type: string).
Municipio (Required, Type: string).
NoExterior (Optional, Type: string).
NoInterior (Optional, Type: string).
Pais (Required, Type: string).
```
    
### Emisor:
```
RCF (Required).
Nombre (Optional).
Domicilio Fiscal (Optional, Type: Entity(Domicilio)).
ExpedidEn (Optional, Type: Entity(Domicilio)).
RegimenFiscal (Optional, Type: Entity(RegimenFiscal)).
```
	
### Factura:
```
Conceptos (Optional, Type: List<Concepto>).
DatosFactura (Required, Type: Entity(FacturaXML)).
Emisor (Required, Type: Entity(Emisor)).
Impuestos (Required, Type: Entity(Impuestos)).
NombreArchivo (Required, Type: string).
```
	
### FacturaXML: 
```
Serie (Optional, Type: string).
Folio (Optional, Type: string).
Fecha (Required, Type: DateTime).
Sello (Required, Type: string).
FormaDePago (Required, Type: string).
NoCertificado (Required, Type: string).
Certificado (Required, Type: string).
CondicionesDePago (Optional, Type: string).
Subtotal (Optional, Type: decimal).
Moneda (Optional, Type: string).
Total (Required, Type: decimal).
MetodoDePago (Required, Type: string).
LugarExpedicion (Required, Type: string).
```
	  
### Impuestos:
```
Retenciones (Optional, Type: List<Retencion>).
Traslados (Optional, Type: List<Traslado>).
TotalImpuestosRetenidos (Optional, Type: string).
TotalImpuestosTrasladados (Optional, Type: string).
```
		
### Receptor:
```
RFC (Required, Type: string).
Nombre (Optional, Type: string).
Domicilio (Optional, Type: Entity(Domicilio)).
```

###RegimenFiscal:
```
Regimen (Required, Type: string).
```
  	
### Retencion:
```
Importe (Required, Type: decimal).
Impuesto (Required, Type: string).
```
		
###Traslado:
```
Importe (Required, Type: decimal).
Impuesto (Required, Type: string).
Tasa (Required, Type: decimal).
```
##Usage
``` c#
using LectorFacturasXML;
using LectorFacturasXML.Entidades;
using System.IO;
using System.Xml;

// From Local File
Factura invoiceLocalFile = FromLocalFile.GetData(@"..\localPathXmlFile.xml");

// From String 
string xmlString = File.ReadAllText(@"..\localPathXmlFile.xml");
Factura invoiceString = FromString.GetData(xmlString, "FileName");

// From XmlDocument
XmlDocument xmlDoc = new XmlDocument();
xmlDoc.Load(@"..\localPathXmlFile.xml");

Factura invoiceXmlDocument = ReadXML.GetData(xmlDoc, "FileName");
```
