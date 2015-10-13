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

	protected float volumen=0.6f;
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
			MainMusic.volume=volumen;
			GameMusic.volume=0;

			MainMusic.GetComponent<AudioSource>().clip = MainAudio0;
			GameMusic.GetComponent<AudioSource>().clip = MainAudio1;

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
			GameMusic.volume=volumen;
			if(!MainMusic.isPlaying)
			{
				MainMusic.GetComponent<AudioSource>().clip = MainAudio0;
				GameMusic.GetComponent<AudioSource>().clip = MainAudio1;
				MainMusic.Play();
				GameMusic.Play();
			}

			//Debug.Log("SOUND2");
		}
		else
		{
			MainMusic.Stop();
			MainMusic.volume=0;
			GameMusic.volume=volumen;
			GameMusic.GetComponent<AudioSource>().clip = GameAudio;
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
		SoundsAS.GetComponent<AudioSource>().clip = SoundsAC;
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
