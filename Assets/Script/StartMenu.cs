using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public void onStart()
	{
		ScreenManager.instance.GoToScene("PhotoScene");
	}
}
