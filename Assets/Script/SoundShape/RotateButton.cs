using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RotateButton : MonoBehaviour {

	protected float random;
	protected int randvalue;

	// Use this for initialization
	void Start () {
		random = Random.Range(0.1f,0.5f);
		randvalue = Random.Range(0,2);
		if(randvalue==0)
		{
			random*=-1;
		}
		randvalue = Random.Range(0,360);
		this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x,this.transform.eulerAngles.y,randvalue);
	}

	void Update()
	{
		this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x,this.transform.eulerAngles.y,this.transform.eulerAngles.z+random);
	}

}
