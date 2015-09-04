using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PiracyPopUp : MonoBehaviour 
{
	void Start()
	{

		if(!UserDataManager.instance.isAPirateGame && (UserDataManager.instance.showPopUp == false || UserDataManager.instance.startGame == false))
		{
			gameObject.SetActive(false);
		}
	}

	public void changeFirstRun()
	{
		Toggle tog = gameObject.GetComponent<Toggle>();

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

		if(UserDataManager.instance.isAPirateGame)
		{
			ScreenManager.instance.GoToScene("Validation");
		}
	}
}