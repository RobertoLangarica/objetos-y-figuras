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
		SIZE1,
		SIZE2,
		SIZE3,
		SIZE4,
		SIZE5,
		SIZE6,
		SIZE7,
		SIZE8,
		NONE
	};

	public SpriteRenderer spriteRenderer; 
	public Image image;
	public GameObject _rotateHandler = null;
	public GameObject _translateHandler = null;
	public EShapeColor initialColor = EShapeColor.NONE;//Color inicial
	public EShapeSize initialSize = EShapeSize.NONE;//Tamaño inicial
	public float initialAlpha = 1;//Alfa con el que inicia la figura
	public Vector3 scaleMultiplier = Vector3.one;//Se multiplica por las escalas predefinidas en size

	protected EShapeColor currentColor;
	protected EShapeSize currentSize;
	protected bool rotateH,translateH;
	protected Color rendererColor;
	protected float currentAlpha;

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
		if(spriteRenderer)
		{
			rendererColor = spriteRenderer.color;
		}
		else if(image)
		{
			rendererColor = image.color;
		}
		else
		{
			rendererColor = new Color(1,1,1);	
		}

		rendererColor.a = initialAlpha;
		currentAlpha = initialAlpha;

		//No ejecutamos el alpha ya que alpha modifica el color al igual que el setter de color
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
			initialColor = value;
			currentColor = value;

			switch(currentColor)
			{
			case EShapeColor.AQUA:
				rendererColor = new Color(0.376f, 0.698f, 0.639f); 
				break;
			case EShapeColor.BLACK:
				rendererColor = new Color(0.180f, 0.188f, 0.192f);
				break;
			case EShapeColor.BLUE:
				rendererColor = new Color(0.133f, 0.565f, 0.945f);
				break;
			case EShapeColor.GREEN:
				rendererColor = new Color(0.192f, 0.545f, 0.263f);
				break;
			case EShapeColor.GREY:
				rendererColor = new Color(0.392f, 0.514f, 0.584f);
				break;
			case EShapeColor.MAGENTA:
				rendererColor = new Color(0.643f, 0.059f, 0.482f);
				break;
			case EShapeColor.RED:
				rendererColor = new Color(0.965f, 0.282f, 0.427f);
				break;
			case EShapeColor.WHITE:
				rendererColor = new Color(0.910f, 0.937f, 0.957f);
				break;
			case EShapeColor.YELLOW:
				rendererColor = new Color(0.976f, 0.627f, 0.000f);
			break;
			case EShapeColor.NONE:
				return;
				break;
			}

			rendererColor.a = currentAlpha;

			if(spriteRenderer)
			{
				spriteRenderer.color = rendererColor;
			}
			else if(image)
			{
				image.color = rendererColor;
			}
		}
	}

	public static Color getColorFromIndex(int index)
	{
		switch((EShapeColor)index)
		{
		case EShapeColor.AQUA:
			return new Color(0.376f, 0.698f, 0.639f); 

		case EShapeColor.BLACK:
			return new Color(0.180f, 0.188f, 0.192f);

		case EShapeColor.BLUE:
			return new Color(0.133f, 0.565f, 0.945f);

		case EShapeColor.GREEN:
			return new Color(0.192f, 0.545f, 0.263f);

		case EShapeColor.GREY:
			return new Color(0.392f, 0.514f, 0.584f);

		case EShapeColor.MAGENTA:
			return new Color(0.643f, 0.059f, 0.482f);

		case EShapeColor.RED:
			return new Color(0.965f, 0.282f, 0.427f);

		case EShapeColor.WHITE:
			return new Color(0.910f, 0.937f, 0.957f);

		case EShapeColor.YELLOW:
			return new Color(0.976f, 0.627f, 0.000f);

		}

		return new Color();
	}
	
	public float alpha
	{
		get{return currentAlpha;}
		set
		{
			initialAlpha = value;
			currentAlpha = value;
			rendererColor.a = value;

			if(spriteRenderer)
			{
				spriteRenderer.color = rendererColor;
			}
			else if(image)
			{
				image.color = rendererColor;
			}
		}
	}
	
	public EShapeSize size
	{
		get{return currentSize;}
		set
		{
			initialSize = value;
			currentSize = value;

			if(spriteRenderer)
			{
				switch(currentSize)
				{
				case EShapeSize.SIZE1:
					transform.localScale = new Vector3(0.16666f*scaleMultiplier.x,0.16666f*scaleMultiplier.y,0.16666f*scaleMultiplier.z);
					break;
				case EShapeSize.SIZE2:
					transform.localScale = new Vector3(0.25000f*scaleMultiplier.x,0.25000f*scaleMultiplier.y,0.25000f*scaleMultiplier.z);
					break;
				case EShapeSize.SIZE3:
					transform.localScale = new Vector3(0.33333f*scaleMultiplier.x,0.33333f*scaleMultiplier.y,0.33333f*scaleMultiplier.z);
					break;
				case EShapeSize.SIZE4:
					transform.localScale = new Vector3(0.41666f*scaleMultiplier.x,0.41666f*scaleMultiplier.y,0.41666f*scaleMultiplier.z);
					break;
				case EShapeSize.SIZE5:
					transform.localScale = new Vector3(0.50000f*scaleMultiplier.x,0.50000f*scaleMultiplier.y,0.50000f*scaleMultiplier.z);
					break;
				case EShapeSize.SIZE6:
					transform.localScale = new Vector3(0.66666f*scaleMultiplier.x,0.66666f*scaleMultiplier.y,0.66666f*scaleMultiplier.z);
					break;
				case EShapeSize.SIZE7:
					transform.localScale = new Vector3(0.83333f*scaleMultiplier.x,0.83333f*scaleMultiplier.y,0.83333f*scaleMultiplier.z);
					break;
				case EShapeSize.SIZE8:
					transform.localScale = new Vector3(1*scaleMultiplier.x,1*scaleMultiplier.y,1*scaleMultiplier.z);
					break;
				}

				//Debug.Log (transform.localScale.y+"___"+scaleMultiplier);
			}
		}
		
	}

	public Vector3 getScaleFromSizeWithoutMultiplicator(int sizeIndex)
	{
		return getScaleFromSizeWithoutMultiplicator((EShapeSize)sizeIndex);
	}

	public Vector3 getScaleFromSizeWithoutMultiplicator(EShapeSize size)
	{
		switch(size)
		{
		case EShapeSize.SIZE1:
			return new Vector3(0.16666f,0.16666f,16666f);
			break;
		case EShapeSize.SIZE2:
			return new Vector3(0.25000f,0.25000f,16666f);
			break;
		case EShapeSize.SIZE3:
			return new Vector3(0.33333f,0.33333f,16666f);
			break;
		case EShapeSize.SIZE4:
			return new Vector3(0.41666f,0.41666f,16666f);
			break;
		case EShapeSize.SIZE5:
			return new Vector3(0.50000f,0.50000f,16666f);
			break;
		case EShapeSize.SIZE6:
			return new Vector3(0.66666f,0.66666f,16666f);
			break;
		case EShapeSize.SIZE7:
			return new Vector3(0.83333f,0.83333f,16666f);
			break;
		case EShapeSize.SIZE8:
			return new Vector3(1,1,1);
			break;
		}

		return Vector3.zero;
	}
	
	public void setSizeByInt(int value)
	{
		size = (EShapeSize)value;
	}

	public void setColorByInt(int value)
	{
		color = (EShapeColor)value;
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
