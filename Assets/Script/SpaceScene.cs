using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpaceScene : MonoBehaviour 
{	
	[HideInInspector]
	public List<GameObject> allShips = new List<GameObject>();
	public Image img;

	protected int currentCount = 3;
	protected GridLayoutGroup shipContainer;
	protected List<GameObject> spaceObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void createObjects(int amount,string prefabName)
	{
		GameObject go;

		Vector3 randPos = new Vector3(Random.Range(0,Screen.width),Random.Range(0,Screen.height),0);
		Vector3 randRot = new Vector3(0,0,Random.Range(0,180));

		for(int i = 0;i < amount;i++)
		{
			GameObject prefab = (GameObject)Resources.Load(prefabName);
			go = GameObject.Instantiate(prefab,randPos,Quaternion.Euler(randRot)) as GameObject;
			go.GetComponent<ShipControler>().ignition();
			spaceObjects.Add(go);
		}
	}

	public void addNewShip(GameObject go)
	{
		allShips.Add(go);
		go.transform.parent = shipContainer.transform;
	}

	public void starCronometer()
	{
		StartCoroutine("cronometerCount");
	}

	IEnumerator cronometerCount()
	{
		yield return new WaitForSeconds(2);
		if(currentCount > 0)
		{
			img.enabled = true;
			img.sprite = Resources.Load(currentCount.ToString(),typeof(Sprite)) as Sprite;
			StartCoroutine("cronometerCount");
			currentCount--;
		}
		else
		{
			img.enabled = false;
			for(int i = 0; i< allShips.Count;i++)
			{
				allShips[i].GetComponent<ShipControler>().ignition();
			}
		}
	}
}
