using UnityEngine;
using System.Collections;

public class MenuItems : MonoBehaviour {

	public string lvlName;

	public void onClick()
	{
		GameManager.lvlToPrepare = lvlName;
		ScreenManager.instance.GoToScene("Gameplay");
	}
}
