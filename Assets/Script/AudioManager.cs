using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

	public AudioSource MainMusic;
	public AudioSource GameMusic;

	public AudioClip MainAudio0;
	public AudioClip MainAudio1;
	public AudioClip GameAudio;

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
		else if(Application.loadedLevel < 7)
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
			GameMusic.audio.clip = GameAudio;
			MainMusic.volume=0;
			GameMusic.volume=1;
			GameMusic.Play();
		}
	}
}
