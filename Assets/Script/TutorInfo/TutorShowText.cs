using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorShowText : MonoBehaviour {

	protected Teacher data;

	public GameObject panelShowText;
	public GameObject panelButonShow;
	public Text title;
	public Text	text;
	public Scrollbar scrollBar;
	public string textToShow ="";
	void Start()
	{
		if(!UserDataManager.instance.tutorMode)
		{
			this.gameObject.SetActive(false);
			return;
		}
		panelButonShow.GetComponentInChildren<Button>().interactable=false;

		//print(Screen.currentResolution.width);
		//print(Screen.currentResolution.height);

		StartCoroutine("lateStart");
	}


	IEnumerator lateStart()
	{
		yield return  new WaitForSeconds(.1f);
		//Debug.Log(title.cachedTextGenerator.fontSizeUsedForBestFit);

		TextAsset tempTxt = (TextAsset)Resources.Load ("Texts/toastTexts");
		
		//Ya eixste el archivo y solo checamos la version
		data = Teacher.LoadFromText(tempTxt.text);//Levels.Load(path);
		
		showTutorText shtutor;
		shtutor = data.getShowTutorTextByName(textToShow);
		shtutor.text = shtutor.text.Replace("@",System.Environment.NewLine);
		shtutor.title = shtutor.title.Replace("@",System.Environment.NewLine);
		shtutor.text = shtutor.text.Replace("()","<color=black>");
		shtutor.text = shtutor.text.Replace("(*)","</color>");
		
		text.text =  System.Environment.NewLine+shtutor.text;
		title.text = shtutor.title;
		yield return new WaitForSeconds(.1f);

		//if(title.cachedTextGenerator.fontSizeUsedForBestFit <50)
		//{
		//	text.resizeTextMaxSize = 12;
		//}
		//else if(title.cachedTextGenerator.fontSizeUsedForBestFit >100)
		//{
		//	text.resizeTextMaxSize =title.cachedTextGenerator.fontSizeUsedForBestFit;
		//	Debug.Log(title.cachedTextGenerator.fontSizeUsedForBestFit);
		//}
		text.resizeTextMaxSize =(int)(title.cachedTextGenerator.fontSizeUsedForBestFit*.5);
		Debug.Log(text.resizeTextMaxSize);
		yield return  new WaitForSeconds(.1f);
		panelButonShow.GetComponentInChildren<Button>().interactable=true;
		panelShowText.SetActive(false);
		panelShowText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
	}

	public void exitPopUp()
	{
		panelShowText.SetActive(false);
		panelButonShow.SetActive(true);
	}

	public void showPopUp()
	{
		panelShowText.SetActive(true);
		panelShowText.GetComponent<Image>().enabled = true;
		panelButonShow.SetActive(false);
		
		scrollBar.value =1;
	}
}
