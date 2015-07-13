﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundShapeManager : MonoBehaviour {

	public AudioSource audioSource;
	public Text txt;
	public GameObject popUp;
	protected Teacher data;
	protected bool isDrawing;
	public GameObject DrawTool;

	void Start()
	{
		TextAsset tempTxt = (TextAsset)Resources.Load ("Levels/soundShapes");
		
		//Ya eixste el archivo y solo checamos la version
		data = Teacher.LoadFromText(tempTxt.text);//Levels.Load(path);
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			popUp.SetActive(false);
		}
	}

	public void selectSound(string soundToPlay)
	{

		#if TEACHER_MODE
			//PopUp
			question(soundToPlay);
		#else
		if(!DrawTool.GetComponent<DrawingInput>().canDraw)
		{
			AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay);
			audioSource.clip = aC;
			audioSource.Play();
		}
		#endif
	}
	protected void question(string textToPlay)
	{
		string number = "";
		string shape = "";
		Info infTemp = new Info();

		number = textToPlay.Substring(textToPlay.IndexOf('_')+1);
		shape = textToPlay.Substring(0,textToPlay.IndexOf('_'));

		infTemp = data.getFigureByName(shape).getInfoByName(number);
		Figures[] figure = data.figure;

		Debug.Log(infTemp.text);

		txt.text = infTemp.text;
		popUp.SetActive(true);
	}


}
