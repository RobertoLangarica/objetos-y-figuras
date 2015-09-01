using UnityEngine;
using System.Collections;
using System;

public class OSXNativeFullScreen : MonoBehaviour 
{
	void Awake() 
	{
		#if UNITY_STANDALONE_OSX
		Resolution[] resolutions = Screen.resolutions;
		Array.Reverse(resolutions);

		foreach (Resolution res in resolutions) 
		{
			print(res.width + "x" + res.height);
		}

		Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);
		Debug.Log("Full Screen Resolution set for UNITY_STANDALONE_OSX");
		#endif
	}
	
}
