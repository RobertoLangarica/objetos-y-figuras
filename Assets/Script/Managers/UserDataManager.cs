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

		PlayerPrefs.SetInt("startGame",1);
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
		PlayerPrefs.SetInt("isAPirateGame",1);//Pirateado por default
	}

	public bool tutorMode
	{
		get{return PlayerPrefs.GetInt("tutorMode")==1;}
		set{PlayerPrefs.SetInt("tutorMode",value ? 1:0);}
	}

	public bool isAPirateGame
	{
		get
		{
			//En moviles no se permite el ser pirata
			bool pirateAllowed = false;

			#if UNITY_STANDALONE
			pirateAllowed = true;
			#endif

			if(pirateAllowed)
			{return PlayerPrefs.GetInt("isAPirateGame")==1;}

			return false;
		}
		set{PlayerPrefs.SetInt("isAPirateGame",value ? 1:0);}
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

