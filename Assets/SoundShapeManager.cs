using UnityEngine;
using System.Collections;

public class SoundShapeManager : MonoBehaviour {

	public AudioSource audioSource;
	
	public void selectSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay);
		Debug.Log(aC);
		audioSource.clip = aC;
		audioSource.Play();
	}
}
