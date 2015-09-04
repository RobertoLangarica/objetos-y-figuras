using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorScreen : MonoBehaviour {

	protected bool go = true;
	void Start()
	{
		Toggle toggle = GameObject.Find("Toggle").GetComponent<Toggle>();

		if(toggle)
		{
			if(toggle.isOn !=UserDataManager.instance.tutorMode)
			{
				go=false;
				toggle.isOn = UserDataManager.instance.tutorMode;
			}
		}
	}

	public void activateTutor()
	{
		if(!go)
		{
			go = true;
			return;
		}
		if(UserDataManager.instance.tutorMode)
		{
			UserDataManager.instance.tutorMode = false;
		}
		else
		{
			UserDataManager.instance.tutorMode = true;
		}
	}

	public void exit()
	{
		ScreenManager.instance.showPrevScene();
	}
}
