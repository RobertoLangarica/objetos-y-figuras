using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public GameObject ships;

	public int shipsNumbers = 10;
	// Use this for initialization
	void Start () {
		/*GameObject[] ship;
		ship = new GameObject[shipsNumbers];
		float boundaryBottom = -Camera.main.orthographicSize;
		float boundaryLeft = -Camera.main.aspect * Camera.main.orthographicSize;
		Vector3 Pos;
		Vector3 randRot = new Vector3(0,0,(Random.Range(0,5)*15));

		//Vector3 size = ships.collider2D.bounds.size;
		//float x = (shipsNumbers/(Camera.main.aspect * Camera.main.orthographicSize)*15);

		for(int i =0; i<ship.Length; i++)
		{
			Pos = new Vector3((Random.Range(0,5)*15),0);
			ship[i] = ships;
			ship[i]= GameObject.Instantiate(ships,Pos,Quaternion.Euler(0,0,0))as GameObject;
		}*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
