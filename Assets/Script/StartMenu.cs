using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class StartMenu : MonoBehaviour
{
	public Button shopButton;

	// Use this for initialization
	void Start () 
	{
		
		shopButton.gameObject.SetActive(false);
		
		#if UNITY_ANDROID
		shopButton.gameObject.SetActive(true);

		if (!SoomlaStore.Initialized) 
		{
			SoomlaStore.Initialize (new KSEconomy ());
			
			SoomlaStore.RefreshInventory ();
			
			SoomlaStore.StartIabServiceInBg();
		}
		#endif

		#if UNITY_IOS
		shopButton.gameObject.SetActive(true);
		if (!SoomlaStore.Initialized) 
		{
			SoomlaStore.Initialize (new KSEconomy ());
		}
		#endif


		#if UNITY_STANDALONE_WIN
		UserDataManager.instance.premiumVersion = "premiumVersion";
		#endif
	}

	public void onStart()
	{
		#if UNITY_ANDROID
		if (SoomlaStore.Initialized) 
		{
			SoomlaStore.StopIabServiceInBg();
		}
		#endif
		ScreenManager.instance.GoToScene("GameMenu");
	}
}
