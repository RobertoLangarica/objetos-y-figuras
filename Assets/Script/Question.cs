using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Question : MonoBehaviour {

	protected AudioSource audioSource;
	protected int soundtoGo =0;
	public int soundlist;
	protected Teacher data;
	protected GameObject toast;
	protected string currentToast;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void questionSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay+"_"+soundtoGo);
		
		if(!audioSource.isPlaying)
		{
			audioSource.clip = aC;
			audioSource.Play();
			currentToast = soundToPlay+"_"+soundtoGo;
			StartCoroutine("hideToastWhenSoundEnd",new object[2]{audioSource.clip.length,soundToPlay+"_"+soundtoGo});
			questionText(soundToPlay+"_"+soundtoGo);
			soundtoGo++;
		}
		if(soundtoGo>=soundlist)
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
		float val = Screen.height*0.2f;

		if(hide)
		{
			toast.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-val ),delay);
		}
		else
		{
			toast.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay);
		}
	}

	IEnumerator hideToastWhenSoundEnd(object[] parms)
	{
		yield return new WaitForSeconds((float)parms[0]+0.5f);
		if(string.Compare(currentToast,(string)parms[1])==0)
			showToast(true);
	}
}
