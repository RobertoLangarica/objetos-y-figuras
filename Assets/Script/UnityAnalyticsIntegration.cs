using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
		const string projectId = "043dc45a-1cfb-4669-83f7-4f0a00254c70";
		UnityAnalytics.StartSDK (projectId);
		
	}
	
}