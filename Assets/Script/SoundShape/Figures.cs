using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class Figures
{
	[XmlAttribute("name")]
	public string name;//Nombre del nivel
	
	//Piezas (se serializan como arreglo normal Piece[]
	protected List<Info> _infos = new List<Info>();

	//Constructor existente para evitar problemas con XMLSerializer
	public Figures()
	{}
	
	
	//Convertimos la lista a arreglo para evitar errores usando List<T> con iOS
	[XmlArray("infos"),XmlArrayItem("info")]
	public Info[] infos
	{
		set{_infos = new List<Info>(value);}
		get{return _infos.ToArray();}
	}

	public Info getInfoByName(string Name)
	{
		foreach(Info l in _infos)
		{
			if(l.name == Name)
			{
				return l;
			}
		}
		
		return null;
	}

}