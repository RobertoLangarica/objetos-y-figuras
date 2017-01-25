using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public float timeBeforeNextScreen = 2;
	public string nextScreen;
	public GameObject pcVer;
	public GameObject mobVer;
	public bool forceMobileVersion;

	// Use this for initialization
	void Start () 
	{
		ScreenManager.instance.GoToSceneAsync(nextScreen,timeBeforeNextScreen);

		if(forceMobileVersion)
		{
			pcVer.SetActive(false);
		}
		else
		{
			#if UNITY_STANDALONE
			mobVer.SetActive(false);
			#endif

			#if UNITY_ANDROID || UNITY_IOS
			pcVer.SetActive(false);
			#endif	
		}
	}
}
