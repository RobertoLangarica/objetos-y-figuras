using UnityEngine;
using System.Collections;

public class BlockedScene : MonoBehaviour {

	public void onAccept()
	{
		if(ScreenManager.instance)
		{
			//Permitimos el regreso de pantallas
			ScreenManager.instance.backAllowed = true;
			ScreenManager.instance.GoToScene("GameMenu");
		}
	}
}
