using UnityEngine;
using System.Collections;

public class soundOver : MonoBehaviour {

	public AudioSource audioSource;

	public void selectSound(string soundToPlay)
	{
		bool sameName = false;
		if(GameObject.Find("AudioOver").GetComponent<AudioSource>())
		{
			audioSource = GameObject.Find("AudioOver").GetComponent<AudioSource>();
			audioSource.volume = 1;//0.5f;
		}


		if(audio.clip)
		{
			if(audioSource.isPlaying&&soundToPlay == audioSource.clip.name)
			{
				sameName = true;
			}
		}
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay);

		if(!sameName)
		{
			audioSource.clip = aC;
			audioSource.Play();
		}
	}
}
