using UnityEngine;
using System.Collections;

public class StartMenuServer : MonoBehaviour {

	// Use this for initialization
	public void onStart () {
		ScreenManager.instance.GoToScene("SpaceServer");
	}

}