using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class Piece
{
	[XmlAttribute("name")]
	public string name;//Nombre de la pieza

	[XmlAttribute("scale")]
	public string scale;
	
	[XmlAttribute("scaleCircle")]
	public string scaleCiclre;

	[XmlAttribute("scaleSquare")]
	public string scaleSquare;

	[XmlAttribute("radius")]
	public float radius;

	//Constructor existente para evitar problemas con XMLSerializer
	public Piece()
	{}
}

