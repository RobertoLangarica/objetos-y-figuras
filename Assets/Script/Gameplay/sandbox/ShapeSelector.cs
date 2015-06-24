using UnityEngine;
using System.Collections;

public class ShapeSelector : MonoBehaviour {

	public GameObject square;
	public GameObject rectangle;
	public GameObject triangle;
	public GameObject rhomboid;
	public GameObject trapezium;

	public ColorSelector colorSelector;


	public void instantiateShape(string name)
	{
		switch(name)
		{
		case "square":
			GameObject.Instantiate(square);
			break;
		case "rectangle":
			GameObject.Instantiate(rectangle);
			break;
		case "triangle":
			GameObject.Instantiate(triangle);
			break;
		case "rhomboid":
			GameObject.Instantiate(rhomboid);
			break;
		case "trapezium":
			GameObject.Instantiate(trapezium);
			break;
		}
	}
}
