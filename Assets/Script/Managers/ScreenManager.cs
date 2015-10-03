﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {
	public static ScreenManager instance;

	public string firstScreenName;//Primer pantalla que se muestra en el juego
	public string firstEditorScreen;//Primer pantalla que se muestra en el editor
	public string screenBeforeClose;//Pantalla que no tiene back (cierra la app)
	public AudioSource music;

	[HideInInspector]
	public bool blocked = false;
	[HideInInspector]
	public bool backAllowed = true;

	protected float waitTime;
	protected string waitScreen;

	protected bool isAudioPlaying = true;
	protected Dictionary<string,string> backScreens;

	protected float timeBeforeNextScreen;
	protected AsyncOperation waitingScreen = null;
	protected int framesBeforeSwitch;

	void Awake()
	{
		DontDestroyOnLoad(this);
		instance = this;

		backScreens = new Dictionary<string, string>();

		//Solo que se inicialize user data
		UserDataManager.instance.foo();
		//UserDataManager.instance.cleanData();
	
		transform.SetAsLastSibling();
	}

	void Start()
	{
		//Cliente
		#if UNITY_EDITOR
		GoToScene(firstEditorScreen);
		#else
		GoToScene(firstScreenName);
		#endif
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


		if(waitingScreen != null)
		{
			timeBeforeNextScreen -= Time.deltaTime;

			if(timeBeforeNextScreen <= 0)
			{
				//AsyncOperation no reporta 100% o isDone == true hasta que se permite activar
				if(waitingScreen.progress >= 0.9f && --framesBeforeSwitch < 0)
				{
					if(SceneFadeInOut.instance != null)
					{
						SceneFadeInOut.instance.Fade();
					}
					
					waitingScreen.allowSceneActivation = true;
					waitingScreen = null;

					//No se pueden encimar estas 2 acciones
					if(blocked)
					{
						//Cambia de pantalla con delay
						StartCoroutine("waitForScreen");
					}
				}
			}
		}
	}
	
	public void showPrevScene()	
	{
		if(blocked || !backAllowed){return;}

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
		if(blocked || waitingScreen != null || newScene == Application.loadedLevelName)
		{
			return;
		}

		if (!isAudioPlaying && (newScene == firstEditorScreen || newScene == firstScreenName) ) 
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

		Application.LoadLevel (newScene);
	}


	public void GoToSceneAsync(string newScene,float waitTime = -1, int waitFrames = 10)
	{	
		if(blocked){return;}

		if(newScene == Application.loadedLevelName)
		{
			return;
		}

		if (!isAudioPlaying && (newScene == firstEditorScreen || newScene == firstScreenName) ) 
		{
			isAudioPlaying = true;
		}

		if(!backScreens.ContainsKey(newScene))
		{
			backScreens.Add(newScene,Application.loadedLevelName);
		}

		timeBeforeNextScreen = waitTime;
		framesBeforeSwitch = waitFrames;

		waitingScreen = Application.LoadLevelAsync(newScene);
		waitingScreen.allowSceneActivation = false;
	}

	public void GoToSceneDelayed(string newScene, float delay = 5)
	{
		blocked = true;
		waitScreen = newScene;
		waitTime = delay;

		//Hay un nivel asincrono cargando?
		//No se pueden encimar las acciones
		if(waitingScreen == null)
		{
			StartCoroutine("waitForScreen");
		}
	}

	IEnumerator waitForScreen()
	{
		yield return new WaitForSeconds(1.5f);
		blocked = false;
		GoToScene(waitScreen);
	}
	
}
