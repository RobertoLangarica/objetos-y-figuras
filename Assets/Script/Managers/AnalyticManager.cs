using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;
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

	public void finishGame(string lvlPass)
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
	}

}


