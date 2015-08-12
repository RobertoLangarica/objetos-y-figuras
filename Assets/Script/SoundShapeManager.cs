using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundShapeManager : MonoBehaviour {

	protected Teacher data;

	protected GameObject shapes;
	public string startSoundName;
	protected Button[] shapeBtn;
	protected AudioSource audioSource;
	void Start()
	{
		TextAsset tempTxt = (TextAsset)Resources.Load ("Levels/soundShapes");
		
		//Ya eixste el archivo y solo checamos la version
		data = Teacher.LoadFromText(tempTxt.text);//Levels.Load(path);

		shapes = GameObject.Find("Shapes");
		shapeBtn = shapes.gameObject.GetComponentsInChildren<Button>();
		audioSource = GameObject.FindObjectOfType<AudioSource>();
		if(audioSource==null)
		{
			GameObject.Find("Main Camera").AddComponent<AudioSource>();
			audioSource = GameObject.FindObjectOfType<AudioSource>();
		}
		overSound(startSoundName);
	}

	void Update()
	{
		#if TEACHER_MODE
		if(Input.GetMouseButtonDown(0))
		{
			popUp.SetActive(false);
		}
		
		if(DrawTool.GetComponent<DrawingInput>().canDraw)
		{
			foreach(Button b in shapeBtn)
			{
				b.transition = Selectable.Transition.None;
			}
		}
		else
		{
			foreach(Button b in shapeBtn)
			{
				b.transition = Selectable.Transition.ColorTint;
			}
		}
		#else

		#endif
	}

	public void overSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay);
		
		if(!audioSource.isPlaying)
		{
			audioSource.clip = aC;
			if(!GameObject.FindObjectOfType<DrawingInput>().canDraw)
				audioSource.Play();
		}
	}

}
