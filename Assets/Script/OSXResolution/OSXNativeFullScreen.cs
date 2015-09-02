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
		Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);
		#endif
	}
	
}
