using UnityEngine;
using System.Collections;

public class ColorSelector : MonoBehaviour {

	[HideInInspector]
	public Color selectedColor;

	void Awake()
	{
		selectedColor = new Color();
	}

	public void selectColor(int index)
	{
		switch(index)
		{
		case 1:
			//RED
			break;
		case 2:
			//GREEN
			break;
		case 3:
			//BLUE
			break;
		case 4:
			//PINK
			break;
		}
	}
}
