using UnityEngine;
using System.Collections;

public class GameValidator : MonoBehaviour 
{

	void Awake()
	{
		if(UserDataManager.instance.isAPirateGame)
		{
			if(ScreenManager.instance)
			{ScreenManager.instance.GoToScene("Validation");}
		}
	}	
}
