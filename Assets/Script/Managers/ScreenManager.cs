using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {
	
	public static ScreenManager instance;
	public AudioSource music;
	protected string _prevScene;
	protected bool isAudioPlaying = false;
	
	void Awake()
	{
		DontDestroyOnLoad(this);
		instance = this;
		if(Application.loadedLevelName == "Intro")
		{
			//Cliente
			#if UNITY_EDITOR
			UserDataManager.instance.cleanData();
			GoToScene("SplashVilla");
			#else
			GoToScene("SplashVilla");
			#endif
		}
		else
		{
			//Server
		}

		if(Application.loadedLevelName == "ServerIntro")
		{
			//Cliente
			GoToScene("SplashServer");
		}
		else
		{
			//Server
		}
		transform.SetAsLastSibling ();
	}
	
	// Update is called once per frame
	void Update () {
		
		#if UNITY_ANDROID
		//Back nativo de android
		if (Input.GetKey(KeyCode.Escape))
		{
			showPrevScene();
		}
		#endif
		if (Input.GetKeyUp(KeyCode.B))
		{
			Debug.Log(previousScene);
			showPrevScene();
		}
	}
	
	public void showPrevScene()	
	{
		switch(Application.loadedLevelName)
		{
			case "MainMenu":
				//Application.Quit();
				//showPopUp

				if(ExitPopUp.instance.popUp == false)
				{	
					ExitPopUp.instance.popUpActive();
				}
				break;
			case "PhotoScene":
			case"ShipSelector":
				GoToScene("Test");
			break;

		}
	}

	public string previousScene
	{
		get{return _prevScene;}
		protected set{_prevScene=value;}
	}
	
	public void GoToScene(string newScene)
	{
		if (newScene == Application.loadedLevelName) 
		{
			return;
		}
		if (!isAudioPlaying && newScene == "StartMenu") 
		{
			isAudioPlaying = true;
			music.Play();
		}

		if(SceneFadeInOut.instance != null)
			SceneFadeInOut.instance.Fade();

		previousScene = Application.loadedLevelName;
		Application.LoadLevel (newScene);
	}
}
