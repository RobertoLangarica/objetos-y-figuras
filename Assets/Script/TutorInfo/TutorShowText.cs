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
		shtutor.text = shtutor.text.Replace("()","<b>");
		shtutor.text = shtutor.text.Replace("(*)","</b>");
		
		text.text =  shtutor.text;
		title.text = shtutor.title;
		yield return new WaitForSeconds(.1f);
		if(title.cachedTextGenerator.fontSizeUsedForBestFit <50)
		{
			text.resizeTextMaxSize = 12;
		}
		yield return  new WaitForSeconds(.1f);
		panelButonShow.GetComponentInChildren<Button>().interactable=true;
		panelShowText.SetActive(false);
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
