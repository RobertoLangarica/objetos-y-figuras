using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[XmlRoot("papirolas")]
public class Levels
{
	//Niveles (se serializan como arreglo normal Level[] mas abajo)
	protected List<Level> _levels = new List<Level>();

	protected List<Level> _levels500 = new List<Level>();

	protected GroupLevel _gLevel;

	protected List<Photo> _photos = new List<Photo>();

	//Constructor existente para evitar problemas con XMLSerializer
	public Levels(){}


	public void Save(string path)
	{
		var serializer = new XmlSerializer(typeof(Levels));
		using(var stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}
	}
	
	public static Levels Load(string path)
	{
		var serializer = new XmlSerializer(typeof(Levels));
		using(var stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as Levels;
		}
	}
	
	//Loads the xml directly from the given string. Useful in combination with www.text.
	public static Levels LoadFromText(string text) 
	{
		var serializer = new XmlSerializer(typeof(Levels));
		return serializer.Deserialize(new StringReader(text)) as Levels;
	}

	//Convertimos la lista a arreglo para evitar errores usando List<T> con iOS
	[XmlArray("levels"),XmlArrayItem("level")]
	public Level[] levels
	{
		set{_levels = new List<Level>(value);}
		get{return _levels.ToArray();}
	}

	[XmlArray("exercise500"),XmlArrayItem("level")]
	public Level[] levels500
	{
		set{_levels500 = new List<Level>(value);}
		get{return _levels500.ToArray();}
	}
	
	[XmlElement("groupLevels")]
	public GroupLevel gLevel
	{
		set{_gLevel = value;}
		get{return _gLevel;}
	}

	//Convertimos la lista a arreglo para evitar errores usando List<T> con iOS
	[XmlArray("photos"),XmlArrayItem("photo")]
	public Photo[] photos
	{
		set{_photos = new List<Photo>(value);}
		get{return _photos.ToArray();}
	}

	/**
	 * Devuelve un arreglo con los niveles que tengan la dificultad indicada
	 * @param difficulty
	 * */
	public Level[] getLevelsFromDifficulty(int difficulty)	
	{
		List<Level> result = new List<Level>();

		foreach(Level l in _levels)
		{
			if(l.difficulty == difficulty)
			{
				result.Add(l);
			}
		}

		return result.ToArray();
	}

	public Level[] getLevelsFromPurchaseName(string name)	
	{
		List<Level> result = new List<Level>();
		
		foreach(Level l in _levels)
		{
			if(l.purchaseID == name)
			{
				result.Add(l);
			}
		}
		
		return result.ToArray();
	}

	public Level getLevelByName(string lvlName)
	{
		foreach(Level l in _levels)
		{
			if(l.name == lvlName)
			{
				return l;
			}
		}
		
		return null;
	}

	public Photo getPhotobyName(string photoName)
	{
		foreach(Photo p in _photos)
		{
			if(p.name == photoName)
			{
				return p;
			}
		}
		
		return null;
	}
}

