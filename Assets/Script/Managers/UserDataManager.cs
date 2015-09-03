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
		PlayerPrefs.SetInt("startGame",1);
		//PlayerPrefs.SetInt("piracyPopUp",1);

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
		int tutor = 0;

		#if UNITY_STANDALONE || UNITY_EDITOR
		//Por default activo en stand alone
		tutor = 1;
		#endif

		PlayerPrefs.SetInt("tutorMode",tutor);
		PlayerPrefs.SetInt("piracyPopUp",1);
		PlayerPrefs.SetInt("validateGame",0);
	}

	public bool tutorMode
	{
		get{return PlayerPrefs.GetInt("tutorMode")==1;}
		set{PlayerPrefs.SetInt("tutorMode",value ? 1:0);}
	}

	public bool showPopUp
	{
		get{return PlayerPrefs.GetInt("piracyPopUp")==1;}
		set{PlayerPrefs.SetInt("piracyPopUp",value ? 1:0);}
	}

	public bool startGame
	{
		get{return PlayerPrefs.GetInt("startGame")==1;}
		set{PlayerPrefs.SetInt("startGame",value ? 1:0);}
	}
	
	public bool validateGame
	{
		get{return PlayerPrefs.GetInt("validateGame")==1;}
		set{PlayerPrefs.SetInt("validateGame",value ? 1:0);}
	}

	protected void resolveVersion()
	{
		PlayerPrefs.SetInt("version",version);
	}

	public void cleanData()
	{
		Debug.Log("UM->Cleaning previous data.");
		PlayerPrefs.DeleteAll();
		createDefaultData();
	}

	public void foo(){}
}

