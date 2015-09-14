using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PiracyPopUp : MonoBehaviour 
{
	public Toggle tog;
	public TermsAndConditions terms;
	void Start()
	{
		#if UNITY_STANDALONE
		if(!UserDataManager.instance.isAPirateGame && (UserDataManager.instance.showPopUp == false || UserDataManager.instance.startGame == false))
		{
			gameObject.SetActive(false);
		}
		#endif

		#if UNITY_ANDROID || UNITY_IOS
		gameObject.SetActive(false);
		#endif
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

		if(UserDataManager.instance.isAPirateGame)
		{
			ScreenManager.instance.GoToScene("Validation");
		}

		//if(UserDataManager.instance.TermsAccepted)
		//{
		//	terms.openTerms();
		//}
	}

	public void toCuriosamente()
	{
		Application.OpenURL("http://www.curiosamente.com/terminos-y-condiciones");
	}

	public void toRegistro()
	{
		Application.OpenURL("http://www.curiosamente.com/registro");
	}
}