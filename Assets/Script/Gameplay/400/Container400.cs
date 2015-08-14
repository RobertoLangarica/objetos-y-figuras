using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Container400 : MonoBehaviour {

	public bool active = true;

	protected Rect area;
	[HideInInspector]
	public int value;
	[HideInInspector]
	public int secondValue;

	protected Image image;
	protected Color startColor;
	protected Color secondaryColor;
	protected bool _isEmpty;

	// Use this for initialization
	void Start () 
	{
		_isEmpty = true;

		image = GetComponent<Image>();

		if(image)
		{
			startColor = image.color;
			secondaryColor = image.color;
			secondaryColor.a*=0.5f;
		}
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

	public bool isEmpty
	{
		get{return _isEmpty;}
		set
		{
			_isEmpty = value;

			if(image)
			{
				image.color = _isEmpty ? startColor:secondaryColor;
			}
		}
	}
}
