using UnityEngine;
using System.Collections;
using DG.Tweening.Core;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseShape : MonoBehaviour {

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
		YELLOW,
		NONE
	};

	public enum EShapeSize
	{
		SIZE5,
		SIZE4,
		SIZE3,
		SIZE2,
		SIZE1,
		NONE
	};

	public SpriteRenderer spriteRenderer; 
	public GameObject _rotateHandler = null;
	public GameObject _translateHandler = null;
	public EShapeColor initialColor = EShapeColor.NONE;
	public EShapeSize initialSize = EShapeSize.NONE;

	protected EShapeColor currentColor;
	protected EShapeSize currentSize;
	protected bool rotateH,translateH;

	void Awake()
	{
		rotateHandler = false;
		translateHandler = false;
	}

	void Start()
	{
		baseStart();
	}

	public void baseStart()
	{
		color = initialColor;
		size = initialSize;
	}

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


	public EShapeColor color
	{
		//get{return spriteRenderer.color;}
		//set{spriteRenderer.color = value;}

		get{return currentColor;}
		set{currentColor = value;}

	}
	
	public EShapeSize size
	{
		get{return currentSize;}
		set
		{
			currentSize = value;

			switch(currentSize)
			{
			case EShapeSize.SIZE1:
				transform.localScale = new Vector3(0.33333f,0.33333f,1);
				break;
			case EShapeSize.SIZE2:
				transform.localScale = new Vector3(0.50000f,0.50000f,1);
				break;
			case EShapeSize.SIZE3:
				transform.localScale = new Vector3(0.66666f,0.66666f,1);
				break;
			case EShapeSize.SIZE4:
				transform.localScale = new Vector3(0.83333f,0.83333f,1);
				break;
			case EShapeSize.SIZE5:
				transform.localScale = new Vector3(1,1,1);
				break;
			}
		}
		
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
}
