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
		PlayerPrefs.SetInt("TermsAccepted",1);

		PlayerPrefs.SetString("serial","");
		PlayerPrefs.SetInt("serialCount",0);

		PlayerPrefs.SetInt("blockedSerialCount",0);
	}

	public void saveSerialNumber(string value)
	{
		value = value.ToUpperInvariant();
		int index = PlayerPrefs.GetInt("serialCount");
		index++;
		PlayerPrefs.SetString("serial"+index.ToString(),value);
		PlayerPrefs.SetInt("serialCount",index);
	}

	public string[] getAllPreviousValidatedSerials()
	{
		string[] result;
		int index = PlayerPrefs.GetInt("serialCount");
		result = new string[index];

		for(int i = 1; i <= index; i++)
		{
			result[i-1] = PlayerPrefs.GetString("serial"+i.ToString());
		}

		return result;
	}

	public void saveBlockedSerialNumber(string value)
	{
		if(isPreviouslyBlocked(value))
		{
			//evitamos clones
			return;	
		}

		value = value.ToUpperInvariant();
		int index = PlayerPrefs.GetInt("blockedSerialCount");
		index++;
		PlayerPrefs.SetString("blockedSerial"+index.ToString(),value);
		PlayerPrefs.SetInt("blockedSerialCount",index);
	}

	public bool isPreviouslyBlocked(string value)
	{
		int index = PlayerPrefs.GetInt("blockedSerialCount");
		string tmp;
		value = value.ToUpperInvariant();

		for(int i = 1; i <= index; i++)
		{
			tmp = PlayerPrefs.GetString("blockedSerial"+i.ToString());

			if(tmp.Equals(value))
			{
				return true;
			}
		}

		return false;
	}

	public string currentSerial
	{
		get{return PlayerPrefs.GetString("serial");}
		set{PlayerPrefs.SetString("serial",value);}
	}

	public bool tutorMode
	{
		get{return PlayerPrefs.GetInt("tutorMode")==1;}
		set{PlayerPrefs.SetInt("tutorMode",value ? 1:0);}
	}

	public bool TermsAccepted
	{
		get{return PlayerPrefs.GetInt("TermsAccepted")==1;}
		set{PlayerPrefs.SetInt("TermsAccepted",value ? 1:0);}
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

