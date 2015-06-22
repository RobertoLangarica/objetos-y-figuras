using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;

[RequireComponent(typeof(DragRecognizer))]
[RequireComponent(typeof(TwistRecognizer))]
public class InputHub : MonoBehaviour
{
	public LayerMask touchMask;
	public float rotateDistance = 4.0f;

	protected Shape selected;
	protected GameManager manager;
	protected bool isRotating = false;
	protected float wheel;
	protected float lastRotation = -1;
	protected float timeToSnapRotation = 0.50f;

	private Vector3 initialP = Vector3.zero;
	private bool mouseFlag = false;

	void Start ()
	{
		selected = null;
		manager = FindObjectOfType<GameManager>();

		GetComponent<DragRecognizer>().OnGesture += OnDrag;
		GetComponent<TwistRecognizer>().OnGesture += OnTwist;
	}
	
	void Update()
	{

		if(selected != null)
		{
			wheel = Input.GetAxis("Mouse ScrollWheel");
			if(!GameManager.isEasy)
			{
				if(wheel != 0)
				{
					lastRotation = Time.time;
					isRotating = true;
					selected.transform.Rotate(Vector3.back,wheel*15);
				}
				else if(lastRotation != -1 && Time.time-lastRotation >= timeToSnapRotation)
				{
					stopRotation();
					manager.checkForLevelComplete();
				}
			}
		}
		if (Input.GetMouseButtonDown (0)) 
		{
			mouseFlag = true;
		} 
	}

	void OnDrag(DragGesture gesture)
	{
		if (!mouseFlag)return;

		DOTween.Kill("SnapMove");
		switch(gesture.Phase)
		{
			case ContinuousGesturePhase.Started:

			if(selected != null  && gesture.StartSelection)
			{
				if(!gesture.StartSelection.Equals(selected))
				{
					stopSelected();
				}
			}
			if(gesture.StartSelection)
			{
				selected = gesture.StartSelection.GetComponent<Shape>();
				selected.onTouchBegan(Camera.main.ScreenToWorldPoint(gesture.StartPosition));
				initialP = Input.mousePosition;
			}
			if(selected != null && gesture.StartSelection == null)
			{
				selected.onTouchBegan(Camera.main.ScreenToWorldPoint(gesture.StartPosition));
			}
			break;

			case ContinuousGesturePhase.Updated:
			if(selected && !isRotating && Input.mousePosition != initialP)
			{
				selected.onTouchMove(Camera.main.ScreenToWorldPoint(gesture.Position));
				selected.turnRotationSpriter(false);
				initialP = Input.mousePosition;
			}
			else if(isRotating)
			{
				selected.onTouchBegan(Camera.main.ScreenToWorldPoint(gesture.Position));
			}
			break;

			case ContinuousGesturePhase.Ended:
			sortShapes();
			//selected.transform.localPosition=new Vector3(selected.transform.localPosition.x,selected.transform.localPosition.y,-0.1f);
			
			if(selected)
			{
				mouseFlag = false;
				selected.turnRotationSpriter(true);
				manager.checkForLevelComplete();
			}
			break;
		}
	}

	void OnTwist(TwistGesture gesture)
	{
		if(selected == null)return;
		if (GameManager.isEasy)return;

		switch(gesture.Phase)
		{
			case ContinuousGesturePhase.Started:
				isRotating = true;
				selected.transform.Rotate(Vector3.back,-gesture.DeltaRotation);
				break;

			case ContinuousGesturePhase.Updated:
				selected.transform.Rotate(Vector3.back,-gesture.DeltaRotation);
				break;

			case ContinuousGesturePhase.Ended:
			if(Input.touchCount == 2 && selected)
			{
				stopRotation();
				manager.checkForLevelComplete();
			}
			break;
		}
	}

	void stopRotation()
	{
		lastRotation = -1;
		isRotating = false;
		selected.onRotationComplete();
	}

	void stopSelected()
	{
		Debug.Log ("Se va a deseleccionar");
		if(isRotating)
		{
			//stopRotation();
		}
		selected.onTouchStop();
		selected.turnRotationSpriter(false);
		selected = null;
		manager.checkForLevelComplete();
		DOTween.Play("SnapMove");
	}

	void sortShapes()
	{
		GameObject temp;
		float posTemp =-.1f;
		for (int write = 0; write < manager.shapes.Length; write++) 
		{
			for (int sort = 0; sort < manager.shapes.Length - 1; sort++) 
			{
				if (manager.shapes[sort].transform.renderer.sortingOrder > manager.shapes[sort + 1].transform.renderer.sortingOrder) 
				{
					temp = manager.shapes[sort + 1];
					manager.shapes[sort + 1] = manager.shapes[sort];
					manager.shapes[sort] = temp;
				}
			}
		}
		for(int i=0; i<manager.shapes.Length; i++)
		{
			manager.shapes[i].transform.localPosition = new Vector3(manager.shapes[i].transform.localPosition.x,manager.shapes[i].transform.localPosition.y,posTemp);
			posTemp -= .01f;
		}
		
	}
}

