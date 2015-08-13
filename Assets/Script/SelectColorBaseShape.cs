using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SelectColorBaseShape : MonoBehaviour {

	public enum EShapeColor
	{
		AQUA,
		BLACK,
		BLUE,
		GREEN,
		GREY,
		MAGENTA,
		RED,
		WHITE,
		YELLOW
	};


	protected List<int> colorList;
	protected GameObject[] pieces;
	// Use this for initialization
	void Start () {
		colorList = new List<int>();

		for(int i=0; i< System.Enum.GetValues(typeof(EShapeColor)).Length; i++)
		{
			colorList.Add(i);
		}
		pieces = GameObject.FindGameObjectsWithTag("MenuShape");
		SetColor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	protected void SetColor()
	{
		int rand; 
		foreach (GameObject piece in pieces) {
			rand = Random.Range(0,colorList.Count-1);
			piece.GetComponent<BaseShape>().color = (BaseShape.EShapeColor)System.Enum.GetValues(typeof(EShapeColor)).GetValue(colorList[rand]);
			colorList.RemoveAt(rand);
		}
	}
}
	