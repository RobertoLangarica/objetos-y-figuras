using UnityEngine;
using System.Collections;

public class soundOver : MonoBehaviour {

	public AudioSource audioSource;

	public void selectSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay);

		//if(!audioSource.isPlaying)
		{
			audioSource.clip = aC;
			audioSource.Play();
		}
	}
}
