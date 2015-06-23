using UnityEngine;
using System.Collections;

public class ConstruyeMenu : MonoBehaviour {

	public void selectSandBox()
	{
		ScreenManager.instance.GoToScene("SandBoxScene");
	}
	
	public void selectTangram()
	{
		ScreenManager.instance.GoToScene("TangramScene");
	}
	public void selectSpacegram()
	{
		ScreenManager.instance.GoToScene("MainMenu");
	}
}
