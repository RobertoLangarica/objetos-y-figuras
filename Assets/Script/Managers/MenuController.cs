using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

	protected GameObject menu;
	void Start()
	{
		//Cargamos los niveles desde aqui
		LevelManager.instance.getLevel("1");

		menu = GameObject.Find("Menu");

		Level[] levels = LevelManager.instance.getLevels(UserDataManager.instance.level);

		foreach(Level level in levels)
		{
			GameObject tmp = (GameObject)Resources.Load("Menu/"+level.name+"_menu");
			GameObject go = ((GameObject)GameObject.Instantiate(tmp));
			go.transform.SetParent(menu.transform);
			go.GetComponent<MenuItem>().lvlName = level.name;
		}

		GameObject lo = GameObject.Find("Level");
		lo.GetComponent<Text>().text = "Nivel "+UserDataManager.instance.level;
	}
}
