﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Notification : MonoBehaviour 
{

	[HideInInspector]
	public delegate void OnClose();
	[HideInInspector]
	public OnClose onClose;
	
	protected AudioSource audioSource;
	protected Teacher data;
	protected GameObject toast;
	protected string currentToast;
	protected GameObject[] robots;
	protected Vector2 initialAnchoredPos;

	// Use this for initialization
	void Start () 
	{
		onClose += foo;

		audioSource = GameObject.FindObjectOfType<AudioSource>();
		if(audioSource==null)
		{
			audioSource = GameObject.Find("Main Camera").AddComponent<AudioSource>();
		}
		
		TextAsset tempTxt = (TextAsset)Resources.Load ("Texts/toastTexts");
		data = Teacher.LoadFromText(tempTxt.text);
		toast = GameObject.Find("Notification");
		robots = GameObject.FindGameObjectsWithTag("Robot");

		initialAnchoredPos = toast.GetComponent<RectTransform>().anchoredPosition;
	}

	void foo(){}
	
	public void showToast(string toastXMLName,float duration = -1)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+toastXMLName);

		currentToast = toastXMLName;

		if(duration < 0)
		{
			if(aC)
			{
				duration = aC.length;
			}
			else
			{
				duration = 3.0f;
			}
		}

		if(aC)
		{
			audioSource.PlayOneShot(aC);
		}

		StartCoroutine("hideToastWhenSoundEnd",new object[2]{duration,toastXMLName});

		changeText(toastXMLName);

	}

	public void showToast(string toastXMLName,AudioClip sound,float duration = -1)
	{
		currentToast = toastXMLName;

		if(duration < 0)
		{
			duration = sound.length;
		}

		audioSource.PlayOneShot(sound);
		StartCoroutine("hideToastWhenSoundEnd",new object[2]{duration,toastXMLName});
		
		changeText(toastXMLName);
		
	}
	
	protected void changeText(string notifyXMLName)
	{
		notify infTemp = new notify();

		infTemp = data.getNotifyByName(notifyXMLName);

		show(true);
		toast.GetComponentInChildren<Text>().text = infTemp.text;
	}
	
	protected void show(bool show,float delay = .5f)
	{
		if(!show)
		{
			toast.GetComponent<RectTransform>().DOAnchorPos(initialAnchoredPos,delay).OnComplete(onToastClosed).SetEase(Ease.InBack);
		}
		else
		{
			choseRobot();
			toast.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay).SetEase(Ease.OutBack);
		}
	}

	protected void onToastClosed()
	{
		onClose();
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

	IEnumerator hideToastWhenSoundEnd(object[] parms)
	{
		yield return new WaitForSeconds((float)parms[0]);
		if(string.Compare(currentToast,(string)parms[1])==0)
		{
			show(false);
		}
	}
}