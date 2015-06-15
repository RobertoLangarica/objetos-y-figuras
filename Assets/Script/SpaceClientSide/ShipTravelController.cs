using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipTravelController : MonoBehaviour 
{
	public RectTransform container;

	public static string shipName = "SN01_01";

	public Text txt;

	protected GameObject ship;
	//protected ClientManager client;

	protected float boundaryTop;
	protected float boundaryBottom;
	protected float boundaryLeft;
	protected float boundaryRight;
	protected Vector3 size;

	//[HideInInspector]
	public bool startShip = false;
	protected float limitSpeed = 2;//cuadrada
	protected float width;//Ancho para el acomodo
	protected Vector2 force = new Vector2(0,10);//Fuerza que se agrega cada frame
	public int col;//hardcoding de posicion (fila) solo funciona en el editor
	public string rotateDirection;
	
	protected int currentCount = 5;
	protected bool gameRunning = false;

	void Start () 
	{
		//client = GameObject.FindObjectOfType<ClientManager>();
		shipName = ScreenManager.instance.myCurrentShip;//"SN01_01";//
		Debug.Log(shipName);

		GameObject tmp = (GameObject)Resources.Load("SpaceShips/"+shipName);
		
		ship = GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity) as GameObject;

		ship.AddComponent<CircleCollider2D>();
		ship.gameObject.GetComponent<CircleCollider2D>().radius = 1.5f;
		//Debug.Log(Screen.height+"  "+ship.transform.FindChild("turbina").renderer.bounds.size);
		ship.transform.localScale = new Vector3(.15f,.15f,.15f);
		//ship.transform.SetParent(container,false);
		ship.AddComponent<Rigidbody2D>();
		ship.rigidbody2D.gravityScale =0;

		boundaries();
		
		currentCount = 5;
		starCronometer ();
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

		#if UNITY_EDITOR_WIN 
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			moveShip("left");
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			moveShip("right");
		}
		#endif

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
		//boundaryTop = Screen.height*0.5f;
		//boundaryBottom = Screen.height*-0.5f;
		//boundaryLeft = Screen.width*(-0.5f);
		//boundaryRight = Screen.width*(0.5f);
		
		//Debug.Log(Screen.width*0.5f);
		//Debug.Log(Screen.height*0.5f);

		boundaryTop = Camera.main.orthographicSize;
		boundaryBottom = -Camera.main.orthographicSize;
		boundaryLeft = -Camera.main.aspect * Camera.main.orthographicSize;
		boundaryRight = Camera.main.aspect * Camera.main.orthographicSize;
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
	}
	
	public void starCronometer()
	{
		gameRunning = true;
		StartCoroutine("cronometerCount");
	}
	
	IEnumerator cronometerCount()
	{
		if(currentCount > 0)
		{
			txt.enabled = true;
			txt.text = currentCount.ToString();//img.sprite = Resources.Load(currentCount.ToString(),typeof(Sprite)) as Sprite;
			currentCount--;
			yield return new WaitForSeconds (1);
			StartCoroutine("cronometerCount");
		}
		else
		{
			txt.enabled = false;
			ignition();
		}
	}
}