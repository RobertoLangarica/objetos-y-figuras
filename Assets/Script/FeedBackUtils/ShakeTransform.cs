using UnityEngine;
using System.Collections;

public class ShakeTransform : MonoBehaviour 
{

	[HideInInspector]
	public delegate void OnFinish();
	[HideInInspector]
	public OnFinish onFinish;

	public float shakeAmount = 0.15f;
	protected bool started = false;
	protected Vector3 initialPos;
	protected Vector2 r;
	protected float time;
	protected float elapsed;

	void Start()
	{
		onFinish = foo;
	}

	void foo(){}


	void Update()
	{
		if(started)
		{
			elapsed += Time.deltaTime;
			r = Random.insideUnitCircle*shakeAmount;
			transform.localPosition = new Vector3(initialPos.x+r.x,initialPos.y+r.y,initialPos.z);

			if(elapsed > time)
			{
				started = false;
				elapsed = 0;
				transform.localPosition = initialPos;
				onFinish();
			}
		}
	}

	public void startAction(float duration)
	{
		started = true;
		time = duration;
		initialPos = transform.localPosition;
	}
}
