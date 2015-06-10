using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

	public GameObject content;

	void Start()
	{
		//Cargamos los niveles desde aqui
		LevelManager.instance.getLevel("1");


		//Todos los niveles
		Level[] levels = LevelManager.instance.getAllLevels();

		foreach(Level level in levels)
		{
			GameObject tmp = (GameObject)Resources.Load("Menu/"+level.name+"_menu");
			GameObject go = ((GameObject)GameObject.Instantiate(tmp));
			go.transform.SetParent(content.transform);
			go.GetComponent<MenuItem>().lvlName = level.name;
			go.GetComponent<LayoutElement>().minWidth = Screen.width/3.0f; 
		}

		GameObject lo = GameObject.Find("Level");
		lo.GetComponent<Text>().text = "Nivel "+UserDataManager.instance.level;
	}
}
