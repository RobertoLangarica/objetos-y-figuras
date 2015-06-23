using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawingInput : MonoBehaviour 
{	
	public GameObject brushType;

	protected bool paintStarted = false;
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
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.B)) 
		{
			erraseAll();
		}
		if (Input.GetKeyDown (KeyCode.E)) 
		{
			switchBetweenEraseAndPaint();
		}
		
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

		switch(gesture.Phase)
		{			
		case (ContinuousGesturePhase.Updated):
		{
			if(!isErrasing)
			{
				spawnNewPoint(Camera.main.ScreenToWorldPoint(gesture.Position));
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
		}
			break;
		}
	}

	protected void spawnNewPoint(Vector3 nVec3)
	{
		GameObject go;

		nVec3.z = 0;
		go = GameObject.Instantiate(brushType,nVec3,Quaternion.identity) as GameObject;
		go.transform.SetParent(paintedFather.transform);
		allPainted.Add (go);
	}

	protected void moveErraser(Vector3 nVec3)
	{
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
}