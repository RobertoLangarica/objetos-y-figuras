using UnityEngine;
using System.Collections;

public class Pencil : MonoBehaviour {

	public GameObject Switch2EraseBtn;
	public GameObject EreaseAllBtn;
	// Use this for initialization
	void Start () {
		Switch2EraseBtn.SetActive(false);
		EreaseAllBtn.SetActive(false);
	}
	
	public void activateDrawing()
	{
		if(Switch2EraseBtn.activeSelf)
		{
			Switch2EraseBtn.SetActive(false);
			EreaseAllBtn.SetActive(false);
		}
		else
		{
			Switch2EraseBtn.SetActive(true);
			EreaseAllBtn.SetActive(true);
		}
	}
}
