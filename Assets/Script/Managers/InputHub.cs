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
			if(wheel != 0)
			{
				lastRotation = Time.time;
				selected.transform.Rotate(Vector3.back,wheel*15);
			}
			else if(lastRotation != -1 && Time.time-lastRotation >= timeToSnapRotation)
			{
				stopRotation();
				manager.checkForLevelComplete();
			}
		}
	}

	void OnDrag(DragGesture gesture)
	{
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
				selected.onTouchMove(Camera.main.ScreenToWorldPoint(gesture.Position));
			}
			if(!gesture.StartSelection)
			{
				selected.onTouchBegan(Camera.main.ScreenToWorldPoint(gesture.StartPosition));
			}
			break;

			case ContinuousGesturePhase.Updated:
			if(selected && !isRotating)
			{
				selected.onTouchMove(Camera.main.ScreenToWorldPoint(gesture.Position));
				selected.turnRotationSpriter(false);
			}
			break;

			case ContinuousGesturePhase.Ended:
			if(selected)
			{
				selected.turnRotationSpriter(true);
				manager.checkForLevelComplete();
			}
			break;
		}
	}

	void OnTwist(TwistGesture gesture)
	{
		if(selected == null)return;

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
			isRotating = false;
			if(selected)//Evitando alguna race condition
			{
				selected.onRotationComplete();
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
		if(isRotating)
		{
			stopRotation();
		}
		selected.onTouchStop();
		selected.turnRotationSpriter(false);
		selected = null;
		isRotating = false;
		manager.checkForLevelComplete();
		DOTween.Play("SnapMove");
	}
}

