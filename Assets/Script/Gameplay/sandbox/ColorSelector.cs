using UnityEngine;
using System.Collections;

public class ColorSelector : MonoBehaviour {

	public enum EShapeColor
	{
		AQUA,
		BLACK,
		BLUE,
		GREEN,
		GREY,
		MAGENTA,
		RED,
		WHITE,
		YELLOW
	};

	[HideInInspector]
	public int selectedColor;

	void Awake()
	{
		selectedColor = 0;
	}
	
	public EShapeColor selectColor(int index)
	{
		Debug.Log(index);
		switch(index)
		{
		case 1:
			return EShapeColor.AQUA;
			break;
		case 2:
			return EShapeColor.BLACK;
			break;
		case 3:
			return EShapeColor.BLUE;
			break;
		case 4:
			return EShapeColor.GREEN;
			break;
		case 5:
			return EShapeColor.GREY;
			break;
		case 6:
			return EShapeColor.MAGENTA;
			break;
		case 7:
			return EShapeColor.RED;
			break;
		case 8:
			return EShapeColor.WHITE;
			break;
		case 9:
			return EShapeColor.YELLOW;
			break;
		}
		return EShapeColor.AQUA;
	}
}
