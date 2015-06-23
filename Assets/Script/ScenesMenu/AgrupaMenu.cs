using UnityEngine;
using System.Collections;

public class AgrupaMenu : MonoBehaviour 
{
	public void selectFigura()
	{
		ScreenManager.instance.GoToScene("FiguraScene");
	}
	
	public void selectTamaño()
	{
		ScreenManager.instance.GoToScene("TamañoScene");
	}

	public void selectColor()
	{
		ScreenManager.instance.GoToScene("ColorScene");
	}
}
