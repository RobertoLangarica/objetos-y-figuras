using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Question : MonoBehaviour {

	public AudioSource audioSource;
	protected int soundtoGo =0;
	protected Teacher data;
	protected GameObject toast;
	protected string currentToast;
	protected GameObject[] robots;
	protected Vector2 initialAnchoredPos;

	public bool firstTime = false;
	public string firstTimeText;
	protected bool showing = false;
	protected float waitForClick;
	protected Notification notification;

	// Use this for initialization
	void Start () {
		notification = Object.FindObjectOfType<Notification>();
		if(GameObject.Find("Main Camera").GetComponent<AudioSource>()==null)
		{
			audioSource = GameObject.Find("Main Camera").AddComponent<AudioSource>();
		}
		else
		{
			audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		}

		TextAsset tempTxt = (TextAsset)Resources.Load ("Texts/toastTexts");
		data = Teacher.LoadFromText(tempTxt.text);
		toast = GameObject.Find("Question");
		robots = GameObject.FindGameObjectsWithTag("RobotQuestion");

		toast.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-Screen.height);

		initialAnchoredPos = toast.GetComponent<RectTransform>().anchoredPosition;
		showToast(false);

		if(firstTime)
		{
			if(Application.loadedLevelName == "DrawingScene")
			{
				AudioClip aC = (AudioClip)Resources.Load("Sounds/observa");
				if(aC)
				{
					audioSource.PlayOneShot(aC);
					Invoke("soundFirstTime",aC.length);
				}
				else
				{
					firstQuestionSound(firstTimeText);
				}
			}
			else
			{
				firstQuestionSound(firstTimeText);
			}
		}
	}

	public void questionSound(string soundToPlay)
	{
		//Debug.Log(data.getFigureByName(soundToPlay).getInfoByName(soundtoGo.ToString()).idSound);
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+data.getFigureByName(soundToPlay).getInfoByName(soundtoGo.ToString()).idSound);
		showToast(false,0);
		showing = true;
		waitForClick = 0.5f;

		audioSource.clip = aC;
		if(notification)
		{
			if(notification.showing)
			{
				return;
			}
		}
		audioSource.Play();
		currentToast = soundToPlay+"_"+soundtoGo;
		float time = 3.0f;

		StopCoroutine("hideToastWhenSoundEnd");
		if(audioSource.clip!=null && audioSource.clip.length > time)
		{
			time = audioSource.clip.length;
		}

		StartCoroutine("hideToastWhenSoundEnd",new object[2]{time,soundToPlay+"_"+soundtoGo});

		questionText(soundToPlay+"_"+soundtoGo);
		soundtoGo++;

		if(soundtoGo>=data.getFigureByName(soundToPlay).infos.Length)
		{

			soundtoGo=0;
		}
	}

	protected void soundFirstTime()
	{
		firstQuestionSound(firstTimeText);
	}

	public void firstQuestionSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+data.getFigureByName(soundToPlay).getInfoByName(soundtoGo.ToString()).idSound);
		showToast(false,0);
		showing = true;
		waitForClick = 0.6f;

		audioSource.clip = aC;
		
		audioSource.Play();
		currentToast = soundToPlay+"_"+soundtoGo;
		float time = 3.0f;
		
		StopCoroutine("hideToastWhenSoundEnd");
		if(audioSource.clip!=null && audioSource.clip.length > time)
		{
			time = audioSource.clip.length;
		}
		
		StartCoroutine("hideToastWhenSoundEnd",new object[2]{time,soundToPlay+"_"+soundtoGo});
		
		questionText(soundToPlay+"_"+soundtoGo);
	}
	protected void questionText(string textToPlay)
	{
		string number = "";
		string shape = "";
		Info infTemp = new Info();

		number = textToPlay.Substring(textToPlay.IndexOf('_')+1);
		shape = textToPlay.Substring(0,textToPlay.IndexOf('_'));

		infTemp = data.getFigureByName(shape).getInfoByName(number);
		Figures[] figure = data.figure;

		showToast(true);
		toast.GetComponentInChildren<Text>().text = infTemp.text;
	}

	public void showToast(bool show,float delay = .5f)
	{
		float val = Screen.height*0.4f;

		if(!show)
		{
			toast.GetComponent<RectTransform>().DOAnchorPos(initialAnchoredPos,delay).SetEase(Ease.InBack);
		}
		else
		{
			choseRobot();
			toast.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay).SetEase(Ease.OutBack);
		}
	}

	protected void choseRobot()
	{
		int rand = Random.Range(0,3);
		for(int i =0; i<robots.Length; i++)
		{
			robots[i].SetActive(false);
			if(i==rand)
			{
				robots[i].SetActive(true);
			}
		}
	}

	IEnumerator hideToastWhenSoundEnd(object[] parms)
	{
		yield return new WaitForSeconds((float)parms[0]);
		if(string.Compare(currentToast,(string)parms[1])==0)
		{
			showing = false;
			showToast(false);
		}
	}

	void Update()
	{
		//Se cierra al click en la pantalla
		if(showing)
		{
			waitForClick -= Time.deltaTime;
			if(waitForClick <= 0 && Input.GetMouseButtonUp(0))
			{
				StopCoroutine("hideToastWhenSoundEnd");
				showing = false;
				showToast(false);
			}
		}
	}

	protected void forceClose()
	{
		showToast(false);
	}

	IEnumerator lateStart()
	{
		yield return new WaitForSeconds(.2f);

		Debug.Log(notification);
	}
}
