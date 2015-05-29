using System.Xml;
using System.Xml.Serialization;

public class Pair
{
	[XmlAttribute("piece")]
	public int piece;

	[XmlAttribute("shapes")]
	public string shapes;

	[XmlAttribute("angles")]
	public string angles;

	[XmlAttribute("range")]
	public int range;

	//Constructor existente para evitar problemas con XMLSerializer
	public Pair()
	{}
}

