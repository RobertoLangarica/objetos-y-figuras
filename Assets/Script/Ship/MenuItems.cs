using UnityEngine;
using System.Collections;

public class MenuItems : MonoBehaviour {

	public string lvlName;
	public string lvlPurchseID;

	public void onClick()
	{
		SpacegramManager.lvlToPrepare = lvlName;
		ScreenManager.instance.GoToScene ("Spacegram");
	}
}
