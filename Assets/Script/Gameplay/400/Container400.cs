using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Container400 : MonoBehaviour {

	public bool active = true;

	protected Rect area;
	[HideInInspector]
	public bool isEmpty;

	// Use this for initialization
	void Start () 
	{
		isEmpty = true;
	}

	public void setArea(Vector2 min, Vector2 max)
	{
		area = new Rect();
		area.max = max;
		area.min = min;
	}

	public bool Contains(Vector3 position)
	{
		return area.Contains(position);
	}
	
	public bool Contains(Vector2 position)
	{
		return area.Contains(position);
	}
	
	public bool Contains(Vector3 position,bool allowInverse)
	{
		return area.Contains(position,allowInverse);
	}

	public Vector2 getCenter()
	{
		return area.center;
	}
}
