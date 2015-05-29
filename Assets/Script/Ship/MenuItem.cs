using UnityEngine;
using System.Collections;

public class MenuItem : MonoBehaviour {

	public string lvlName;

	public void onClick()
	{
		GameManager.lvlToPrepare = lvlName;
		ScreenManager.instance.GoToScene("Gameplay");
	}
}
