using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerminosCondiciones : MonoBehaviour {

	public GameObject TYC;
	public Text textSize;
	public Text txtWantedSize;
	public Scrollbar sB;

	// Use this for initialization
	void Start () {
		
		StartCoroutine("lateStart");
	}

	public void openTerms()
	{

		TYC.SetActive(true);
	}

	IEnumerator lateStart()
	{
		yield return new WaitForSeconds(.01f);
		Debug.Log(txtWantedSize.cachedTextGenerator.fontSizeUsedForBestFit);
		int txtSizeValue = txtWantedSize.cachedTextGenerator.fontSizeUsedForBestFit;
		Debug.Log(txtWantedSize.preferredHeight);
		textSize.resizeTextMaxSize = txtSizeValue;
		txtWantedSize.text="";
		yield return new WaitForSeconds(.01f);
		sB.value=1;
		
		TYC.SetActive(false);
		TYC.GetComponent<RectTransform>().anchoredPosition =new Vector2 (0,0);
		//rt.sizeDelta = new Vector2(0, txt.preferredHeight);
	}
	
	public void termsAnswer(bool accepted)
	{
		if(accepted)
		{
			//closePopUp
			TYC.SetActive(false);
		}
		else
		{
			Application.Quit();
		}
	}
}
