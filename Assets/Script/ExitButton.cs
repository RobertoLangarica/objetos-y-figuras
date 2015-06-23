using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour 
{
	void OnTap( TapGesture gesture ) 
	{
		Debug.Log(gesture.Selection);
		Debug.Log(this.transform.name);
		if( gesture.Selection.name == this.transform.name)
		{
			Debug.Log ("Going to previous");
			ScreenManager.instance.showPrevScene();
		}
	}

	public void tap()
	{
		Debug.Log ("Going to previous");
		ScreenManager.instance.showPrevScene();
	}

	public void back()
	{
		Debug.Log("2");
		ScreenManager.instance.GoToScene("MainMenu");
	}
}
