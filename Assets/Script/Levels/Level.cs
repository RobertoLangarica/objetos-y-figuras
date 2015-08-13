using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class Level
{
	[XmlAttribute("name")]
	public string name;//Nombre del nivel
	
	[XmlAttribute("referenceImage")]
	public string referenceImage;//imagen de referencia para cuando se completa el nivel
	
	[XmlAttribute("difficulty")]
	public int difficulty;//imagen de referencia para cuando se completa el nivel
	
	[XmlAttribute("error")]
	public float error;//Error aceptado en la distancia
	
	[XmlAttribute("purchaseID")]
	public string purchaseID;//El idea de la nave a que paquete pertenece

	[XmlAttribute("fType")]
	public string fType;//El idea de la nave a que paquete pertenece

	//Piezas (se serializan como arreglo normal Piece[]
	protected List<Piece> _pieces = new List<Piece>();

	//Solucion (se serializa como arreglo normal Pair[]
	protected List<Pair> _pairs = new List<Pair>();

	//Constructor existente para evitar problemas con XMLSerializer
	public Level()
	{}


	//Convertimos la lista a arreglo para evitar errores usando List<T> con iOS
	[XmlArray("pieces"),XmlArrayItem("piece")]
	public Piece[] pieces
	{
		set{_pieces = new List<Piece>(value);}
		get{return _pieces.ToArray();}
	}

	//Convertimos la lista a arreglo para evitar errores usando List<T> con iOS
	[XmlArray("solution"),XmlArrayItem("pair")]
	public Pair[] pairs
	{
		set{_pairs = new List<Pair>(value);}
		get{return _pairs.ToArray();}
	}
}

