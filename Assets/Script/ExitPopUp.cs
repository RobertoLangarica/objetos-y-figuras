using UnityEngine;
using System.Collections;

public class ExitPopUp : MonoBehaviour {

	public static ExitPopUp instance;
	public bool popUp;

	// Use this for initialization
	void Start () {
		instance = this;
		gameObject.SetActive(false);
	}

}
