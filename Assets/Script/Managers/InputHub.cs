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
	protected bool nextDragIsAnchor = false;//Indica si el siguiente draggin se trata como anchor


	void Start ()
	{
		selected = null;
		manager = FindObjectOfType<GameManager>();

		GetComponent<DragRecognizer>().OnGesture += OnDrag;
		GetComponent<TwistRecognizer>().OnGesture += OnTwist;
	}
	


	void OnDrag(DragGesture gesture)
	{
		switch(gesture.Phase)
		{
			case ContinuousGesturePhase.Started:
			if(gesture.StartSelection)
			{
				selected = gesture.StartSelection.GetComponent<Shape>();
				selected.onTouchBegan(Camera.main.ScreenToWorldPoint(gesture.StartPosition));
				selected.onTouchMove(Camera.main.ScreenToWorldPoint(gesture.Position));
			}
			break;

			case ContinuousGesturePhase.Updated:
			if(selected && !isRotating)
			{
				if(nextDragIsAnchor)
				{
					selected.onTouchBegan(Camera.main.ScreenToWorldPoint(gesture.Position));
					nextDragIsAnchor = false;
				}
				else
				{
					selected.onTouchMove(Camera.main.ScreenToWorldPoint(gesture.Position));
				}
			}
			break;

			case ContinuousGesturePhase.Ended:
			if(selected)
			{
				stopSelected();
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
				selected.onTouchBegan(Camera.main.ScreenToWorldPoint(gesture.Position));
				selected.transform.Rotate(Vector3.back,-gesture.DeltaRotation);
				break;

			case ContinuousGesturePhase.Updated:
				selected.transform.Rotate(Vector3.back,-gesture.DeltaRotation);
				selected.onTouchMove(Camera.main.ScreenToWorldPoint(gesture.Position));
				break;

			case ContinuousGesturePhase.Ended:
			isRotating = false;
			if(selected)//Evitando alguna race condition
			{
				nextDragIsAnchor = true;
				selected.onRotationComplete();
			}
			break;
		}
	}

	void stopSelected()
	{
		if(isRotating)
		{
			isRotating = false;
			selected.onRotationComplete();
		}
		nextDragIsAnchor = false;

		selected.onTouchStop();
		selected = null;
		isRotating = false;
		manager.checkForLevelComplete();
		DOTween.Play("SnapMove");
	}
}

