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
	// Use this for initialization
	void Start () {
		audioSource = GameObject.FindObjectOfType<AudioSource>();
		if(audioSource==null)
		{
			GameObject.Find("Main Camera").AddComponent<AudioSource>();
			audioSource = GameObject.FindObjectOfType<AudioSource>();
		}

		TextAsset tempTxt = (TextAsset)Resources.Load ("Levels/soundShapes");
		data = Teacher.LoadFromText(tempTxt.text);
		toast = GameObject.Find("Toast");
		robots = GameObject.FindGameObjectsWithTag("Robot");
		showToast(true);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void questionSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay+"_"+soundtoGo);
		showToast(true,0);

		audioSource.clip = aC;

		audioSource.Play();
		currentToast = soundToPlay+"_"+soundtoGo;
		if(audioSource.clip!=null)
		{
			StartCoroutine("hideToastWhenSoundEnd",new object[2]{audioSource.clip.length,soundToPlay+"_"+soundtoGo});
		}
		else
		{
			StartCoroutine("hideToastWhenSoundEnd",new object[2]{3f,soundToPlay+"_"+soundtoGo});
		}
		questionText(soundToPlay+"_"+soundtoGo);
		soundtoGo++;

		if(soundtoGo>=data.getFigureByName(soundToPlay).infos.Length)
		{

			soundtoGo=0;
		}
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

		showToast(false);
		toast.GetComponentInChildren<Text>().text = infTemp.text;
	}

	protected void showToast(bool hide,float delay = .5f)
	{
		float val = Screen.height*0.4f;

		if(hide)
		{
			toast.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-val ),delay);
		}
		else
		{
			choseRobot();
			toast.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay);
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
			showToast(true);
	}

}
