using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DragRecognizer))]
public class GroupInput : MonoBehaviour 
{
	protected GroupFigure selected;
	protected Vector3 initialP = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
		GetComponent<DragRecognizer> ().OnGesture += OnDrag;
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
				selected = gesture.StartSelection.GetComponent<GroupFigure>();
				selected.onTouchBegan(Camera.main.ScreenToWorldPoint(gesture.StartPosition));
				initialP = Input.mousePosition;
			}
			break;
			
		case ContinuousGesturePhase.Updated:
			if(selected && Input.mousePosition != initialP)
			{
				selected.onTouchMove(Camera.main.ScreenToWorldPoint(gesture.Position));
				initialP = Input.mousePosition;
			}
			break;
			
		case ContinuousGesturePhase.Ended:
			//selected.transform.localPosition=new Vector3(selected.transform.localPosition.x,selected.transform.localPosition.y,-0.1f);
			
			if(selected)
			{
				selected.opnTouchEnded();
				selected = null;
			}
			break;
		}
	}
}
