using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorScreen : MonoBehaviour {

	protected bool go = true;
	void Start()
	{

		if(GameObject.Find("Toggle").GetComponent<Toggle>().isOn !=UserDataManager.instance.tutorMode)
		{
			go=false;
			GameObject.Find("Toggle").GetComponent<Toggle>().isOn = UserDataManager.instance.tutorMode;
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
