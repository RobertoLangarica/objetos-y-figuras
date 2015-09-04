using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PiracyPopUp : MonoBehaviour 
{
	public Toggle tog;

	void Start()
	{
		if(UserDataManager.instance.showPopUp == false || UserDataManager.instance.startGame == false)
		{
			gameObject.SetActive(false);
		}
	}

	public void changeFirstRun()
	{
		if(tog.isOn)
		{
			UserDataManager.instance.showPopUp = false;
		}
		else
		{
			UserDataManager.instance.showPopUp = true;
		}
	}

	public void closePopUp()
	{
		gameObject.SetActive(false);
		UserDataManager.instance.startGame = false;
	}
}