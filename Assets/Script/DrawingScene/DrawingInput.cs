using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawingInput : MonoBehaviour 
{	
	public GameObject brushType;
	[HideInInspector]
	public bool canDraw = true;
	public GameObject input;
	public bool hiding = false;
	public bool randomizeColor = true;
	protected float bWidth;
	protected bool paintStarted = true;
	protected bool newLine = true;
	protected bool isErrasing = false;
	protected GameObject paintedFather;
	protected GameObject erraser;
	protected List<GameObject> allPainted = new List<GameObject>();

	[HideInInspector]
	public Color currentColor = new Color(0.133f, 0.565f, 0.945f);
	
	void Start ()
	{
		paintedFather = new GameObject ();
		paintedFather.name = "Father";
		paintedFather.transform.SetParent (transform);

		erraser = transform.FindChild ("Erraser").gameObject;
		erraser.SetActive (false);
	
		bWidth = brushType.renderer.bounds.size.x*brushType.transform.localScale.x*0.5f;
		input.GetComponent<DragRecognizer>().OnGesture += OnDrag;

		if(randomizeColor)
		{
			currentColor = BaseShape.getColorFromIndex(Random.Range(0,System.Enum.GetValues(typeof(BaseShape.EShapeColor)).Length-1));
		}
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
			if(isErrasing)
			{
				erraser.SetActive(true);
			}
			else
			{
				spawnNewPoint(Camera.main.ScreenToWorldPoint(gesture.Position));
			}
		}
			break;
		case (ContinuousGesturePhase.Updated):
		{
			if(!isErrasing)
			{
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
			//paintStarted = false;
			newLine = true;
		}
			break;
		}
	}

	//protected int count = 0;
	protected void spawnNewPoint(Vector3 nVec3)
	{
		if(!canDraw) return;
		GameObject go;
		float tempMag = 0;

		nVec3.z = 0;
		go = GameObject.Instantiate(brushType,nVec3,Quaternion.identity) as GameObject;
		go.transform.SetParent(paintedFather.transform);
		/*count++;
		if(count >= 50)
		{
			count = 0;
			currentColor = BaseShape.getColorFromIndex(Random.Range(0,System.Enum.GetValues(typeof(BaseShape.EShapeColor)).Length-1));
		}*/
		go.GetComponent<SpriteRenderer>().color = currentColor;

		if(!newLine)
		{
			Vector3 prevVec = allPainted[allPainted.Count-1].transform.position;
			tempMag = (prevVec - nVec3).magnitude;
			if(tempMag > bWidth)
			{
				Vector3 difVec = prevVec - nVec3;
				Vector3 defaultVec = new Vector3(1,0,0);
				float rotZ = Vector3.Angle(difVec,defaultVec);
				if(prevVec.y < nVec3.y)
				{
					rotZ *= -1;
				}

				Vector3 nScale = go.transform.localScale;
				nScale.x *= (tempMag/bWidth)*0.5f;
				go.transform.localScale = nScale;
				go.transform.localRotation = Quaternion.Euler(new Vector3(0,0,rotZ));
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

	public void switchToPaint()
	{
		erraser.SetActive(false);
		isErrasing = false;
	}

	public void switchToErase()
	{
		erraser.SetActive(true);
		moveErraser(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		isErrasing = true;
	}

	public void erraseAll()
	{
		foreach (GameObject val in allPainted) 
		{
			Destroy(val);
		}
		allPainted = new List<GameObject> ();
		switchToPaint();
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
	public void drawingTrue()
	{
		switchToPaint();
		if(hiding)
			return;
		if(canDraw)
		{
			foreach (GameObject val in allPainted) 
			{
				val.SetActive(true);
			}
		}
		else
		{
			foreach (GameObject val in allPainted) 
			{
				val.SetActive(false);
			}
		}
	}
}