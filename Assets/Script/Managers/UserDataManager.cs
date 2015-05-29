using UnityEngine;
using System.Collections;

public class UserDataManager 
{
	protected static UserDataManager _instance;

	//Version para controlar cambios a los datos en el futuro
	public const int version = 0;

	public static UserDataManager instance 
	{
		get {
			if(null == _instance)
			{
				_instance = new UserDataManager();
			}

			return _instance;
		}
	}

	UserDataManager()
	{
		if(PlayerPrefs.HasKey("version"))
		{
			if(PlayerPrefs.GetInt("version") != version)
			{
				//Resolvemos la version
				resolveVersion();
			}
		}
		else
		{
			createDefaultData();
		}
	}

	protected void createDefaultData()
	{
		PlayerPrefs.SetInt("version",version);
		PlayerPrefs.SetInt("level",0);
		PlayerPrefs.SetString("levels","");
		PlayerPrefs.SetString("name","");
		PlayerPrefs.SetString("shipSelected","");

	}

	protected void resolveVersion()
	{
		PlayerPrefs.SetInt("version",version);
	}

	public string name
	{
		get{return PlayerPrefs.GetString("name");}
		set{PlayerPrefs.SetString("name",value);}
	}

	public int level
	{
		get{return PlayerPrefs.GetInt("level");}
		set{PlayerPrefs.SetInt("level",value);}
	}

	public void cleanData()
	{
		Debug.Log("UM->Cleaning previous data.");
		PlayerPrefs.DeleteAll();
		createDefaultData();
	}

	public string shipSelected
	{
		get{return PlayerPrefs.GetString("shipSelected");}
		set{PlayerPrefs.SetString("shipSelected",value);}
	}

	public bool isLevelComplete(string levelName)
	{
		return PlayerPrefs.HasKey(levelName);
	}

	public void markLevelAsComplete(string levelName)
	{
		//Guardamos una cadena para siempre tener referencia a los niveles pasados
		string levels = PlayerPrefs.GetString("levels");

		if(levels == "")
		{
			levels = levelName;
		}
		else
		{
			levels = levels+","+levelName;
		}

		PlayerPrefs.SetString("levels",levels);
		PlayerPrefs.SetInt(levelName,1);
	}

	public string[] getCompletedLevels()
	{
		string levels = PlayerPrefs.GetString("levels");

		if(levels == "")
		{
			return null;
		}
		return levels.Split(new char[1]{','});
	}
}

