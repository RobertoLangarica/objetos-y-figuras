using UnityEngine;
using System.Collections;

public class DrawingMenuScene : MonoBehaviour 
{	
	public void selectNewScene(string sceneToGo)
	{
		DrawingScene.shape = sceneToGo;
		ScreenManager.instance.GoToScene("DrawingScene");
	}
}