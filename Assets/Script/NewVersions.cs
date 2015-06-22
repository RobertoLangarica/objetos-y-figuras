using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewVersions : MonoBehaviour {

	public bool isEasy;
	public Sprite easyActivate;
	public Sprite normalActivate;
	public Image img;
	public Text txt;

	void Start()
	{
		if(GameManager.isEasy)
		{
			isEasy = true;
			txt.text = "Facil";
			img.sprite = easyActivate;
			GameManager.isEasy = true;
		}
	}

	public void changeDificulty()
	{
		if(!isEasy)
		{
			isEasy = true;
			txt.text = "Facil";
			img.sprite = easyActivate;
			GameManager.isEasy = true;
		}
		else
		{
			isEasy = false;
			txt.text = "Normal";
			img.sprite = normalActivate;
			GameManager.isEasy = false;
		}
	}
}
