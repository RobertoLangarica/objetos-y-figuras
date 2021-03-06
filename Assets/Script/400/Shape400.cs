﻿using UnityEngine;
using System.Collections;

public class Shape400 : BaseShape {

	public enum EShapeAlign
	{
		LEFT,RIGHT,TOP,BOTTOM,NONE
	};

	public float snapDelay = 0.1f;

	[HideInInspector]
	public Container400 container;
	[HideInInspector]
	public int value;
	[HideInInspector]
	public int secondValue;

	protected float velX;
	protected float velY;
	protected Vector3 pos;

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


	//Para moverla
	protected Vector2 movingTo;
	protected float inverseMovingTime;
	protected Vector3 actualPos;
	protected float movingElapsedTime;
	protected bool moving = false;
	protected float percent;
	[HideInInspector]
	public EShapeAlign align = EShapeAlign.NONE;
	protected Vector2 spriteSize = Vector2.zero;
	protected Vector2 finalPos = Vector2.zero;

	// Use this for initialization
	void Start () {
		container = null;
		baseStart();


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
		else if(moving)
		{
			percent = movingElapsedTime*inverseMovingTime;
			actualPos.x = Mathf.SmoothStep(actualPos.x,movingTo.x,percent);
			actualPos.y = Mathf.SmoothStep(actualPos.y,movingTo.y,percent);

			if(actualPos.x == movingTo.x && actualPos.y == movingTo.y)
			{
				moving = false;
			}

			transform.position = actualPos;
			movingElapsedTime += Time.deltaTime;
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
		else if(container)
		{
			pos = transform.position;

			if(align != EShapeAlign.NONE)
			{
				spriteSize.x = spriteRenderer.sprite.rect.width*transform.localScale.x;
				spriteSize.y = spriteRenderer.sprite.rect.height*transform.localScale.y;
				spriteSize 	/= spriteRenderer.sprite.pixelsPerUnit;

				finalPos = container.getCenter();

				switch(align)
				{
				case EShapeAlign.BOTTOM:
					finalPos.y = container.min.y + spriteSize.y*0.5f + container.area.height*0.1f;
					break;
				case EShapeAlign.TOP:
					finalPos.y = container.max.y - spriteSize.y*0.5f - container.area.height*0.1f;
					break;
				case EShapeAlign.LEFT:
					finalPos.x = container.min.x + spriteSize.x*0.5f + container.area.width*0.05f;
					break;
				case EShapeAlign.RIGHT:
					finalPos.x = container.max.x - spriteSize.x*0.5f - container.area.width*0.05f;
					break;
				}

				pos.x = Mathf.SmoothDamp(pos.x,finalPos.x,ref velX,snapDelay);
				pos.y = Mathf.SmoothDamp(pos.y,finalPos.y,ref velY,snapDelay);
			}
			else
			{
				pos.x = Mathf.SmoothDamp(pos.x,container.getCenter().x,ref velX,snapDelay);
				pos.y = Mathf.SmoothDamp(pos.y,container.getCenter().y,ref velY,snapDelay);
			}

			transform.position = pos;
		}
		else
		{
			velX = velY = 0;
		}
	}

	public new void enabled(bool value)
	{
		transform.GetChild(0).gameObject.name = value ? "move":"test";
	}

	public void destroy(float delay)
	{
		currentScale = transform.localScale;
		inverseDestroyTime = 1.0f/delay;
		destroyElapsed = 0;
		destroying = true;
	}

	public void moveTo(Vector2 pos,float delay = 0.2f)
	{
		inverseMovingTime = 1.0f/delay;
		movingTo = pos;
		actualPos = transform.position;
		movingElapsedTime = 0;
		moving = true;
	}
}
