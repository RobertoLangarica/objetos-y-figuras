using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {
	public static ScreenManager instance;

	public string firstScreenName;//Primer pantalla que se muestra en el juego
	public string firstEditorScreen;//Primer pantalla que se muestra en el editor
	public string screenBeforeClose;//Pantalla que no tiene back (cierra la app)
	public AudioSource music;

	protected bool isAudioPlaying = true;
	protected Dictionary<string,string> backScreens;

	void Awake()
	{
		DontDestroyOnLoad(this);
		instance = this;

		backScreens = new Dictionary<string, string>();

		//Cliente
		#if UNITY_EDITOR
		UserDataManager.instance.cleanData();
		GoToScene(firstEditorScreen);
		#else
		GoToScene(firstScreenName);
		#endif
	
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

		if (Input.GetKeyUp(KeyCode.Escape))
		{
			showPrevScene();
		}
	}
	
	public void showPrevScene()	
	{

		if(Application.loadedLevelName == screenBeforeClose)
		{
			Application.Quit();
		}
		else
		{
			//Mostramos la pantalla anterior
			if(backScreens.ContainsKey(Application.loadedLevelName))
			{
				string name = Application.loadedLevelName;
				GoToScene(backScreens[name]);
				backScreens.Remove(name);
			}
		}
	}

	public void GoToScene(string newScene)
	{
		if (newScene == Application.loadedLevelName) 
		{
			return;
		}

		if (!isAudioPlaying && newScene == firstEditorScreen) 
		{
			isAudioPlaying = true;
			//music.Play();
		}

		if(SceneFadeInOut.instance != null)
			SceneFadeInOut.instance.Fade();

		if(!backScreens.ContainsKey(newScene))
		{
			backScreens.Add(newScene,Application.loadedLevelName);
		}

		Debug.Log (newScene);
		Application.LoadLevel (newScene);
	}
}
