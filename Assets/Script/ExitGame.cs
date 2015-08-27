using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour 
{
	void Start () 
	{
		#if UNITY_STANDALONE
		#else
		gameObject.SetActive(false);
		#endif
	}

	public void quitGame()
	{
		Debug.Log("Algo");
		Application.Quit();
	}
}