using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour {

	public AudioSource MainMusic;
	public AudioSource GameMusic;
	public AudioSource SoundsAS;

	public AudioClip MainAudio0;
	public AudioClip MainAudio1;
	public AudioClip GameAudio;
	public AudioClip SoundsAC;
	void Awake()
	{
		MainMusic.loop = true;
		GameMusic.loop = true;
	}
	void OnLevelWasLoaded() {

		if(Application.loadedLevel < 3)
		{
			if(!MainMusic.isPlaying)
			{
				GameMusic.Stop();
			}
			MainMusic.volume=1;
			GameMusic.volume=0;

			MainMusic.audio.clip = MainAudio0;
			GameMusic.audio.clip = MainAudio1;

			if(!MainMusic.isPlaying)
			{
				MainMusic.Play();
				GameMusic.Play();
			}

			//Debug.Log("Sound1");
		}
		else if(Application.loadedLevel < 8)
		{
			MainMusic.volume=0;
			GameMusic.volume=1;
			if(!MainMusic.isPlaying)
			{
				MainMusic.audio.clip = MainAudio0;
				GameMusic.audio.clip = MainAudio1;
				MainMusic.Play();
				GameMusic.Play();
			}

			//Debug.Log("SOUND2");
		}
		else
		{
			MainMusic.Stop();
			MainMusic.volume=0;
			GameMusic.volume=1;
			GameMusic.audio.clip = GameAudio;
			if(!GameMusic.isPlaying)
			{
				GameMusic.Play();
			}


		}

		searchButtons();
	}

	protected void searchButtons()
	{
		GameObject[] buttons;
		buttons = GameObject.FindGameObjectsWithTag("UIButton");
		SoundsAS.audio.clip = SoundsAC;
		foreach(GameObject button in buttons)
		{
			if(button.GetComponent<Button>())
			{
				button.GetComponent<Button>().onClick.AddListener(() => { soundButton(); }); 
			}
		}
	}

	public void soundButton()
	{
		SoundsAS.Play();
	}
}
