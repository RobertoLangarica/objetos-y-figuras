using UnityEngine;
using System.Collections;

public class ShipTravelController : MonoBehaviour 
{
	public RectTransform container;

	public static string shipName = "SN01_03";//ScreenManager.instance.myCurrentShip; //"SN01_01";

	protected GameObject ship;
	//protected ClientManager client;

	protected float boundaryTop;
	protected float boundaryBottom;
	protected float boundaryLeft;
	protected float boundaryRight;
	protected Vector3 size;

	//[HideInInspector]
	public bool startShip = false;
	protected float limitSpeed = 5;//cuadrada
	protected float width;//Ancho para el acomodo
	protected Vector2 force = new Vector2(0,10);//Fuerza que se agrega cada frame
	public int col;//hardcoding de posicion (fila) solo funciona en el editor
	public string rotateDirection;

	void Start () 
	{
		//client = GameObject.FindObjectOfType<ClientManager>();
		Debug.Log(shipName);

		GameObject tmp = (GameObject)Resources.Load("SpaceClient/"+shipName+"_client");
		
		ship = GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity) as GameObject;

		ship.transform.localScale = new Vector3(.2f,.2f,.2f);
		ship.transform.SetParent(container,false);
		ship.AddComponent<Rigidbody2D>();
		ship.rigidbody2D.gravityScale =0;

		boundaries();
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

			force = ship.transform.up * 230;
			ship.transform.rigidbody2D.AddForce(force);

			//Debug.Log(ship.transform.rigidbody2D.velocity.sqrMagnitude);

			if(ship.transform.rigidbody2D.velocity.sqrMagnitude > limitSpeed)
			{
				float v = limitSpeed/ship.transform.rigidbody2D.velocity.sqrMagnitude;
				ship.transform.rigidbody2D.velocity = ship.transform.rigidbody2D.velocity*v; 
			}
		}
		//Debug.Log(ship.transform.localPosition.x);
		//Para que al salir vuelva a entrar
		checkBounds();

	}

	public void rotation(bool right)
	{
		if(right)
		{
			ship.transform.rigidbody2D.angularVelocity -=5;
			if(ship.transform.rigidbody2D.angularVelocity < -25)
			{
				ship.transform.rigidbody2D.angularVelocity = -25;
			}
		}
		else
		{
			ship.transform.rigidbody2D.angularVelocity +=5;
			if(ship.transform.rigidbody2D.angularVelocity > 25)
			{
				ship.transform.rigidbody2D.angularVelocity = 25;
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
		if(ship.transform.localPosition.y-(size.y*.5f)> boundaryTop)
		{
			ship.transform.localPosition= new Vector2(ship.transform.localPosition.x, boundaryBottom-(size.y*.5f));
		}
		if(ship.transform.localPosition.y+.1f+(size.y*.5f) < boundaryBottom)
		{
			ship.transform.localPosition= new Vector2(ship.transform.localPosition.x, boundaryTop+(size.y*.5f));
		}
		if(ship.transform.localPosition.x+(size.x*.5f) < boundaryLeft)
		{
			ship.transform.localPosition= new Vector2(boundaryRight+(size.x*.5f), ship.transform.localPosition.y);
		}
		if(ship.transform.localPosition.x-.1f-(size.x*.5f)> boundaryRight)
		{
			ship.transform.localPosition= new Vector2(boundaryLeft-(size.x*.5f), ship.transform.localPosition.y);
		}
	}

	public void moveShip(string direction)
	{
		rotateDirection = direction;
	}
	public void ignition()
	{
		startShip = true;
		GameObject.Find("Start").SetActive(false);
	}
}