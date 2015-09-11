using UnityEngine;
using System.Collections;

public class soundOver : MonoBehaviour {

	public AudioSource audioSource;

	public void selectSound(string soundToPlay)
	{
		bool sameName = false;
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
