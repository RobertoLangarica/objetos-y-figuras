using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening;

public class Shape : MonoBehaviour {
 
	protected static short sort; 
	public float rotateAmount = 15;
	public float maxRotationAllowed = -1;

	[HideInInspector]
	public float currentRotation;
	protected Vector3 positionDifference;//diferencia de posicon con el touch
	[HideInInspector]
	public SpriteRenderer sprite;
	protected float rot;
	protected float mod;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();
		sort = -32767;
		sprite.sortingOrder = sort;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 centerDif = transform.position - transform.collider2D.bounds.center;
		Vector3 nVec3 = transform.position;
		bool modified = false;

		if((transform.collider2D.bounds.center).x - (transform.collider2D.bounds.size.x * 0.5f) < -(Screen.width * 0.009f)) 
		{
			nVec3.x = -(Screen.width * 0.01f) + ((transform.collider2D.bounds.center).x - (transform.collider2D.bounds.size.x * 0.5f));
			modified = true;
		}
		if((transform.collider2D.bounds.center).x + (transform.collider2D.bounds.size.x * 0.5f) > Screen.width * 0.009f)
		{
			nVec3.x = (Screen.width * 0.01f) - ((transform.collider2D.bounds.center).x - (transform.collider2D.bounds.size.x * 0.5f));
			modified = true;
		}
		if((transform.collider2D.bounds.center).y - (transform.collider2D.bounds.size.y * 0.5f) < -(Screen.height * 0.009f))
		{
			nVec3.y = -(Screen.height * 0.01f) + ((transform.collider2D.bounds.center).x - (transform.collider2D.bounds.size.x * 0.5f));
			modified = true;
		}
		if((transform.collider2D.bounds.center).y + (transform.collider2D.bounds.size.y * 0.5f) > Screen.height * 0.009f) 
		{
			nVec3.y = (Screen.height * 0.01f) - ((transform.collider2D.bounds.center).x - (transform.collider2D.bounds.size.x * 0.5f));
			modified = true;
		}

		if (modified) 
		{
			transform.position = nVec3 + centerDif;
		}
	}

	public void onTouchBegan(Vector3 position)
	{
		sprite.sortingOrder = ++sort;

		if(sort == 32767)
		{
			Shape[] shapes = GameObject.FindObjectsOfType<Shape>();
			foreach(Shape s in shapes)
			{
				s.sprite.sortingOrder = -32767;
			}

			sort = -32767;
			sprite.sortingOrder = ++sort;
		}

		//Ignoramos z
		positionDifference = position - transform.position;
		positionDifference.z = 0;
	}


	public void onTouchMove(Vector3 position)
	{
		//z no varia
		position.z = transform.position.z;
		/*if(((transform.collider2D.bounds.center).x - (transform.collider2D.bounds.size.x * 0.5f) < -(Screen.width * 0.009f)) && (position-positionDifference).x < transform.position.x) 
		{
			position.x = (transform.position+positionDifference).x;
		}
		if(((transform.collider2D.bounds.center).x + (transform.collider2D.bounds.size.x * 0.5f) > Screen.width * 0.009f) && (position-positionDifference).x > transform.position.x) 
		{
			position.x = (transform.position+positionDifference).x;
		}
		if(((transform.collider2D.bounds.center).y - (transform.collider2D.bounds.size.y * 0.5f) < -(Screen.height * 0.009f)) && (position-positionDifference).y < transform.position.y) 
		{
			position.y = (transform.position+positionDifference).y;
		}
		if(((transform.collider2D.bounds.center).y + (transform.collider2D.bounds.size.y * 0.5f) > Screen.height * 0.009f) && (position-positionDifference).y > transform.position.y) 
		{
			position.y = (transform.position+positionDifference).y;
		}*/
		transform.position = (position-positionDifference);
	}

	public void onTouchStop()
	{

	}

	public void onRotationComplete(float delay = .2f)
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

			transform.DORotate(new Vector3(0,0,rot),delay);
		}

		//Figuras que de manera logica solo admiten una rotacion
		if(maxRotationAllowed != -1 && rot>maxRotationAllowed)
		{

			currentRotation = getAllowedRotation(rot);

			if(rot > 180)
			{
				currentRotation = -maxRotationAllowed+currentRotation;
			}
		}
		else
		{
			currentRotation = rot;
		}
	}

	float getAllowedRotation(float current)
	{
		while(current > maxRotationAllowed)
		{
			current-=maxRotationAllowed;
		}

		return current;
	}
}
