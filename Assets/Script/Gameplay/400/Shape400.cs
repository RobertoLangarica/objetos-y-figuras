using UnityEngine;
using System.Collections;

public class Shape400 : BaseShape {

	public float snapDelay = 0.1f;

	[HideInInspector]
	public Container400 container;
	[HideInInspector]
	public int value;

	protected float velX;
	protected float velY;
	protected Vector3 pos;

	//Para la destruccion
	protected bool destroying = false;
	protected float currentScale = -1;
	protected float destroyTime;
	protected float destroyElapsed = 0;

	// Use this for initialization
	void Start () {
		container = null;
		baseStart();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(destroying)
		{
			if(currentScale < 0)
			{
				currentScale = transform.localScale.x;
			}
			else
			{
				destroyElapsed += Time.deltaTime;

				currentScale = Mathf.SmoothStep(currentScale,0,destroyTime);

				transform.localScale = new Vector3(currentScale,currentScale,1);
				if(destroyElapsed > destroyTime)
				{
					GameObject.DestroyImmediate(this.gameObject);
				}
			}
		}
		else if(container)
		{
			pos = transform.position;

			pos.x = Mathf.SmoothDamp(pos.x,container.getCenter().x,ref velX,snapDelay);
			pos.y = Mathf.SmoothDamp(pos.y,container.getCenter().y,ref velY,snapDelay);

			transform.position = pos;
		}
		else
		{
			velX = velY = 0;
		}
	}

	public void enabled(bool value)
	{
		transform.GetChild(0).gameObject.name = value ? "move":"test";
	}

	public void destroy(float delay)
	{
		destroyTime = delay;
		destroying = true;
	}
}
