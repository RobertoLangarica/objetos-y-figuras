using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class notify
{
	
	[XmlAttribute("name")]
	public string name;//Nombre de la figura
	
	[XmlAttribute("text")]
	public string text;//info de la figura

	[XmlAttribute("idSound")]
	public int idSound;//id del sonido

	//Constructor existente para evitar problemas con XMLSerializer
	public notify()
	{}
	
}