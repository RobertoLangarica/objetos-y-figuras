using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputField : MonoBehaviour {

	public Text myName;
	public string mn;
	public static InputField instance;
	// Use this for initialization
	void Start () {
		instance = this;
		myName = GameObject.Find("Text").GetComponent<Text>();
	}
	void Update()
	{
		myName = GameObject.Find("Text").GetComponent<Text>();
		mn = myName.ToString();
	}

	public void Name()
	{
		UserDataManager.instance.name = myName.text;
	}

}
