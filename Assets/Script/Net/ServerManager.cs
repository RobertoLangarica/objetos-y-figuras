using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour {

	public delegate void ResponseDelegate(string sender,string data);

	public ResponseDelegate onShip;
	public ResponseDelegate onName;
	public ResponseDelegate onPhoto;
	public ResponseDelegate onMove;

	public int maxConnections = 50;
	public int listeningPort = 80;
	
	protected List<NetworkPlayer> players = new List<NetworkPlayer>();

	void Start () 
	{
		Debug.Log("SM->Inicializando Server.");
		//Inicializamos delegates
		onShip	= foo;
		onName	= foo;
		onPhoto = foo;
		onMove	= foo;

		//Inicializamos el server
		Network.InitializeServer(maxConnections,listeningPort,false);
	}

	//Para que los delegate no truenen
	void foo(string a,string b){}

	void OnServerInitialized()
	{
		Debug.Log("SM-> Server initialized");
		Debug.Log("SM-> ServerIP: "+Network.player.ipAddress);
		Debug.Log("SM-> ServerPort: "+Network.player.port);
		Debug.Log("SM-> ServerExternalIP: "+Network.player.externalIP);
		Debug.Log("SM-> ServerExternalPort: "+Network.player.externalPort);
	}


	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("SM-> Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		players.Remove(player);
	}

	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log ("SM-> Player connected"+player);
		players.Add(player);
	}

	void OnDestroy()
	{
		Debug.Log("SM->Destroying Server");
		foreach(NetworkPlayer player in players)
		{
			Network.RemoveRPCs(player);
			Network.DestroyPlayerObjects(player);
		}

		//Cerramos las conexiones
		Network.Disconnect(200);

		players.Clear();
	}

	public void initSpaceMode()
	{
		networkView.RPC("onInitSpaceMode",RPCMode.Others);
	}

	public void stopSpaceMode()
	{
		networkView.RPC("onStopSapcemode",RPCMode.Others);
	}

	[RPC]
	void onInitSpaceMode()
	{
		//Homonimo del cliente
	}

	[RPC]
	void onStopSapcemode()
	{
		//Homonimo del cliente
	}

	[RPC]
	void onPlayerPhoto(string data,NetworkMessageInfo info)
	{
		Debug.Log("SM->Received photo: "+info.sender);
		onPhoto(info.sender.ToString(),data);
	}

	[RPC]
	void onPlayerName(string data,NetworkMessageInfo info)
	{
		Debug.Log("SM->Received name: "+info.sender);
		onName(info.sender.ToString(),data);
	}

	[RPC]
	void onPlayerShip(string data,NetworkMessageInfo info)
	{
		Debug.Log("SM->Received ship: "+info.sender);
		onShip(info.sender.ToString(),data);
	}

	[RPC]
	void onShipMove(string data,NetworkMessageInfo info)
	{
		Debug.Log("SM->Received move: "+info.sender);
		onMove(info.sender.ToString(),data);
	}
}
