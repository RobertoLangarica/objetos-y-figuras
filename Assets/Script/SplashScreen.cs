using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public float timeBeforeNextScreen = 2;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		timeBeforeNextScreen -= Time.deltaTime;
		if(timeBeforeNextScreen <= 0)
		{
			switch (ScreenManager.instance.previousScene) 
			{
			case "SpaceClient":
				UserDataManager.instance.cleanData();
				ScreenManager.instance.GoToScene("SplashVilla");
				break;
			case "Intro":
				ScreenManager.instance.GoToScene("SplashVilla");
				break;
			case "Splash":
				UserDataManager.instance.cleanData();
				ScreenManager.instance.GoToScene("StartMenu");
				break;
			case "ServerIntro":
				ScreenManager.instance.GoToScene("SplashVillaServer");
				break;
			case "SplashServer":
				ScreenManager.instance.GoToScene("StartMenuServer");
				break;
			}

		}
	}
}
