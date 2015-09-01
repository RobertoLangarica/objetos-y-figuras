using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorShowText : MonoBehaviour {

	protected Teacher data;

	public GameObject panelShowText;
	public GameObject panelButonShow;
	public Text	text;
	public string textToShow ="";

	void Start()
	{
		if(!UserDataManager.instance.tutorMode)
		{
			//this.gameObject.SetActive(false);
		}
		TextAsset tempTxt = (TextAsset)Resources.Load ("Texts/toastTexts");
		
		//Ya eixste el archivo y solo checamos la version
		data = Teacher.LoadFromText(tempTxt.text);//Levels.Load(path);

		text.text =  data.getShowTutorTextByName(textToShow).text;
	}

	public void exitPopUp()
	{
		panelShowText.SetActive(false);
		panelButonShow.SetActive(true);
	}

	public void showPopUp()
	{
		panelShowText.SetActive(true);
		panelButonShow.SetActive(false);
	}
}
