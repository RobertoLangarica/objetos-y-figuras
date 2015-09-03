using UnityEngine;
using System.Collections;

public class GameValidator : MonoBehaviour 
{

	void Awake()
	{
		if(UserDataManager.instance.isAPirateGame)
		{
			bool validate = true;
			#if UNITY_EDITOR
				validate = false;
			#endif

			if(validate)
			{
				if(ScreenManager.instance)
				{ScreenManager.instance.GoToScene("Validation");}
			}
		}
	}	
}
