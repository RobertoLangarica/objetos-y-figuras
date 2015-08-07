using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class GroupLevel
{
	protected List<GLevel> _color = new List<GLevel>();
	protected List<GLevel> _shape = new List<GLevel>();
	protected List<GLevel> _size = new List<GLevel>();
	protected List<GLevel> _freestyle = new List<GLevel>();

	public GroupLevel()
	{}
	
	[XmlArray("byColor"),XmlArrayItem("gLevel")]
	public GLevel[] byColor
	{
		set{_color = new List<GLevel>(value);}
		get{return _color.ToArray();}
	}
	
	[XmlArray("byShape"),XmlArrayItem("gLevel")]
	public GLevel[] byShape
	{
		set{_shape = new List<GLevel>(value);}
		get{return _shape.ToArray();}
	}
	
	[XmlArray("bySize"),XmlArrayItem("gLevel")]
	public GLevel[] bySize
	{
		set{_size = new List<GLevel>(value);}
		get{return _size.ToArray();}
	}
	
	[XmlArray("freeStyle"),XmlArrayItem("gLevel")]
	public GLevel[] freeStyle
	{
		set{_freestyle = new List<GLevel>(value);}
		get{return _freestyle.ToArray();}
	}
}