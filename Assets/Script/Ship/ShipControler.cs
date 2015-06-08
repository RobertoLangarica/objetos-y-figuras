using UnityEngine;
using System.Collections;

public class ShipControler : MonoBehaviour 
{
	protected float boundaryTop;
	protected float boundaryBottom;
	protected float boundaryLeft;
	protected float boundaryRight;
	protected Vector3 size;

	[HideInInspector]
	public bool startShip = false;
	protected float limitSpeed = 50;//cuadrada
	protected float width;//Ancho para el acomodo
	protected Vector2 force = new Vector2(0,10);//Fuerza que se agrega cada frame
	public int col;//hardcoding de posicion (fila) solo funciona en el editor
	public string rotateDirection;
	
	void Start () {
		boundaryTop = Camera.main.orthographicSize;
		boundaryBottom = -Camera.main.orthographicSize;
		boundaryLeft = -Camera.main.aspect * Camera.main.orthographicSize;
		boundaryRight = Camera.main.aspect * Camera.main.orthographicSize;

		//Le damos el ancho para que solo quepan 15 naves en pantalla
		Sprite sprite = GetComponent<SpriteRenderer>().sprite;
		width = Screen.width/20.0f;
		float height = Screen.height/10.0f;

		Debug.Log ("desired: " +width/(transform.localScale.x*sprite.bounds.extents.x*sprite.pixelsPerUnit));
		Debug.Log ("desired: " +height/(transform.localScale.y*sprite.bounds.extents.y*sprite.pixelsPerUnit));

		float scale = Mathf.Min(width/(transform.localScale.x*sprite.bounds.extents.x*sprite.pixelsPerUnit)
		                        ,height/(transform.localScale.y*sprite.bounds.extents.y*sprite.pixelsPerUnit));
		Debug.Log ("scale: "+scale);

		Debug.Log ("WIDTH: "+Screen.width+"    HEIGHT: "+Screen.height);

		width*=0.5f;
		transform.localScale = new Vector3(scale,scale,1.0f);
		Debug.Log ("WIDTH_SHIP: "+transform.localScale.x*sprite.bounds.extents.x*sprite.pixelsPerUnit+"    HEIGHT_SHIP: "+transform.localScale.y*sprite.bounds.extents.y*sprite.pixelsPerUnit);

		#if UNITY_EDITOR
		//setPosition(col);
		#endif
	}
	
	/**
	 * Pone la nave en la fila indicada
	 * */
	public void setPosition(int column)
	{
		col = column;
		Debug.Log ("Posicionando:"+column);
		//profundidad
		int depth = (column+1)*30;
		GetComponent<SpriteRenderer>().sortingOrder = depth;
		transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = depth-1;
		

		float y;
		float x;

		if(column < 10)
		{
			//arriba
			y = Screen.height*.6f;
		}
		else if(column < 20)
		{

			//en medio
			y = Screen.height*.35f;
			column-=10;
		}
		else
		{
			//abajo
			y = Screen.height*.25f;
			column-=20;
		}

		x = width*column + width*0.5f;


		Vector3 vpos = Camera.main.ScreenToWorldPoint(new Vector3(x,y,0));
		vpos.z = transform.position.z;
		transform.position = vpos;
	}

	public static Vector3 getPosition(int column)
	{
		float w = Screen.width/10.0f;

		float y;
		float x;
		
		if(column < 10)
		{
			//arriba
			y = Screen.height*.6f;
		}
		else if(column < 20)
		{
			
			//en medio
			y = Screen.height*.35f;
			column-=10;
		}
		else
		{
			//abajo
			//y = Screen.height*.1f;
			y = 0;
			column-=20;
		}
		
		x = w*column + w*0.5f;
		
		
		Vector3 vpos = Camera.main.ScreenToWorldPoint(new Vector3(x,y,0));
		vpos.z = 0;

		return vpos;
	}

	public void setDepth(int column)
	{
		//profundidad
		int depth = (column+1)*30;
		GetComponent<SpriteRenderer>().sortingOrder = depth;
		transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = depth-1;
	}

	void Update () 
	{
		//Movimiento
		if(startShip)
		{
			if(rotateDirection != "")
			{
				rotation(rotateDirection == "right");
			}

			force = gameObject.transform.up * 30;
			rigidbody2D.AddForce(force);

			if(rigidbody2D.velocity.sqrMagnitude > limitSpeed)
			{
				float v = limitSpeed/rigidbody2D.velocity.sqrMagnitude;
				rigidbody2D.velocity = rigidbody2D.velocity*v; 
			}
		}

		//Para que al salir vuelva a entrar
		checkBounds();

		//hacer limites su velocidad de rotacion
		//checkRotation();
	}

	public void ignition()
	{
		startShip = true;
	}

	public void rotation(bool right)
	{
		if(right)
		{
			rigidbody2D.angularVelocity -=5;
			if(rigidbody2D.angularVelocity < -25)
			{
				rigidbody2D.angularVelocity = -25;
			}
		}
		else
		{
			rigidbody2D.angularVelocity +=5;
			if(rigidbody2D.angularVelocity > 25)
			{
				rigidbody2D.angularVelocity = 25;
			}
			//transform.Rotate(0,0,2);
		}
	}

	/**
	 * Que no se salga por los bordes
	 * */
	void checkBounds()
	{
		if(transform.localPosition.y-(size.y*.5f)> boundaryTop)
		{
			transform.localPosition= new Vector2(transform.localPosition.x, boundaryBottom-(size.y*.5f));
		}
		if(transform.localPosition.y+.1f+(size.y*.5f) < boundaryBottom)
		{
			transform.localPosition= new Vector2(transform.localPosition.x, boundaryTop+(size.y*.5f));
		}
		if(transform.localPosition.x+(size.x*.5f) < boundaryLeft)
		{
			transform.localPosition= new Vector2(boundaryRight+(size.x*.5f), transform.localPosition.y);
		}
		if(transform.localPosition.x-.1f-(size.x*.5f)> boundaryRight)
		{
			transform.localPosition= new Vector2(boundaryLeft-(size.x*.5f), transform.localPosition.y);
		}
	}

	void checkRotation()
	{
		if(rigidbody2D.angularVelocity > 25)
		{
			rigidbody2D.angularVelocity = 25;
			float v = 50/rigidbody2D.angularVelocity;
			rigidbody2D.angularVelocity = rigidbody2D.angularVelocity*v; 
		}
		else if(rigidbody2D.angularVelocity < -25)
		{
			rigidbody2D.angularVelocity = -25;
			float v = -50/rigidbody2D.angularVelocity;
			rigidbody2D.angularVelocity = rigidbody2D.angularVelocity*v; 
		}
	}
}
