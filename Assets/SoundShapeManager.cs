using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundShapeManager : MonoBehaviour {

	public AudioSource audioSource;
	public Text txt;
	public GameObject popUp;

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			popUp.SetActive(false);
		}
	}

	public void selectSound(string soundToPlay)
	{

		#if TEACHER_MODE
		//PopUp

		#else
			AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay);
			Debug.Log(aC);
			audioSource.clip = aC;
			audioSource.Play();
		#endif
	}
	public void question(string soundToPlay)
	{
		TextAsset tempTxt = (TextAsset)Resources.Load ("Levels/levels");
		txt.text = tempTxt.text;
		popUp.SetActive(true);
	}
}
