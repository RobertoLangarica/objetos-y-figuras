using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundShapeManager : MonoBehaviour {

	protected Teacher data;

	protected GameObject shapes;
	public string startSoundName;
	protected Button[] shapeBtn;
	protected AudioSource audioSource;
	protected Question question;

	void Awake()
	{
		if(!GameObject.Find("Main Camera").GetComponent<AudioSource>())
		{
			audioSource = GameObject.Find("Main Camera").AddComponent<AudioSource>();
			audioSource.volume = 1;//0.5f;
		}
		else
		{
			audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
			audioSource.volume = 1;//0.5f;
		}
		question = GameObject.FindObjectOfType<Question>();
		//Se llama starGame en el analytic para setear el tiempo = 0
		AnalyticManager.instance.startGame();
	}

	void Start()
	{
		TextAsset tempTxt = (TextAsset)Resources.Load ("Texts/toastTexts");
		
		//Ya eixste el archivo y solo checamos la version
		data = Teacher.LoadFromText(tempTxt.text);//Levels.Load(path);

		shapes = GameObject.Find("Shapes");
		shapeBtn = shapes.gameObject.GetComponentsInChildren<Button>();


		overSound(startSoundName);
	}

	public void overSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay);

		if(!audioSource.isPlaying)
		{
			audioSource.clip = aC;
			if(!GameObject.FindObjectOfType<DrawingInput>().canDraw)
			{
				audioSource.Play();
			}
		}
	}

	public void overSoundNotStoping(string soundToPlay)
	{
		bool sameName = false;
		if(audio.clip)
		{
			if(audioSource.isPlaying&&soundToPlay == audioSource.clip.name)
			{
				sameName = true;
			}
		}
		//Debug.Log(soundToPlay);
		//Debug.Log(audioSource.clip.name);
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay);


		if(!GameObject.FindObjectOfType<DrawingInput>().canDraw&&!sameName&&!question.soundIsPlaying)
		{
			audioSource.clip = aC;
			audioSource.Play();
		}

	}
	void OnDisable() {
		AnalyticManager.instance.finsh("Conoce", startSoundName,startSoundName);
	}

}
