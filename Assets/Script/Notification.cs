using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Notification : MonoBehaviour {
	
	protected AudioSource audioSource;
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
		toast = GameObject.Find("Notify");
		robots = GameObject.FindGameObjectsWithTag("Robot");
		//questionText("0");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void questionSound(string soundToPlay)
	{
		AudioClip aC = (AudioClip)Resources.Load("Sounds/"+soundToPlay);
		

		audioSource.clip = aC;
		audioSource.Play();
		currentToast = soundToPlay;
		if(audioSource.clip)
		{
			StartCoroutine("hideToastWhenSoundEnd",new object[2]{audioSource.clip.length,soundToPlay});
		}
		else
			StartCoroutine("hideToastWhenSoundEnd",new object[2]{3f,soundToPlay});
		questionText(soundToPlay);

	}
	
	protected void questionText(string textToPlay)
	{
		string number = "";
		string shape = "";
		notify infTemp = new notify();

		infTemp = data.getNotifyByName(textToPlay);

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