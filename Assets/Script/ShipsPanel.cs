using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ShipsPanel : MonoBehaviour 
{
	public GameObject container;

	void Start()
	{
		refresh();
	}

	public void refresh()
	{
		string[] myShips = UserDataManager.instance.getCompletedLevels();
		
		if(myShips != null)
		{
			for(int i = 0;i < myShips.Length;i++)
			{
				if(container.transform.FindChild(myShips[i]+"_menu(Clone)") == null)
				{
					GameObject prefab = (GameObject)Resources.Load("Menu/"+myShips[i]+"_menu");
					GameObject ship = GameObject.Instantiate(prefab) as GameObject;
					ship.transform.SetParent(container.transform);
					ship.GetComponent<MenuItem>().lvlName = myShips[i];
					ship.transform.localScale = new Vector3(2,2,2);
				}
				Debug.Log("ship:" + myShips[i]);
			}
		}
	}
}