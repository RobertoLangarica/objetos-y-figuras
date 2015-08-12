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

	void Start()
	{
		boundaryTop = Camera.main.orthographicSize;
		boundaryBottom = -Camera.main.orthographicSize;
		boundaryLeft = -Camera.main.aspect * Camera.main.orthographicSize;
		boundaryRight = Camera.main.aspect * Camera.main.orthographicSize;

		sprite = GetComponent<SpriteRenderer>();
	}

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
}
