using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public float timeBeforeNextScreen = 2;
	public string nextScreen;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
		timeBeforeNextScreen -= Time.deltaTime;
		if(timeBeforeNextScreen <= 0)
		{
			ScreenManager.instance.GoToScene(nextScreen);
		}
	}
}
