using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class Info
{

	[XmlAttribute("name")]
	public string name;//Nombre de la figura
	
	[XmlAttribute("text")]
	public string text;//info de la figura
	
	//Constructor existente para evitar problemas con XMLSerializer
	public Info()
	{}
}
