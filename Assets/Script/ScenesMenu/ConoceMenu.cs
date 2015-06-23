using UnityEngine;
using System.Collections;

public class ConoceMenu : MonoBehaviour {

	public void selectDraw()
	{
		ScreenManager.instance.GoToScene("DrawingScene");
	}
	
	public void selectSoundShapes()
	{
		ScreenManager.instance.GoToScene("SoundShapeScene");
	}
}
