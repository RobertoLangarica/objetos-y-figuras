using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseShape : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public GameObject _rotateHandler = null;
	public GameObject _translateHandler = null;
	
	public string sortingLayer
	{
		get{return spriteRenderer.sortingLayerName;}
		set{spriteRenderer.sortingLayerName = value;}
	}
	
	public int sortingOrder
	{
		get{return spriteRenderer.sortingOrder;}
		set{spriteRenderer.sortingOrder = value;}
	}
	
	public Color color
	{
		get{return spriteRenderer.color;}
		set{spriteRenderer.color = value;}
	}

	void Awake()
	{
		if(_rotateHandler != null)
		{
			_rotateHandler.SetActive(false);
		}
		
		if(_translateHandler != null)
		{
			_translateHandler.SetActive(false);
		}
	}
	
	public bool rotateHandler
	{
		set
		{
			if(_rotateHandler != null)
			{
				_rotateHandler.SetActive(value);
			}
		}
	}
	
	public bool translateHandler
	{
		set
		{
			if(_translateHandler != null)
			{
				_translateHandler.SetActive(value);
			}
		}
	}
	
	/*void Update()
	{
		if(_rotateHandler != null)
		{
			_rotateHandler.transform.eulerAngles = Vector3.zero;
		}
	}*/
}
