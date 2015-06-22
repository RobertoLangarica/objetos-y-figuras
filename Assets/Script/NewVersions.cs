using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewVersions : MonoBehaviour {

	public void selectEasy()
	{
		GameManager.isEasy = true;
		ScreenManager.instance.GoToScene("MainMenu");
	}

	public void selectNormal()
	{
		GameManager.isEasy = false;
		ScreenManager.instance.GoToScene("MainMenu");
	}
}
