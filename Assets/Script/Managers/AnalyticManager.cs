using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class AnalyticManager : MonoBehaviour {

	public static AnalyticManager instance;
	protected float gameTime;
	// Use this for initialization


	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		gameTime += Time.deltaTime;
	}

	public void startGame()
	{
		gameTime =0;
	}

	/*public void finishGame(string lvlPass)
	{
		Debug.Log(lvlPass);
		Debug.Log(gameTime);

		UnityAnalytics.CustomEvent("ships", new Dictionary<string, object>
		                           {
			{ "Ship name", lvlPass },
		});
		UnityAnalytics.CustomEvent(lvlPass, new Dictionary<string, object>
		                           {
			{"Time: ", gameTime }
		});
	}*/

	public void finsh(string activityName,string exerciseType, string exerciseName, bool finish=true)
	{
		if(finish)
		{
			Debug.Log("S");
			Analytics.CustomEvent(activityName, new Dictionary<string, object>
			                           {
				{ "Finished "+exerciseType, exerciseName },
			});
			Analytics.CustomEvent(exerciseType, new Dictionary<string, object>
			                           {
				{ "Finish "+exerciseName, gameTime },
			});
			//Debug.Log("Termino: "+exerciseType+" time: "+gameTime);
		}
		else
		{
			Analytics.CustomEvent(activityName, new Dictionary<string, object>
			                           {
				{ "not Finished "+exerciseType, exerciseName },
			});
			Analytics.CustomEvent(exerciseType, new Dictionary<string, object>
			                           {
				{ "not Finish"+exerciseName, gameTime },
			});
			//Debug.Log("NO termino "+"time: "+gameTime);
		}

	}

	public void serialCodeSend(string serialKey)
	{
		Analytics.CustomEvent("SerialKey", new Dictionary<string, object>
		                           {
			{ "serialKey: ", serialKey },
		});
	}

	void OnApplicationQuit() {
		//Debug.Log("cerrando");
	}

}


