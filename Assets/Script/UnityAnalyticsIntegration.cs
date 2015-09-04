using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
		const string projectId = "38870289-80d2-4270-800f-c1b4e2841ffd";
		UnityAnalytics.StartSDK (projectId);
		
	}
	
}