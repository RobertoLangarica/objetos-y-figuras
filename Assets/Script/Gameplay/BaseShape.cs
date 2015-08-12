using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening.Core;
using DG.Tweening;

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
	public Image image;
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
		get{
			if(spriteRenderer)
			{
				return spriteRenderer.sortingLayerName;
			}

			return "";
		}
		set
		{
			if(spriteRenderer)
			{
				spriteRenderer.sortingLayerName = value;
			}
		}
	}
	
	public int sortingOrder
	{
		get
		{
			if(spriteRenderer)
			{
				return spriteRenderer.sortingOrder;
			}

			return 0;
		}
		set
		{
			if(spriteRenderer)
			{
				spriteRenderer.sortingOrder = value;
			}
		}
	}


	public EShapeColor color
	{
		//get{return spriteRenderer.color;}
		//set{spriteRenderer.color = value;}

		get{return currentColor;}
		set
		{
			currentColor = value;

			switch(currentColor)
			{
			case EShapeColor.AQUA:
				if(spriteRenderer)
				{
					spriteRenderer.color = new Color(0.376f, 0.698f, 0.639f);
				}
				else if(image)
				{
					image.color = new Color(0.376f, 0.698f, 0.639f);
				}
				break;
			case EShapeColor.BLACK:
				if(spriteRenderer)
				{
					spriteRenderer.color = new Color(0.180f, 0.188f, 0.192f);
				}
				else if(image)
				{
					image.color = new Color(0.180f, 0.188f, 0.192f);
				}
				break;
			case EShapeColor.BLUE:
				if(spriteRenderer)
				{
					spriteRenderer.color = new Color(0.133f, 0.565f, 0.945f);
				}
				else if(image)
				{
					image.color = new Color(0.133f, 0.565f, 0.945f);
				}
				break;
			case EShapeColor.GREEN:
				if(spriteRenderer)
				{
					spriteRenderer.color = new Color(0.192f, 0.545f, 0.263f);
				}
				else if(image)
				{
					image.color = new Color(0.192f, 0.545f, 0.263f);
				}
				break;
			case EShapeColor.GREY:
				if(spriteRenderer)
				{
					spriteRenderer.color = new Color(0.392f, 0.514f, 0.584f);
				}
				else if(image)
				{
					image.color = new Color(0.392f, 0.514f, 0.584f);
				}
				break;
			case EShapeColor.MAGENTA:
				if(spriteRenderer)
				{
					spriteRenderer.color = new Color(0.643f, 0.059f, 0.482f);
				}
				else if(image)
				{
					image.color = new Color(0.643f, 0.059f, 0.482f);
				}
				break;
			case EShapeColor.RED:
				if(spriteRenderer)
				{
					spriteRenderer.color = new Color(0.965f, 0.282f, 0.427f);
				}
				else if(image)
				{
					image.color = new Color(0.965f, 0.282f, 0.427f);
				}
				break;
			case EShapeColor.WHITE:
				if(spriteRenderer)
				{
					spriteRenderer.color = new Color(0.910f, 0.937f, 0.957f);
				}
				else if(image)
				{
					image.color = new Color(0.910f, 0.937f, 0.957f);
				}
				break;
			case EShapeColor.YELLOW:
				if(spriteRenderer)
				{
					spriteRenderer.color = new Color(0.976f, 0.627f, 0.000f);
				}
				else if(image)
				{
					image.color = new Color(0.976f, 0.627f, 0.000f);
				}
				break;
			}
		}
	}
	
	public EShapeSize size
	{
		get{return currentSize;}
		set
		{
			currentSize = value;

			if(spriteRenderer)
			{
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
