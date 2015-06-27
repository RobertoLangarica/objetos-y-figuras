using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening;

public class Shape : BaseShape {
	
	public float boundaryTop;
	public float boundaryBottom;
	public float boundaryLeft;
	public float boundaryRight;

	public static int sort; 
	public float rotateAmount = 15;

	[HideInInspector]
	public float currentRotation;
	protected Vector3 positionDifference;//diferencia de posicon con el touch
	[HideInInspector]
	public SpriteRenderer sprite;
	protected float rot;
	protected float mod;

	// Use this for initialization
	void Start () {
		boundaryTop = Camera.main.orthographicSize;
		boundaryBottom = -Camera.main.orthographicSize;
		boundaryLeft = -Camera.main.aspect * Camera.main.orthographicSize;
		boundaryRight = Camera.main.aspect * Camera.main.orthographicSize;

		sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 centerDif = transform.position - transform.collider2D.bounds.center;
		Vector3 nVec3 = transform.position;

		if((transform.collider2D.bounds.center).x - (transform.collider2D.bounds.size.x * 0.5f) < boundaryLeft) 
		{
			nVec3.x = boundaryLeft + (transform.collider2D.bounds.size.x * 0.5f);
			nVec3.x += centerDif.x;
		}
		if((transform.collider2D.bounds.center).x + (transform.collider2D.bounds.size.x * 0.5f) > boundaryRight) 
		{
			nVec3.x = boundaryRight - (transform.collider2D.bounds.size.x * 0.5f);
			nVec3.x += centerDif.x;
		}
		if((transform.collider2D.bounds.center).y - (transform.collider2D.bounds.size.y * 0.5f) < boundaryBottom) 
		{
			nVec3.y = boundaryBottom + (transform.collider2D.bounds.size.y * 0.5f);
			nVec3.y += centerDif.y;
		}
		if((transform.collider2D.bounds.center).y + (transform.collider2D.bounds.size.y * 0.5f) > boundaryTop) 
		{
			nVec3.y = boundaryTop - (transform.collider2D.bounds.size.y * 0.5f);
			nVec3.y += centerDif.y;
		}
		transform.position = nVec3;
	}

	public void onRotationComplete()
	{
		rot = transform.rotation.eulerAngles.z;

		mod = rot%rotateAmount;
		//solo rotaciones multiplo permitidas
		if(mod != 0)
		{
			rot -= mod;
			if(mod > rotateAmount*0.5f)
			{
				//Hacia arriba
				rot += rotateAmount;
			}

			transform.DORotate(new Vector3(0,0,rot),0.1f);
		}

		currentRotation = rot;
	}	
}
