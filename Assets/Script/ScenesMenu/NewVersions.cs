using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewVersions : MonoBehaviour {

	public void selectConoce()
	{
		ScreenManager.instance.GoToScene("Conoce");
	}

	public void selectConstruye()
	{
		ScreenManager.instance.GoToScene("Construye");
	}

	public void selectAgrupa()
	{
		ScreenManager.instance.GoToScene("Agrupa");
	}
	
	public void selectOrdena()
	{
		ScreenManager.instance.GoToScene("Ordena");
	}
}
