using UnityEngine;
using System.Collections;

public class Shape400 : BaseShape {

	public float snapDelay = 0.1f;

	[HideInInspector]
	public Container400 container;

	protected float velX;
	protected float velY;
	protected Vector3 pos;

	// Use this for initialization
	void Start () {
		container = null;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(container)
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
}
