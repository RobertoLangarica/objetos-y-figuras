using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawingInput : MonoBehaviour 
{	
	public GameObject brushType;
	[HideInInspector]
	public bool canDraw = true;

	protected float bWidth;
	protected bool paintStarted = false;
	protected bool newLine = true;
	protected bool isErrasing = false;
	protected GameObject paintedFather;
	protected GameObject erraser;
	protected List<GameObject> allPainted = new List<GameObject>();
	
	void Start ()
	{
		paintedFather = new GameObject ();
		paintedFather.name = "Father";
		paintedFather.transform.SetParent (transform);

		erraser = transform.FindChild ("Erraser").gameObject;
		erraser.SetActive (false);

		GetComponent<DragRecognizer>().OnGesture += OnDrag;

		bWidth = brushType.renderer.bounds.size.x;
	}

	// Update is called once per frame
	void Update () 
	{		
		#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		if (Input.GetMouseButtonDown (0) && !paintStarted) 
		{
			paintStarted = true;
			if(!isErrasing)
			{
				spawnNewPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
			else
			{
				erraser.SetActive(true);
				moveErraser(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
		}
		else if (Input.GetMouseButtonUp (0) && paintStarted) 
		{
			erraser.SetActive(false);
			paintStarted = false;
		}
		#endif
		
		
		#if UNITY_ANDROID || UNITY_IOS
		if (Input.touchCount > 0 && !paintStarted) 
		{
			paintStarted = true;
			if(!isErrasing)
			{
				spawnNewPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
			else
			{
				erraser.SetActive(true);
				moveErraser(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
		}
		else if (Input.touchCount == 0 && paintStarted) 
		{
			erraser.SetActive(false);
			paintStarted = false;
		}
		#endif
	}

	void OnDrag(DragGesture gesture)
	{
		if (!paintStarted)return;

		float tempMag = 0;

		switch(gesture.Phase)
		{			
		case (ContinuousGesturePhase.Started):
		{
			newLine = true;
		}
			break;
		case (ContinuousGesturePhase.Updated):
		{
			if(!isErrasing)
			{
				Debug.Log (newLine);
				spawnNewPoint(Camera.main.ScreenToWorldPoint(gesture.Position));
				newLine = false;
			}
			else
			{
				moveErraser(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
		}
			break;
			
		case (ContinuousGesturePhase.Ended):
		{
			erraser.SetActive(false);
			paintStarted = false;
			newLine = true;
		}
			break;
		}
	}

	protected void spawnNewPoint(Vector3 nVec3,float disDif = -1)
	{
		if(!canDraw) return;
		GameObject go;
		float tempMag = 0;

		nVec3.z = 0;
		go = GameObject.Instantiate(brushType,nVec3,Quaternion.identity) as GameObject;
		go.transform.SetParent(paintedFather.transform);
		if(!newLine)
		{
			tempMag = (allPainted[allPainted.Count-1].transform.position - nVec3).magnitude;
			if(tempMag > bWidth)
			{
				Vector3 nScale = new Vector3(tempMag/bWidth,1,1);
				go.transform.localScale = nScale;
			}
		}
		allPainted.Add (go);
	}

	protected void moveErraser(Vector3 nVec3)
	{
		if(!canDraw) return;
		nVec3.z = 0;
		erraser.transform.position = nVec3;
	}

	public void switchBetweenEraseAndPaint()
	{
		if(!isErrasing)
		{
			erraser.SetActive(true);
			moveErraser(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			isErrasing = true;
		}
		else
		{
			erraser.SetActive(false);
			isErrasing = false;
		}
	}
	
	public void erraseAll()
	{
		foreach (GameObject val in allPainted) 
		{
			Destroy(val);
		}
		allPainted = new List<GameObject> ();
	}

	public void erraseThisBrush(GameObject go)
	{
		allPainted.Remove (go);
		Destroy (go);
	}

	public void change2Draw()
	{
		canDraw = canDraw == true ? false: true;
	}
}