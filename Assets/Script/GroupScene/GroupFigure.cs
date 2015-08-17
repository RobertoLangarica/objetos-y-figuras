using UnityEngine;
using System.Collections;

public class GroupFigure : BaseShape 
{
	public delegate void onMovementFinish(GameObject go);

	protected int sort; 
	
	[HideInInspector]
	public int group;
	[HideInInspector]
	public SpriteRenderer sprite;
	[HideInInspector]
	public onMovementFinish finishAction;

	protected int gSort; 
	protected float boundaryTop;
	protected float boundaryBottom;
	protected float boundaryLeft;
	protected float boundaryRight;
	protected Vector3 positionDifference;//diferencia de posicon con el touch
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

	void Start()
	{
		boundaryTop = Camera.main.orthographicSize;
		boundaryBottom = -Camera.main.orthographicSize;
		boundaryLeft = -Camera.main.aspect * Camera.main.orthographicSize;
		boundaryRight = Camera.main.aspect * Camera.main.orthographicSize;

		sprite = GetComponent<SpriteRenderer>();

		baseStart();
		
		inverseStartTime = 1.0f/0.5f;
		startElapsedTime = 0;
		initialScale = transform.localScale;
		transform.localScale = Vector3.zero;
		currentScale = Vector3.zero;
		currentScale.z = initialScale.z;
		starting = true;
	}

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
	
	public void onTouchBegan(Vector3 position)
	{
		sprite.sortingOrder = ++gSort;
		if(gSort == 32767)
		{
			GroupFigure[] shapes = GameObject.FindObjectsOfType<GroupFigure>();
			foreach(GroupFigure s in shapes)
			{
				s.sprite.sortingOrder = -32767;
			}
			
			gSort = -32767;
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
		transform.position = (position-positionDifference);
	}

	public void opnTouchEnded()
	{
		if(finishAction != null)
		{
			finishAction(gameObject);
		}
	}
	
	public void destroy(float delay)
	{
		currentScale = transform.localScale;
		inverseDestroyTime = 1.0f/delay;
		destroyElapsed = 0;
		destroying = true;
	}
}
