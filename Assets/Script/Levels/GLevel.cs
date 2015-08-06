using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class GLevel
{
	[XmlAttribute("colorNum")]
	public string colorNum;
	
	[XmlAttribute("shapeNum")]
	public string shapeNum;
	
	public GLevel()
	{}
}