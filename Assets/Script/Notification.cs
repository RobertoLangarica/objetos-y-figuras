using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	public bool showing;
	protected Question question;
	// Use this for initialization
	void Start () 
	{
		question = Object.FindObjectOfType<Question>();
		onClose += foo;

		if(GameObject.Find("Main Camera").GetComponent<AudioSource>()==null)
		{
			audioSource = GameObject.Find("Main Camera").AddComponent<AudioSource>();
		}
		else
		{
			audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		}
		
		TextAsset tempTxt = (TextAsset)Resources.Load ("Texts/toastTexts");
		data = Teacher.LoadFromText(tempTxt.text);
		toast = GameObject.Find("Notification");
		robots = GameObject.FindGameObjectsWithTag("Robot");

		toast.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-Screen.height);
		initialAnchoredPos = toast.GetComponent<RectTransform>().anchoredPosition;
	}

	void foo(){}
	
	public void showToast(string toastXMLName,float duration = -1)
	{
		notify[] result = data.getNotifyArrByName(toastXMLName);
		int rdm = Random.Range(0,result.Length);
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+result[rdm].idSound);

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
		if(question)
		{
			question.audioSource.Stop();
		}
		if(aC)
		{
			audioSource.PlayOneShot(aC,1);
		}

		StartCoroutine("hideToastWhenSoundEnd",new object[2]{duration,toastXMLName});

		changeText(result[rdm].text);

	}

	public void showToast(string toastXMLName,AudioClip sound,float duration = -1)
	{
		notify[] result = data.getNotifyArrByName(toastXMLName);
		int rdm = Random.Range(0,result.Length);
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+result[rdm].idSound);

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
		if(question)
		{
			question.audioSource.Stop();
		}
		if(aC)
		{
			audioSource.PlayOneShot(aC,1);
		}

		if(sound)
		{
			audioSource.PlayOneShot(sound,1);
		}

		StartCoroutine("hideToastWhenSoundEnd",new object[2]{duration,toastXMLName});
		
		changeText(result[rdm].text);
		
	}
	
	protected void changeText(string notifyXMLName)
	{
		notify infTemp = new notify();

		infTemp = data.getNotifyByName(notifyXMLName);

		show(true);
		toast.GetComponentInChildren<Text>().text = notifyXMLName;
	}
	
	protected void show(bool show,float delay = .5f)
	{
		if(!show)
		{
			showing = false;
			toast.GetComponent<RectTransform>().DOAnchorPos(initialAnchoredPos,delay).OnComplete(onToastClosed).SetEase(Ease.InBack);
		}
		else
		{
			if(question)
			{
				//Debug.Log("S");
				question.showToast(false);
				//question.audioSource.Stop();
			}
			showing=true;
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