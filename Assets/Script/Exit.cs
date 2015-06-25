using UnityEngine;
using System.Collections;

public class Exit: MonoBehaviour {

	public void exit()
	{
		ScreenManager.instance.showPrevScene();
	}
}
