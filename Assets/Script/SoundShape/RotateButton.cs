using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class RotateButton : MonoBehaviour {

	protected float random;
	protected int randvalue;
	protected bool rotate = true;
	protected Pencil Pencil;

	protected bool dontStopRotation;
	// Use this for initialization
	void Start () {
		Pencil = FindObjectOfType<Pencil>();
		random = Random.Range(0.1f,0.5f);
		randvalue = Random.Range(0,2);
		if(randvalue==0)
		{
			random*=-1;
		}

		random*=100;
		randvalue = Random.Range(0,360);
		this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x,this.transform.eulerAngles.y,randvalue);
	}



	void Update()
	{
		if(rotate)
		{
			this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x,this.transform.eulerAngles.y,this.transform.eulerAngles.z+random*Time.smoothDeltaTime);
		}
	}

	public void stopRotate()
	{
		if(!Pencil.showing)
		{
			rotate = rotate == true ? false: true;
		}
	}
}
