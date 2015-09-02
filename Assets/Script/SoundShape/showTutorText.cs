using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class showTutorText
{
	
	[XmlAttribute("name")]
	public string name;//Nombre de la figura
	
	[XmlAttribute("text")]
	public string text;//info de la figura

	[XmlAttribute("idSound")]
	public string idSound;//id del sonido

	[XmlAttribute("title")]
	public string title;//titulo

	[XmlAttribute("description")]
	public string description;//description

	//Constructor existente para evitar problemas con XMLSerializer
	public showTutorText()
	{}
	
}