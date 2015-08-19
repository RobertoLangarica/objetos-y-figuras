using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Container400 : MonoBehaviour {
		
	[HideInInspector]
	public Rect area;
	[HideInInspector]
	public int value;
	[HideInInspector]
	public int secondValue;
	[HideInInspector]
	public bool active = true;
	[HideInInspector]
	public int next = -1;

	protected Image image;
	protected Color startColor;
	protected Color secondaryColor;
	protected bool _isEmpty;
	protected bool isHidden = false;
	[HideInInspector]
	public Vector2 alignedPos;


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

	public Vector2 max
	{
		get{return area.max;}
	}

	public Vector2 min
	{
		get{return area.min;}
	}

	public bool isEmpty
	{
		get{return _isEmpty;}
		set
		{
			_isEmpty = value;

			if(image)
			{
				Color c = _isEmpty ? startColor:secondaryColor;
				c.a = isHidden ? 0 : c.a;
				image.color = c;
			}
		}
	}

	protected Color currentColor;
	protected float finalAlpha;
	protected float elapsedTime;
	protected float alphaInverseDuration = 0.2f;
	protected bool hiding = false;
	protected float percent;

	public void hide(bool shouldHide,bool animate = true)
	{
		if(image)
		{
			Color color = _isEmpty ? startColor:secondaryColor;

			if(animate)
			{
				currentColor = image.color;
				elapsedTime = 0;
				alphaInverseDuration = 1/0.2f;
				finalAlpha = shouldHide ? 0:color.a;
				percent = 0;
				hiding = true;

				isHidden = shouldHide;
			}
			else
			{
				color.a = shouldHide ? 0:color.a;
				image.color = color;
			}
		}
	}

	void Update()
	{
		if(hiding)
		{
			percent = elapsedTime*alphaInverseDuration;
			currentColor.a = Mathf.SmoothStep(currentColor.a,finalAlpha,percent);
			
			if(currentColor.a == finalAlpha)
			{
				hiding = false;
			}
			
			image.color = currentColor;
			elapsedTime += Time.deltaTime;
		}
	}
}
