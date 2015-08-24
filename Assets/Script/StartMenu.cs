using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class StartMenu : MonoBehaviour
{
	public void onStart()
	{
		ScreenManager.instance.GoToScene("GameMenu");
	}
}
