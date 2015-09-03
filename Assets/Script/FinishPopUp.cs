using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class FinishPopUp : MonoBehaviour 
{
	public AudioSource audioSource;
	protected GameObject[] robots;

	// Use this for initialization
	void Start () 
	{
		transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
		transform.GetChild(0).GetComponent<Image>().enabled = false;

		robots = GameObject.FindGameObjectsWithTag("RobotPopUp");
	}

	public void onExit()
	{
		ScreenManager.instance.showPrevScene();
	}

	public void show()
	{
		choseRobot();
		transform.GetChild(0).GetComponent<Image>().enabled = true;
		transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().DOScale(new Vector3(1,1,1) ,1).SetEase(Ease.OutBack);

		if(audioSource)
		{
			audioSource.Play();
			Invoke("playVoice",audioSource.clip.length);
		}
	}

	protected void playVoice()
	{
		Debug.Log("Felicidades");
		AudioClip aC = (AudioClip)Resources.Load("Sounds/felicidades_has_completado_la_mision");
		if(aC)
		{
			Debug.Log("Play");
			Camera.main.audio.PlayOneShot(aC);
		}
	}

	protected void choseRobot()
	{
		int rand = Random.Range(0,3);
		for(int i =0; i<robots.Length; i++)
		{
			robots[i].SetActive(false);
			if(i==rand)
			{
				robots[i].SetActive(true);
			}
		}
	}
}
