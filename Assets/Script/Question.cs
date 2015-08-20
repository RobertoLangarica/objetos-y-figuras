using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Question : MonoBehaviour {

	protected AudioSource audioSource;
	protected int soundtoGo =0;
	protected Teacher data;
	protected GameObject toast;
	protected string currentToast;
	protected GameObject[] robots;
	protected Vector2 initialAnchoredPos;

	public bool firstTime = false;
	public string firstTimeText;
	// Use this for initialization
	void Start () {
		audioSource = GameObject.FindObjectOfType<AudioSource>();
		if(audioSource==null)
		{
			GameObject.Find("Main Camera").AddComponent<AudioSource>();
			audioSource = GameObject.FindObjectOfType<AudioSource>();
		}

		TextAsset tempTxt = (TextAsset)Resources.Load ("Texts/toastTexts");
		data = Teacher.LoadFromText(tempTxt.text);
		toast = GameObject.Find("Question");
		robots = GameObject.FindGameObjectsWithTag("RobotQuestion");

		initialAnchoredPos = toast.GetComponent<RectTransform>().anchoredPosition;
		showToast(false);

		if(firstTime)
		{
			firstQuestionSound(firstTimeText);
		}
	}

	public void questionSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay+"_"+soundtoGo);
		showToast(false,0);

		audioSource.clip = aC;

		audioSource.Play();
		currentToast = soundToPlay+"_"+soundtoGo;
		if(audioSource.clip!=null)
		{
			StopCoroutine("hideToastWhenSoundEnd");
			StartCoroutine("hideToastWhenSoundEnd",new object[2]{audioSource.clip.length,soundToPlay+"_"+soundtoGo});
		}
		else
		{
			StopCoroutine("hideToastWhenSoundEnd");
			StartCoroutine("hideToastWhenSoundEnd",new object[2]{3f,soundToPlay+"_"+soundtoGo});
		}
		questionText(soundToPlay+"_"+soundtoGo);
		soundtoGo++;

		if(soundtoGo>=data.getFigureByName(soundToPlay).infos.Length)
		{

			soundtoGo=0;
		}
	}

	public void firstQuestionSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay+"_"+soundtoGo);
		showToast(false,0);
		
		audioSource.clip = aC;
		
		audioSource.Play();
		currentToast = soundToPlay+"_"+soundtoGo;
		if(audioSource.clip!=null)
		{
			StopCoroutine("hideToastWhenSoundEnd");
			StartCoroutine("hideToastWhenSoundEnd",new object[2]{audioSource.clip.length,soundToPlay+"_"+soundtoGo});
		}
		else
		{
			StopCoroutine("hideToastWhenSoundEnd");
			StartCoroutine("hideToastWhenSoundEnd",new object[2]{3f,soundToPlay+"_"+soundtoGo});
		}
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

	protected void showToast(bool show,float delay = .5f)
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
		yield return new WaitForSeconds((float)parms[0]+3f);
		if(string.Compare(currentToast,(string)parms[1])==0)
		{
			showToast(false);
		}
	}

}
