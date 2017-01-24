using UnityEngine;
using System.Collections;

public class GameValidator : MonoBehaviour 
{
	public bool SKIP_PIRATE_VALIDATION = false;
	public bool cleanDataAtStart = false; 

	void Start()
	{
		if(cleanDataAtStart)
		{
			UserDataManager.instance.cleanData();	
		}

		if(!SKIP_PIRATE_VALIDATION)
		{
			if(UserDataManager.instance.isAPirateGame)
			{
				if(!SKIP_PIRATE_VALIDATION)
				{
					if(ScreenManager.instance)
					{ScreenManager.instance.GoToScene("Validation");}
				}
			}
			else
			{
				//Ya no es pirata asi que checamos si sigue activo el numero de serie
				SerialBlocker.instance.askIsTheSerialIsBlocked(UserDataManager.instance.currentSerial);
			}
		}
		else
		{
			Debug.LogFormat("<color=RED>SALTANDO VALIDACIÓN</color>");
		}
	}	
}
