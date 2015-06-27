using UnityEngine;
using System.Collections;
using System;

public class TangramInput : MonoBehaviour {

	public string selectedLayerName;
	public string normalLayerName;
	public bool allowRotation = true;

	[HideInInspector]
	public BaseShape selected;
	[HideInInspector]
	public bool ignoreNextRotation = false;

	protected Vector3 pos;
	protected float cu;
	protected int _sort;
	protected bool rotating = false;
	protected Vector3 initVector;
	protected Vector3 currentVector;
	protected Vector3 initialRotation;
	
	public delegate void DOnDragFinish();
	public delegate void DOnDrag();
	[HideInInspector]
	public DOnDragFinish onDragFinish;
	[HideInInspector]
	public DOnDrag onAnyDrag;


	void Awake()
	{
		cu = (Camera.main.orthographicSize*2)/Screen.height;
		_sort = -1;

		onDragFinish = foo;
		onAnyDrag = foo;
	}

	protected void foo(){}

	public int nextSort
	{
		get
		{
			if(_sort == int.MaxValue)
			{
				_sort = -1;
			}
			
			return ++_sort;
		}
	}

	void OnDrag(DragGesture gesture) 
	{
		onAnyDrag();

		switch(gesture.Phase)
		{
		case ContinuousGesturePhase.Started:
			if(gesture.Raycast.Hits2D != null && gesture.Raycast.Hits2D.Length > 0)
			{
				Array.Sort(gesture.Raycast.Hits2D,delegate(RaycastHit2D hit1, 
				                                           RaycastHit2D hit2) {
					if(hit1.collider)
					{
						if(hit2.collider)
						{
							BaseShape obj1;
							BaseShape obj2;

							if(hit1.collider.gameObject.name.Equals("move"))
							{
								obj1 = hit1.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
							}
							else
							{
								obj1 = hit1.collider.gameObject.GetComponent<BaseShape>();
							}

							if(hit2.collider.gameObject.name.Equals("move"))
							{
								obj2 = hit2.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
							}
							else
							{
								obj2 = hit2.collider.gameObject.GetComponent<BaseShape>();
							}
							
							return -obj1.spriteRenderer.sortingOrder.CompareTo(obj2.spriteRenderer.sortingOrder);
						}
						else
						{
							return -1;
						}
					}
					
					return 1;
					
				});
				
				BaseShape first = null;
				rotating = true;
				
				foreach(RaycastHit2D hit in gesture.Raycast.Hits2D)
				{
					if(hit.collider)
					{
						if(hit.collider.gameObject.name.Equals("move"))
						{
							selected = hit.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
							selected.sortingLayer = selectedLayerName;
							rotating = false;
							break;
						}
						else if(first == null && allowRotation)
						{
							first = hit.collider.gameObject.GetComponent<BaseShape>();
						}
					}
				}
				
				if(rotating)
				{
					if(first != null)
					{
						selected = first;
						selected.sortingLayer = selectedLayerName;
						if(!ignoreNextRotation)
						{
							initVector = Camera.main.ScreenToWorldPoint(new Vector3(gesture.StartPosition.x,gesture.StartPosition.y))
								- selected.transform.position;
							initVector.z = 0;
							initialRotation = selected.transform.eulerAngles;
						}
						else
						{
							rotating = false;
						}
					}
					else
					{
						rotating = false;
					}
				}
			}
			break;
			
		case ContinuousGesturePhase.Updated:
			if(selected != null && gesture.DeltaMove != Vector2.zero)
			{
				if(rotating)
				{
					currentVector = Camera.main.ScreenToWorldPoint(new Vector3(gesture.Position.x,gesture.Position.y))
						- selected.transform.position;
					currentVector.z = initVector.z;
					float angle = Vector3.Angle(initVector,currentVector)*(Vector3.Cross(initVector,currentVector).z > 0 ? 1:-1);
					
					selected.transform.eulerAngles = new Vector3(0,0
					                                             ,initialRotation.z + angle);
				}
				else
				{
					pos = new Vector3(selected.transform.position.x + gesture.DeltaMove.x*cu
					                  ,selected.transform.position.y + gesture.DeltaMove.y*cu
					                  ,selected.transform.position.z);
					selected.transform.position = pos;
				}
			}
			break;
		case ContinuousGesturePhase.Ended:
			if(selected != null)
			{
				selected.sortingLayer = normalLayerName;
				selected.sortingOrder = nextSort;
			}
			onDragFinish();
			ignoreNextRotation = false;
			selected = null;
			break;
			
		}
	}

	void OnTap(TapGesture gesture) 
	{
		if(gesture.Raycast.Hits2D != null && gesture.Raycast.Hits2D.Length > 0)
		{
			Array.Sort(gesture.Raycast.Hits2D,delegate(RaycastHit2D hit1, 
			                                           RaycastHit2D hit2) {
				if(hit1.collider)
				{
					if(hit2.collider)
					{
						BaseShape obj1;
						BaseShape obj2;
						
						if(hit1.collider.gameObject.name.Equals("move"))
						{
							obj1 = hit1.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
						}
						else
						{
							obj1 = hit1.collider.gameObject.GetComponent<BaseShape>();
						}
						
						if(hit2.collider.gameObject.name.Equals("move"))
						{
							obj2 = hit2.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
						}
						else
						{
							obj2 = hit2.collider.gameObject.GetComponent<BaseShape>();
						}
						
						return -obj1.spriteRenderer.sortingOrder.CompareTo(obj2.spriteRenderer.sortingOrder);
					}
					else
					{
						return -1;
					}
				}
				
				return 1;
				
			});
			
			BaseShape first = null;
			rotating = true;
			
			foreach(RaycastHit2D hit in gesture.Raycast.Hits2D)
			{
				if(hit.collider)
				{
					if(hit.collider.gameObject.name.Equals("move"))
					{
						hit.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>().sortingOrder = nextSort;
						break;
					}
				}
			}
		}
	}
}
