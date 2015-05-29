using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ClientManager : MonoBehaviour 
{
	public string serverIP = "127.0.0.1";
	public int serverPort = 80;

	protected bool _isPhotoReady;
	protected bool _isNameReady;
	protected bool _isShipReady;

	public string playerName;
	public string shipName;
	public Image img;

	protected bool connected;

	void Awake()
	{
		DontDestroyOnLoad(this);
	}

	void Start()
	{
		_isPhotoReady = _isNameReady = _isShipReady = false;
		connected = false;
		img.color = Color.red;

		connect();
	}

	void connect()
	{
		Debug.Log("CM->Trying to connect to the server.");
		Network.Connect(serverIP,serverPort);
	}

	void OnConnectedToServer() 
	{
		Debug.Log("CM->Connected to server*************");
		connected = true;
		img.color = Color.green;

		//Mandamos la información que este lista
		if(_isPhotoReady)
		{
			sendPhoto();
		}

		if(_isNameReady)
		{
			sendName();
		}

		if(_isShipReady)
		{
			sendShip();
		}
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) 
	{
		Debug.Log("CM->Disconnected from server: " + info);
		connected = false;
		img.color = Color.red;

		//Intentamos conectarnos de nuevo
		StartCoroutine("tryToRecconect");
	}

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("CM->Could not connect to server: " + error);

		//Intentamos conectarnos de nuevo
		StartCoroutine("tryToRecconect");
	}

	IEnumerator tryToRecconect() 
	{
		Debug.Log("CM->Reconnecting");

		yield return new WaitForSeconds(2.0f);

		connect();
	}

	void OnDestroy()
	{
		Debug.Log("Destroying client.");
		
		//Cerramos la conexiones
		Network.Disconnect(200);
	}

	public void sendPhoto()
	{
		Debug.Log("CM->Mandando foto.");
		if(connected)
		{
			byte[] bytes = File.ReadAllBytes(WebCamPhotoCamera.PHOTO_PATHRESIZED);

			string data = System.Convert.ToBase64String(bytes);

			Debug.Log ("LENGTH:"+data.Length);

			networkView.RPC("onPlayerPhoto",RPCMode.Server,data);
		}
	}

	public void sendShip()
	{
		Debug.Log("CM->Mandando nave.");
		if(connected)
		{	
			networkView.RPC("onPlayerShip",RPCMode.Server,shipName);
		}
	}

	public void sendName()
	{
		Debug.Log("CM->Mandando nombre.");
		if(connected)
		{	
			networkView.RPC("onPlayerName",RPCMode.Server,playerName);
		}
	}

	public void moveShip(string direction)
	{
		Debug.Log("CM->moviendo nave.");
		if(connected)
		{	
			networkView.RPC("onShipMove",RPCMode.Server,direction);
		}
	}

	[RPC]
	void onPlayerPhoto(string data)
	{
		//Homonimo del servidor
	}

	[RPC]
	void onPlayerName(string data)
	{
		//Homonimo del servidor
	}
	
	[RPC]
	void onPlayerShip(string data)
	{
		//Homonimo del servidor
	}

	[RPC]
	void onInitSpaceMode()
	{
		if(!(Application.loadedLevelName == "PhotoScene" 
		   || Application.loadedLevelName == "Splash"
		   || Application.loadedLevelName == "SplashVilla"
		   || Application.loadedLevelName == "StartMenu"))
		{
			if(shipName != "")
			{
				Debug.Log("tiene");
				ShipTravelController.shipName = shipName;
			}
			else
			{
				Debug.Log("notiene");
				ShipTravelController.shipName = "SN01_01";
				sendShip();
			}
			//Hay que mandar a la pantalla del espacio
			ScreenManager.instance.GoToScene("SpaceClient");
		}
	}

	[RPC]
	void onStopSapcemode()
	{
		if(!(Application.loadedLevelName == "PhotoScene" 
		     || Application.loadedLevelName == "Splash"
		     || Application.loadedLevelName == "SplashVilla"
		     || Application.loadedLevelName == "StartMenu"))
		{
			//Homonimo del cliente
			ScreenManager.instance.GoToScene("Splash");
		}
	}

	[RPC]
	void onShipMove(string data)
	{
		//Homonimo del servidor
	}

	public bool isPhotoReady 
	{
		get 
		{
			return _isPhotoReady;
		}
		set 
		{
			_isPhotoReady = value;
			
			if(value)
			{
				//Mandamos la foto
				sendPhoto();
			}
		}
	}

	public bool isNameReady 
	{
		get 
		{
			return _isNameReady;
		}
		set 
		{
			_isNameReady = value;
			
			if(value)
			{
				//Mandamos el nombre
				sendName();
			}
		}
	}

	public bool isShipReady 
	{
		get 
		{
			return _isShipReady;
		}
		set 
		{
			_isShipReady = value;
			
			if(value)
			{
				//Mandamos la nave
				sendShip();
			}
		}
	}
}
