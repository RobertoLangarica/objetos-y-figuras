using UnityEngine;
using System.Collections;

public class ShipTravelController : MonoBehaviour 
{
	public RectTransform container;

	public static string shipName = "SN01_01";

	protected GameObject ship;
	protected ClientManager client;
	
	void Start () 
	{
		client = GameObject.FindObjectOfType<ClientManager>();
		GameObject tmp = (GameObject)Resources.Load("SpaceClient/"+shipName+"_client");
		
		ship = GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity) as GameObject;

		ship.transform.SetParent(container,false);
	}

	public void moveShip(string direction)
	{
		if(client)
		{
			client.moveShip(direction);
		}
	}
}