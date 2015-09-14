using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[XmlRoot("shapes")]
public class Teacher
{
	//Niveles (se serializan como arreglo normal Level[] mas abajo)
	protected List<Figures> _figure = new List<Figures>();
	protected List<notify> _notify = new List<notify>();
	protected List<showTutorText> _showTutorText = new List<showTutorText>();
	//Constructor existente para evitar problemas con XMLSerializer
	public Teacher(){}
	
	
	public void Save(string path)
	{
		var serializer = new XmlSerializer(typeof(Teacher));
		using(var stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}
	}
	
	public static Teacher Load(string path)
	{
		var serializer = new XmlSerializer(typeof(Teacher));
		using(var stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as Teacher;
		}
	}
	
	//Loads the xml directly from the given string. Useful in combination with www.text.
	public static Teacher LoadFromText(string text) 
	{
		var serializer = new XmlSerializer(typeof(Teacher));
		return serializer.Deserialize(new StringReader(text)) as Teacher;
	}
	
	//Convertimos la lista a arreglo para evitar errores usando List<T> con iOS
	[XmlArray("teacher"),XmlArrayItem("shape")]
	public Figures[] figure
	{
		set{_figure = new List<Figures>(value);}
		get{return _figure.ToArray();}
	}

	[XmlArray("notification"),XmlArrayItem("notify")]
	public notify[] notify
	{
		set{_notify = new List<notify>(value);}
		get{return _notify.ToArray();}
	}

	[XmlArray("tutor"),XmlArrayItem("showTutorText")]
	public showTutorText[] showTutorText
	{
		set{_showTutorText = new List<showTutorText>(value);}
		get{return _showTutorText.ToArray();}
	}

	public Figures getFigureByName(string Name)
	{
		foreach(Figures l in _figure)
		{
			if(l.name == Name)
			{
				return l;
			}
		}
		
		return null;
	}

	public notify getNotifyByName(string Name)
	{
		foreach(notify l in _notify)
		{
			if(l.name == Name)
			{
				return l;
			}
		}
		
		return null;
	}
	
	public notify[] getNotifyArrByName(string Name)
	{
		List<notify> result = new List<notify>();
		foreach(notify l in _notify)
		{
			if(l.name == Name)
			{
				result.Add(l);
			}
		}
		
		return result.ToArray();
	}

	public showTutorText getShowTutorTextByName(string Name)
	{
		foreach(showTutorText l in _showTutorText)
		{
			if(l.name == Name)
			{
				return l;
			}
		}
		
		return null;
	}

}

