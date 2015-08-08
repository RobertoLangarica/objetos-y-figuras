using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class GLevel
{
	[XmlAttribute("colorNum")]
	public int colorNum;
	
	[XmlAttribute("shapeNum")]
	public int shapeNum;
	
	[XmlAttribute("sizeNum")]
	public int sizeNum;
	
	[XmlAttribute("totalGroups")]
	public int totalGroups;
	
	public GLevel()
	{}
}