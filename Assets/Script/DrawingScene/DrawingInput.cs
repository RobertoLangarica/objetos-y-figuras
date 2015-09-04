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
	public float drawDistanceTolerance = 0.125f;

	protected float tolerance;
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
	
		bWidth = brushType.renderer.bounds.size.x*brushType.transform.localScale.x;
		//Menos 2 pixeles (el sprite est a 100pixeles por cada unidad)
		bWidth -=  .02f;
		input.GetComponent<DragRecognizer>().OnGesture += OnDrag;

		if(randomizeColor)
		{
			currentColor = BaseShape.getColorFromIndex(Random.Range(0,System.Enum.GetValues(typeof(BaseShape.EShapeColor)).Length-1));
		}

		tolerance = drawDistanceTolerance*((Camera.main.orthographicSize*2*100)/Screen.height);
		tolerance *= tolerance;
	}

	void OnDrag(DragGesture gesture)
	{
		if (!paintStarted)return;

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
				spawnNewPoint(Camera.main.ScreenToWorldPoint(gesture.Position),false);
			}
		}
			break;
		case (ContinuousGesturePhase.Updated):
		{
			if(isErrasing)
			{
				moveErraser(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
			else
			{
				spawnNewPoint(Camera.main.ScreenToWorldPoint(gesture.Position),true);
				newLine = false;
			}
		}
			break;
			
		case (ContinuousGesturePhase.Ended):
		{
			erraser.SetActive(false);
			newLine = true;
		}
			break;
		}
	}
	
	protected void spawnNewPoint(Vector3 nVec3,bool processLastPoint)
	{
		if(!canDraw) return;



		GameObject go = null;
		float tempMag = 0;
		nVec3.z = 0;


		//Si el lapìz se desactiva durante el update y se vuelve a activar ya no hay lastPoint
		if(processLastPoint && allPainted.Count == 0)
		{
			processLastPoint = false;
		}

		//Si no a superado la tolerancia no se dibuja
		if(processLastPoint)
		{
			//Debug.Log ("Processing last point");
			Vector3 prevVec = allPainted[allPainted.Count-1].transform.position;
			tempMag = (prevVec - nVec3).sqrMagnitude;

			if(tempMag >= tolerance)
			{
				go = GameObject.Instantiate(brushType,nVec3,Quaternion.identity) as GameObject;
				go.transform.SetParent(paintedFather.transform);
				go.GetComponent<SpriteRenderer>().color = currentColor;

				tempMag = (prevVec - nVec3).magnitude;

				Vector3 nScale = go.transform.localScale;
				nScale.x *= (tempMag/bWidth);
				go.transform.localScale = nScale;
				
				//Rotado
				Vector3 difVec = prevVec - nVec3;
				Vector3 defaultVec = new Vector3(1,0,0);
				float rotZ = Vector3.Angle(difVec,defaultVec);

				if(prevVec.y < nVec3.y)
				{
					rotZ *= -1;
				}
				go.transform.localRotation = Quaternion.Euler(new Vector3(0,0,rotZ));

				allPainted.Add (go);
			}
		}
		else
		{
			go = GameObject.Instantiate(brushType,nVec3,Quaternion.identity) as GameObject;
			go.transform.SetParent(paintedFather.transform);
			go.GetComponent<SpriteRenderer>().color = currentColor;

			allPainted.Add (go);
		}
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
		if(CursorChanger.instance&&canDraw)
		{
			CursorChanger.instance.bPencil=false;
			CursorChanger.instance.pencil();
			CursorChanger.instance.bPencil=true;
		}
		erraser.SetActive(false);
		isErrasing = false;
	}

	public void switchToErase()
	{
		if(CursorChanger.instance)
		{
			CursorChanger.instance.bPencil=false;
			CursorChanger.instance.ereaser();
			CursorChanger.instance.bPencil=true;
		}
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
		if(canDraw)
		{
			switchToPaint();
		}
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
		if(hiding)
			return;

		if(canDraw)
		{
			switchToPaint();
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