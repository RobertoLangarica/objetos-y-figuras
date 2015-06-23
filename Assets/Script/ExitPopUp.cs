using UnityEngine;
using System.Collections;

public class ExitPopUp : MonoBehaviour {

	public static ExitPopUp instance;
	public bool popUp;

	// Use this for initialization
	void Start () {
		instance = this;
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void popUpActive()
	{
		if(!popUp)
		{
			popUp = true;
			gameObject.SetActive(true);
		}
		else
		{
			popUp = false;
			gameObject.SetActive(false);
		}
	}

	public void exit()
	{
		//borrar y mandarlo a la escena anterior
		ScreenManager.instance.showPrevScene();
		Debug.Log("Exit");
	}

	public void exitToPhoto()
	{
		//borrar y mandarlo a la escena anterior
		ScreenManager.instance.GoToScene("PhotoScene");
		Debug.Log("Exit");
	}

	public void continuar()
	{
		GameManager.lvlToPrepare = "Ship1";
		ScreenManager.instance.GoToScene("Gameplay");
		gameObject.SetActive(false);
	}
}
