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
	
	public float rotateAmount = 15;

	[HideInInspector]
	public float currentRotation;
	protected Vector3 positionDifference;//diferencia de posicon con el touch
	[HideInInspector]
	public SpriteRenderer sprite;
	[HideInInspector]
	public bool isPositionated = false;
	protected float rot;
	protected float mod;
	protected float percent;
	
	//Para la destruccion
	protected bool destroying = false;
	protected Vector3 currentScale;
	protected float inverseDestroyTime;
	protected float destroyElapsed;
	
	
	//Para el start
	protected Vector3 initialScale;
	protected bool starting;
	protected float inverseStartTime;
	protected float startElapsedTime;

	// Use this for initialization
	void Start () {
		boundaryTop = Camera.main.orthographicSize;
		boundaryBottom = -Camera.main.orthographicSize;
		boundaryLeft = -Camera.main.aspect * Camera.main.orthographicSize;
		boundaryRight = Camera.main.aspect * Camera.main.orthographicSize;

		sprite = GetComponent<SpriteRenderer>();
		
		inverseStartTime = 1.0f/0.5f;
		startElapsedTime = 0;
		initialScale = transform.localScale;
		transform.localScale = Vector3.zero;
		currentScale = Vector3.zero;
		currentScale.z = initialScale.z;
		starting = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(starting)
		{
			percent = startElapsedTime*inverseStartTime;
			currentScale.x = Mathf.SmoothStep(currentScale.x,initialScale.x,percent);
			currentScale.y = Mathf.SmoothStep(currentScale.y,initialScale.y,percent);
			
			if(currentScale.x == initialScale.x && currentScale.y == initialScale.y)
			{
				starting = false;
			}
			
			transform.localScale = currentScale;
			startElapsedTime += Time.deltaTime;
		}
		else if(destroying)
		{
			percent = destroyElapsed*inverseDestroyTime;
			currentScale.x = Mathf.SmoothStep(currentScale.x,0,percent);
			currentScale.y = Mathf.SmoothStep(currentScale.y,0,percent);
			
			transform.localScale = currentScale;
			
			if(currentScale.x == 0 && currentScale.y == 0)
			{
				GameObject.DestroyImmediate(this.gameObject);
			}
			
			destroyElapsed += Time.deltaTime;
		}
		
		if(!destroying)
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
	
	public void destroy(float delay)
	{
		currentScale = transform.localScale;
		inverseDestroyTime = 1.0f/delay;
		destroyElapsed = 0;
		destroying = true;
	}
}
