using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DrawObject : MonoBehaviour 
{
	public bool withPencil = false;
	[HideInInspector]
	public DrawingInput dInput;

	protected GameObject HUD;

	void Start()
	{
		HUD = GetWhereToPutButtons(FindGameObjectsWithLayer(5)); // 5 es UI

		dInput = GameObject.Find("DrawTool").GetComponent<DrawingInput> ();

		if (HUD != null) 
		{
			addDrawButtons();
		}
	}

	protected GameObject[] FindGameObjectsWithLayer (int layer) 
	{ 
		GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[]; 
		List<GameObject> goList = new List<GameObject>(); 
		for (int i = 0; i < goArray.Length; i++) 
		{ 
			if (goArray[i].layer == layer) 
			{ 
				goList.Add(goArray[i]); 
			} 
		} 
		if (goList.Count == 0) 
		{ 
			return null; 
		} 
		return goList.ToArray(); 
	}

	protected GameObject GetWhereToPutButtons(GameObject[] allUIObjects)
	{
		foreach (GameObject val in allUIObjects) 
		{
			if(val.GetComponent<Canvas>() != null)
			{
				return val;
			}
		}
		return null;
	}

	protected void addDrawButtons()
	{
		GameObject tmp = (GameObject)Resources.Load("Drawing/Pencil");
		GameObject goP = null;;

		if(withPencil)
		{
			goP = GameObject.Instantiate (tmp) as GameObject;
			goP.GetComponent<Button>().onClick.AddListener(() => {
				dInput.change2Draw();
			});
			goP.transform.SetParent (HUD.transform);
			goP.transform.localScale = new Vector3 (1,1,1);
			goP.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
			goP.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
			dInput.canDraw = false;
		}

		tmp = (GameObject)Resources.Load("Drawing/EraseAllBtn");
		GameObject go = GameObject.Instantiate (tmp) as GameObject;
		go.GetComponent<Button>().onClick.AddListener(() => {
			dInput.erraseAll();
		});
		go.transform.SetParent (HUD.transform);
		go.transform.localScale = new Vector3 (1,1,1);
		go.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
		go.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
		if(withPencil) goP.GetComponent<Pencil>().EreaseAllBtn = go;

		tmp = (GameObject)Resources.Load("Drawing/Switch2EraseBtn");
		go = GameObject.Instantiate (tmp) as GameObject;
		go.GetComponent<Button>().onClick.AddListener(() => {
			dInput.switchBetweenEraseAndPaint();
		});
		go.transform.SetParent (HUD.transform);
		go.transform.localScale = new Vector3 (1,1,1);
		go.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
		go.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
		if(withPencil) goP.GetComponent<Pencil>().Switch2EraseBtn = go;
	}
}