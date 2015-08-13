using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class Piece
{
	[XmlAttribute("name")]
	public string name;//Nombre de la pieza

	[XmlAttribute("scale")]
	public string scale;//Nombre de la pieza

	//Constructor existente para evitar problemas con XMLSerializer
	public Piece()
	{}
}

