using UnityEngine;
using System.Collections;

public class ScreenSelector : MonoBehaviour {

	public void selectNewScene(string sceneToGo)
	{
		ScreenManager.instance.GoToScene(sceneToGo);
	}
}
