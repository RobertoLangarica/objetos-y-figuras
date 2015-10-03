using UnityEngine;
using System.Collections;
using SimpleJSON;

public class SerialBlocker : MonoBehaviour 
{
	public string API_ROOT = "http://api.curiosamente.com/";

	public static SerialBlocker instance;

	void Awake()
	{
		#if UNITY_STANDALONE
		instance = this;
		#endif
	}

	void Start () 
	{
		#if UNITY_STANDALONE
		DontDestroyOnLoad(this);

		//Validamos si el serial activo no esta bloqueado
		if(!UserDataManager.instance.isAPirateGame)
		{
			//Ya no es pirata y tiene por lo tanto un numero de serie validado en local
			askIsTheSerialIsBlocked(UserDataManager.instance.currentSerial);
		}
		#endif
	}

	public void askIsTheSerialIsBlocked(string serialToValidate)
	{
		Debug.Log("Validando si esta bloqueado el numero de serie");

		serialToValidate = serialToValidate.ToUpper();
		WWW www = new WWW (API_ROOT+"serial/blocked/"+serialToValidate);
		StartCoroutine (WaitForRequest (www));
	}

	public void saveSerialAsInstalled(string serialToSave)
	{
		Debug.Log("Llamado a la API para guardar el serial como instalado.");

		serialToSave = serialToSave.ToUpper();
		//Para ser post al parecer necesita datos
		WWWForm form = new WWWForm();
		form.AddField("data",serialToSave);

		WWW www = new WWW (API_ROOT+"serial/installed/"+serialToSave,form);
		StartCoroutine (WaitForSave (www));
	}

	void onSerialBlockedAPIResponse(JSONNode result)
	{
		/*
		 * Y	El numero de serie esta bloqueado
		 * N	El numero de serie no esta bloqueado
		 * U	El numero de serie no se encuentra
		 * */

		string data;
		data = result["data"].ToString();
		Debug.Log(data);

		if(result["success"].AsBool)
		{
			if(data.Equals("\"Y\""))
			{
				Debug.Log("Serial bloqueado");
				//El bloqueado es el activo?
				//if(result.serial == UserDataManager.instance.currentSerial)
				//{
					UserDataManager.instance.saveBlockedSerialNumber(UserDataManager.instance.currentSerial);
					UserDataManager.instance.isAPirateGame = true;
					
					if(ScreenManager.instance)
					{
						//Evitamos el regreso de pantallas
						ScreenManager.instance.backAllowed = false;
						ScreenManager.instance.GoToSceneDelayed("Blocked",5);
					}
				/*}
				else
				{
					askIsTheSerialIsBlocked(UserDataManager.instance.currentSerial);
				}*/
			}
		}
	}

	private IEnumerator WaitForRequest(WWW www) 
	{
		yield return www;

		//Hubo error?
		if (www.error == null) 
		{
			Debug.Log (www.text);
			onSerialBlockedAPIResponse(SimpleJSON.JSON.Parse(www.text));
		}
		else 
		{
			Debug.Log (www.error);
		}
	}

	private IEnumerator WaitForSave(WWW www) 
	{
		yield return www;
		
		//Hubo error?
		if (www.error == null) 
		{
			Debug.Log ("Respuesta API al guardar el serial como instalado:");
			Debug.Log (www.text);
		}
		else 
		{
			Debug.Log (www.error);
		}
	}

}
