using System;
using UnityEngine;
using System.Collections;
using SimpleJSON;

public class SerialValidator : MonoBehaviour 
{
	public Action OnSerialActivated;
	public Action OnSerialActivationFailed;//locallyFailed
	public Action OnSerialActivationOutOfReach;

	public string API_ROOT = "http://api.curiosamente.com/";
	public bool cleanDataAtStart = false; 

	public static SerialValidator instance;

	void Awake()
	{
		if(cleanDataAtStart)
		{
			UserDataManager.instance.cleanData();	
		}

		#if UNITY_STANDALONE
		instance = this;
		DontDestroyOnLoad(this);
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

		serialToSave = serialToSave.ToUpperInvariant();
		//Para ser post al parecer necesita datos
		WWWForm form = new WWWForm();
		form.AddField("data",serialToSave);

		WWW www = new WWW (API_ROOT+"serial/installed/"+serialToSave,form);
		StartCoroutine (WaitForSave (www));
	}

	public void activateSerial(string serialToActivate)
	{
		Debug.Log("Llamado a la API para activar el serial.");

		serialToActivate = serialToActivate.ToUpperInvariant();
		//Para ser post al parecer necesita datos
		WWWForm form = new WWWForm();
		form.AddField("data",serialToActivate);

		WWW www = new WWW (API_ROOT+"serial/activate/"+serialToActivate,form);
		StartCoroutine (WaitForActivation (www));
	}


	private IEnumerator WaitForActivation(WWW www) 
	{
		yield return www;

		//error?
		if (www.error == null) 
		{
			//Sin error
			Debug.Log (www.text);
			onSerialActivationAPIResponse(SimpleJSON.JSON.Parse(www.text));
		}
		else 
		{
			Debug.Log (www.error);
			if(OnSerialActivationOutOfReach != null)
			{
				OnSerialActivationOutOfReach();
			}
		}
	}

	void onSerialActivationAPIResponse(JSONNode result)
	{
		/*
		 * TRUE /FALSE
		 * */

		string data;
		data = result["data"].ToString();

		if(result["data"].AsBool)
		{
			//Se valido exitosamente
			if(OnSerialActivated != null)
			{
				OnSerialActivated();
			}
		}
		else
		{
			//Es invalido el serial
			if(OnSerialActivationFailed != null)
			{
				OnSerialActivationFailed();
			}
		}




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
