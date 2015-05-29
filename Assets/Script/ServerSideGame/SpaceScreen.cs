using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpaceScreen : MonoBehaviour {

	public GameObject start;
	public GameObject stop;
	public Text txt;

	protected Animator[] animators;
	protected Dictionary<string,RemotePlayer> players = new Dictionary<string, RemotePlayer>();
	
	protected int currentCount = 5;
	protected bool gameRunning = false;
	protected ServerManager server;

	void Start () {

		stop.SetActive(false);

		animators = GameObject.FindObjectsOfType<Animator>();

		//Detenemos los animators
		foreach(Animator a in animators)
		{
			a.enabled = false;
		}

		server = GameObject.FindObjectOfType<ServerManager>();

		server.onName	+= onPlayerName;
		server.onShip	+= onPlayerShip;
		server.onMove	+= onShipMove;
		server.onPhoto	+= onPlayerPhoto;

		ShipControler[] ships = GameObject.FindObjectsOfType<ShipControler>();
		int c = 0;
		foreach(ShipControler ship in ships)
		{
			RemotePlayer p = new RemotePlayer();
			p.pos = players.Count;
			p.ship = ship;
			p.ship.setPosition(p.pos);
			players.Add("_"+c++,p);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void initGame()
	{
		stop.SetActive(true);
		start.SetActive(false);

		gameRunning = true;

		foreach(Animator a in animators)
		{
			a.enabled = true;
		}

		//Iniciamos las naves
		ShipControler[] ships = GameObject.FindObjectsOfType<ShipControler>();
		foreach(ShipControler ship in ships)
		{
			ship.ignition();
		}
	}


	public void stopGame()
	{
		currentCount = 5;
		stop.SetActive(false);
		start.SetActive(true);

		gameRunning = false;

		//Limpiamos los jugadores
		foreach(RemotePlayer p in players.Values)
		{
			p.destroy();
		}
		players.Clear();

		//Detenemos los animators
		foreach(Animator a in animators)
		{
			a.enabled = false;
		}

		if(server)
		{
			server.stopSpaceMode();
		}
		ScreenManager.instance.GoToScene("StartMenuServer");
	}

	public bool existPlayer(string player)
	{
		RemotePlayer tmp = null;

		return players.TryGetValue(player,out tmp);
	}

	public void onPlayerName(string player,string name)
	{
		//Si ya comenzo el juego ya no se reciben datos
		if(gameRunning){return;}

		if(existPlayer(player))
		{
			players[player].playerName = name;
		}
		else
		{
			RemotePlayer p = new RemotePlayer();
			p.pos = players.Count;
			p.playerName = name;

			if(p.ship)
			{
				p.setPhotoOnShip();
				p.ship.setPosition(p.pos);
			}
			players.Add(player,p);
		}
	}

	public void onPlayerShip(string player,string ship)
	{
		//Si ya comenzo el juego ya no se reciben datos
		if(gameRunning){return;}

		if(existPlayer(player))
		{
			players[player].shipName = ship;
		}
		else
		{
			RemotePlayer p = new RemotePlayer();
			p.pos = players.Count;
			p.shipName = ship;
			
			players.Add(player,p);
		}
	}

	public void onPlayerPhoto(string player,string photo)
	{
		//Si ya comenzo el juego ya no se reciben datos
		if(gameRunning){return;}

		if(existPlayer(player))
		{
			players[player].photo = photo;
		}
		else
		{
			RemotePlayer p = new RemotePlayer();
			p.pos = players.Count;
			p.photo = photo;
			
			players.Add(player,p);
		}
	}

	public void onShipMove(string player,string direction)
	{
		if(existPlayer(player)&&players[player].ship)
		{
			players[player].ship.rotateDirection=direction;
		}
	}

	public void starCronometer()
	{
		gameRunning = true;
		StartCoroutine("tryingToSendInitSpace");
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
			initGame();
		}
	}

	IEnumerator tryingToSendInitSpace()
	{
		if (gameRunning) 
		{
			if(server)
			{
				server.initSpaceMode();
			}
			yield return new WaitForSeconds (2);
			StartCoroutine("tryingToSendInitSpace");
		}
	}
}