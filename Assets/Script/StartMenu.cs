using UnityEngine;
using System.Collections;
using Soomla.Store;

public class StartMenu : MonoBehaviour{

	// Use this for initialization
	void Start () 
	{
		if (!SoomlaStore.Initialized) 
		{
			SoomlaStore.Initialize (new KSEconomy ());
		}
		#if UNITY_ANDROID
		SoomlaStore.StartIabServiceInBg();
		#endif
	}

	public void onStart()
	{
		ScreenManager.instance.GoToScene("MainMenu");
		#if UNITY_ANDROID
		SoomlaStore.StopIabServiceInBg();
		#endif
	}
}
