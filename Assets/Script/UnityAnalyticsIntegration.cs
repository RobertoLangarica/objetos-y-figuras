using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
		const string projectId = "d9141f30-6b38-42cd-81af-1a663d78cc00";
		UnityAnalytics.StartSDK (projectId);
		
	}
	
}