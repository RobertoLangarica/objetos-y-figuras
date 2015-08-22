using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class Photo
{
	[XmlAttribute("name")]
	public string name;//Nombre del nivel

	[XmlAttribute("legal")]
	public string legal;

	//Constructor existente para evitar problemas con XMLSerializer
	public Photo()
	{}
}
