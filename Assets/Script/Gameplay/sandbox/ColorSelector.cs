using UnityEngine;
using System.Collections;

public class ColorSelector : MonoBehaviour {

	[HideInInspector]
	public Color selectedColor;

	void Awake()
	{
		selectedColor = new Color(1,0,0);
	}

	public void selectColor(int index)
	{
		switch(index)
		{
		case 1:
			//RED
			selectedColor.r = 1;
			selectedColor.g = 0;
			selectedColor.b = 0;
			break;
		case 2:
			//GREEN
			selectedColor.r = .4f;
			selectedColor.g = 1;
			selectedColor.b = .70f;
			break;
		case 3:
			//BLUE
			selectedColor.r = 0.56f;
			selectedColor.g = 0.82f;
			selectedColor.b = 1;
			break;
		case 4:
			//PINK
			selectedColor.r = 1;
			selectedColor.g = 0.6f;
			selectedColor.b = 0.93f;
			break;
		}
	}
}
