using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public RectTransform container;

	protected float boundaryTop;
	protected float boundaryBottom;
	protected float boundaryLeft;
	protected float boundaryRight;
	protected Vector3 size;

	protected float limitSpeed = 5;//cuadrada
	protected float width;//Ancho para el acomodo
	protected Vector2 force = new Vector2(0,10);//Fuerza que se agrega cada frame
	public string rotateDirection;
	
	void Start () 
	{
		//client = GameObject.FindObjectOfType<ClientManager>();
		
		boundaries();
	}
	
	void Update () 
	{
		//Movimiento

		//{
		//	if(rotateDirection != "")
		//	{
		//		rotation(rotateDirection == "right");
		//	}
		//	
		//	force = transform.up * 230;
		//	transform.rigidbody2D.AddForce(force);
		//	
		//	//Debug.Log(transform.rigidbody2D.velocity.sqrMagnitude);
		//	
		//	if(transform.rigidbody2D.velocity.sqrMagnitude > limitSpeed)
		//	{
		//		float v = limitSpeed/transform.rigidbody2D.velocity.sqrMagnitude;
		//		transform.rigidbody2D.velocity = transform.rigidbody2D.velocity*v; 
		//	}
		//}
		//Debug.Log(transform.localPosition.x);
		//Para que al salir vuelva a entrar
		checkBounds();
		
	}
	
	public void rotation(bool right)
	{
		if(right)
		{
			transform.rigidbody2D.angularVelocity -=5;
			if(transform.rigidbody2D.angularVelocity < -25)
			{
				transform.rigidbody2D.angularVelocity = -25;
			}
		}
		else
		{
			transform.rigidbody2D.angularVelocity +=5;
			if(transform.rigidbody2D.angularVelocity > 25)
			{
				transform.rigidbody2D.angularVelocity = 25;
			}
			//transform.Rotate(0,0,2);
		}
	}
	
	private void boundaries()
	{
		boundaryTop = Screen.height*0.5f;
		boundaryBottom = Screen.height*-0.5f;
		boundaryLeft = Screen.width*(-0.5f);
		boundaryRight = Screen.width*(0.5f);
		
		//Debug.Log(Screen.width*0.5f);
		//Debug.Log(Screen.height*0.5f);
	}
	
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
	
	public void moveShip(string direction)
	{
		rotateDirection = direction;
	}

}