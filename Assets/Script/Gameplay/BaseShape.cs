using UnityEngine;
using System.Collections;
using DG.Tweening.Core;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseShape : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public GameObject _rotateHandler = null;
	public GameObject _translateHandler = null;
	protected bool rotateH,translateH;
	
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
		rotateHandler = false;
		translateHandler = false;
	}
	
	public bool rotateHandler
	{
		set
		{
			rotateH = value;
			if(_rotateHandler != null)
			{
				_rotateHandler.SetActive(value);
			}
		}
		get{return rotateH;}
	}
	
	public bool translateHandler
	{
		set
		{
			translateH = value;
			if(_translateHandler != null)
			{
				_translateHandler.SetActive(value);
			}
		}
		get{return translateH;}
	}
	
	/*void Update()
	{
		if(_rotateHandler != null)
		{
			_rotateHandler.transform.eulerAngles = Vector3.zero;
		}
	}*/
}
