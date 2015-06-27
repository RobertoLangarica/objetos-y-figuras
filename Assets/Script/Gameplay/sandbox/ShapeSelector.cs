﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ShapeSelector : MonoBehaviour {

	public GameObject square;
	public GameObject rectangle;
	public GameObject triangle;
	public GameObject rhomboid;
	public GameObject trapezium;

	public Image img_square;
	public Image img_rectangle;
	public Image img_triangle;
	public Image img_rhomboid;
	public Image img_trapezium; 

	public ColorSelector colorSelector;
	public TangramInput input;



	public void instantiateShape(string name)
	{
		GameObject shape = null;
		Image reference = null;

		switch(name)
		{
		case "square":
			shape = (GameObject.Instantiate(square) as GameObject);
			reference = img_square;
			break;
		case "rectangle":
			shape = (GameObject.Instantiate(rectangle) as GameObject);
			reference = img_rectangle;
			break;
		case "triangle":
			shape = (GameObject.Instantiate(triangle) as GameObject);
			reference = img_triangle;
			break;
		case "rhomboid":
			shape = (GameObject.Instantiate(rhomboid) as GameObject);
			reference = img_rhomboid;
			break;
		case "trapezium":
			shape = (GameObject.Instantiate(trapezium) as GameObject);
			reference = img_trapezium;
			break;
		}
		input.ignoreNextRotation = true;
		input.selected = shape.GetComponent<SandboxShape>();
		Sprite sprite = selected.spriteRenderer.sprite;

		selected.sortingLayer = "SelectedShape";
		selected.color = colorSelector.selectedColor;

		float u = (Camera.main.orthographicSize*2*sprite.pixelsPerUnit)/Screen.height;
		Vector3 size = sprite.bounds.size * sprite.pixelsPerUnit;
		float s = Mathf.Min((reference.rectTransform.sizeDelta.y*u)/size.y
		                    ,(reference.rectTransform.sizeDelta.x*u)/size.x);
		shape.transform.localScale = new Vector3(s,s,1);
		size = Camera.main.ScreenToWorldPoint(reference.transform.position);
		size.z = 0;
		shape.transform.position = size;
	}

	void OnDrag(DragGesture gesture) 
	{
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
							SandboxShape obj1;
							SandboxShape obj2;

							if(hit1.collider.gameObject.name.Equals("move"))
							{
								obj1 = hit1.collider.gameObject.transform.parent.gameObject.GetComponent<SandboxShape>();
							}
							else
							{
								obj1 = hit1.collider.gameObject.GetComponent<SandboxShape>();
							}

							if(hit2.collider.gameObject.name.Equals("move"))
							{
								obj2 = hit2.collider.gameObject.transform.parent.gameObject.GetComponent<SandboxShape>();
							}
							else
							{
								obj2 = hit2.collider.gameObject.GetComponent<SandboxShape>();
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

				SandboxShape first = null;
				rotating = true;

				foreach(RaycastHit2D hit in gesture.Raycast.Hits2D)
				{
					if(hit.collider)
					{
						if(hit.collider.gameObject.name.Equals("move"))
						{
							selected = hit.collider.gameObject.transform.parent.gameObject.GetComponent<SandboxShape>();
							selected.sortingLayer = "SelectedShape";
							rotating = false;
							break;
						}
						else if(first == null)
						{
							first = hit.collider.gameObject.GetComponent<SandboxShape>();
						}
					}
				}

				if(rotating)
				{
					if(first != null)
					{
						selected = first;
						selected.sortingLayer = "SelectedShape";
						if(!creating)
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
				pos = Camera.main.WorldToScreenPoint(selected.transform.position);

				if(pos.x < Screen.width*.2f)
				{
					GameObject.Destroy(selected.gameObject);
				}
				else
				{
					pos.x = Mathf.Round(pos.x);
					pos.y = Mathf.Round(pos.y);
					pos = Camera.main.ScreenToWorldPoint(pos);
					pos.z = selected.transform.position.z;
					selected.transform.position = pos;
					selected.sortingLayer = "Shapes";
					selected.sortingOrder = nextSort;
				}
			}
			creating = false;
			selected = null;
			break;

		}
	}

	void OnTap(TapGesture gesture) 
	{
		Debug.Log(int.MaxValue);
		if(gesture.Raycast.Hits2D != null && gesture.Raycast.Hits2D.Length > 0)
		{
			Array.Sort(gesture.Raycast.Hits2D,delegate(RaycastHit2D hit1, 
			                                           RaycastHit2D hit2) {
				if(hit1.collider)
				{
					if(hit2.collider)
					{
						SandboxShape obj1;
						SandboxShape obj2;
						
						if(hit1.collider.gameObject.name.Equals("move"))
						{
							obj1 = hit1.collider.gameObject.transform.parent.gameObject.GetComponent<SandboxShape>();
						}
						else
						{
							obj1 = hit1.collider.gameObject.GetComponent<SandboxShape>();
						}
						
						if(hit2.collider.gameObject.name.Equals("move"))
						{
							obj2 = hit2.collider.gameObject.transform.parent.gameObject.GetComponent<SandboxShape>();
						}
						else
						{
							obj2 = hit2.collider.gameObject.GetComponent<SandboxShape>();
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
			
			SandboxShape first = null;
			rotating = true;
			
			foreach(RaycastHit2D hit in gesture.Raycast.Hits2D)
			{
				if(hit.collider)
				{
					if(hit.collider.gameObject.name.Equals("move"))
					{
						hit.collider.gameObject.transform.parent.gameObject.GetComponent<SandboxShape>().sortingOrder = nextSort;
						break;
					}
				}
			}
		}
	}	
}