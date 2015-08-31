using UnityEngine;
using System.Collections;

public class BtnTutor : MonoBehaviour {

	protected PopupBeforeTutor popup;

	void Start()
	{
		popup = FindObjectOfType<PopupBeforeTutor>();
		popup.gameObject.SetActive(false);
	}

	public void OnOpenTutor()
	{
		popup.gameObject.SetActive(true);
	}
}
