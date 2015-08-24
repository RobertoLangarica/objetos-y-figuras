using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DragRecognizer))]
public class GroupInput : MonoBehaviour 
{
	public AudioSource dragSound;

	protected GroupFigure selected;
	protected Vector3 initialP = Vector3.zero;

	//Para el audio
	protected float elapsedDragTime;
	protected float dragSpeed;

	// Use this for initialization
	void Start () 
	{
		GetComponent<DragRecognizer> ().OnGesture += OnDrag;
		
		if(dragSound)
		{
			dragSound.pitch = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrag(DragGesture gesture)
	{
		switch(gesture.Phase)
		{
		case ContinuousGesturePhase.Started:
			if(gesture.StartSelection)
			{
				if(dragSound)
				{
					dragSound.Play();
					elapsedDragTime = 0;
				}
				selected = gesture.StartSelection.GetComponent<GroupFigure>();
				selected.onTouchBegan(Camera.main.ScreenToWorldPoint(gesture.StartPosition));
				initialP = Input.mousePosition;
			}
			break;
			
		case ContinuousGesturePhase.Updated:
			if(selected && Input.mousePosition != initialP)
			{
				if(dragSound)
				{
					if((gesture.ElapsedTime-elapsedDragTime) == 0)
					{
						dragSpeed = gesture.DeltaMove.sqrMagnitude;
						dragSound.pitch = percent(0,9000000,dragSpeed)*3;
						elapsedDragTime = gesture.ElapsedTime;
					}
					else
					{
						dragSpeed = gesture.DeltaMove.sqrMagnitude/((gesture.ElapsedTime-elapsedDragTime)*(gesture.ElapsedTime-elapsedDragTime));
						dragSound.pitch = percent(0,9000000,dragSpeed)*3;
						elapsedDragTime = gesture.ElapsedTime;
					}
				}
				selected.onTouchMove(Camera.main.ScreenToWorldPoint(gesture.Position));
				initialP = Input.mousePosition;
			}
			break;
			
		case ContinuousGesturePhase.Ended:
			//selected.transform.localPosition=new Vector3(selected.transform.localPosition.x,selected.transform.localPosition.y,-0.1f);
			
			if(dragSound)
			{
				dragSound.Stop();
				dragSound.pitch = 0;
			}
			if(selected)
			{
				selected.opnTouchEnded();
				selected = null;
			}
			break;
		}
	}
	
	float percent(float min, float max, float value)
	{
		if(value >= max)
		{
			return 1;
		}
		
		if(value <= min)
		{
			return 0;
		}
		
		return (value-min)/(max-min);
	}
}
