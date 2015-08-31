using UnityEngine;
using System.Collections;

public class BeforeTutorPopupShape : MonoBehaviour 
{
	public string displayName;

	[HideInInspector]
	public int clicksForTrigger;
	[HideInInspector]
	public delegate void OnTrigger();
	[HideInInspector]
	public OnTrigger onTrigger;

	protected int clickCount;

	protected bool showing;
	protected Vector3 currentScale;
	protected Vector3 initialScale;
	protected float inverseTime;
	protected float showElapsed;
	protected Vector3 destinationScale;
	protected float percent;

	void Start()
	{
		showing = false;
		onTrigger += foo;
	}
	
	void foo(){}

	public void onClick()
	{
		Debug.Log (clickCount+"____"+clicksForTrigger);
		if(++clickCount == clicksForTrigger)
		{
			onTrigger();
		}
	}

	void Update()
	{
		if(showing)
		{
			percent = showElapsed*inverseTime;
			currentScale.x = Mathf.SmoothStep(initialScale.x,destinationScale.x,percent);
			currentScale.y = currentScale.x;
			currentScale.z = currentScale.x;

			if(currentScale == destinationScale)
			{
				showing = false;
			}

			transform.localScale = currentScale;
			showElapsed+= Time.deltaTime;
		}
	}

	public void show()
	{
		showing = true;
		initialScale = transform.localScale;
		currentScale = initialScale;
		showElapsed = 0;
		percent = 0;
		inverseTime = 1/0.2f;
		destinationScale = Vector3.one;
	}
}
