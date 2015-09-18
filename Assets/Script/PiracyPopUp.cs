using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class PiracyPopUp : MonoBehaviour 
{
	public Toggle tog;
	public TermsAndConditions terms;
	public Transform hidePos;
	public GameObject pcPopUp;
	public GameObject mobilePopUp;

	void Start()
	{
		#if UNITY_STANDALONE
		mobilePopUp.SetActive(false);
		if(!UserDataManager.instance.isAPirateGame && (UserDataManager.instance.showPopUp == false || UserDataManager.instance.startGame == false))
		{
			gameObject.SetActive(false);
		}
		#endif

		#if UNITY_ANDROID || UNITY_IOS
		pcPopUp.SetActive(false);
		if(UserDataManager.instance.showPopUp == false || UserDataManager.instance.startGame == false)
		{
			gameObject.SetActive(false);
		}
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
		//gameObject.SetActive(false);
		UserDataManager.instance.startGame = false;
		gameObject.GetComponent<Image>().enabled = false;
		gameObject.GetComponent<RectTransform>().DOScale(Vector3.zero,0.5f).SetEase(Ease.InBack);
		gameObject.GetComponent<RectTransform>().DOMove(hidePos.position,0.5f).SetEase(Ease.InBack).OnComplete(()=>{
			#if UNITY_STANDALONE
			if(UserDataManager.instance.isAPirateGame)
			{
				ScreenManager.instance.GoToScene("Validation");
			}
			#endif
		});
	}

	public void showPopUp()
	{
		tog.isOn = UserDataManager.instance.showPopUp;

		gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0),0.5f).SetEase(Ease.OutBack);
		gameObject.GetComponent<RectTransform>().DOScale(new Vector3(1,1,1),0.5f).SetEase(Ease.OutBack).OnComplete(()=>{gameObject.GetComponent<Image>().enabled = true;});
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