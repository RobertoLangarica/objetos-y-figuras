using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public float timeBeforeNextScreen = 2;
	public string nextScreen;

	// Use this for initialization
	void Start () 
	{
		ScreenManager.instance.GoToSceneAsync(nextScreen,timeBeforeNextScreen);
	}
}
