using UnityEngine;
using System.Collections;

public class SoundShapeMenu : MonoBehaviour {
	
	public void Triangles()
	{
		SoundShapeManager.shapeToPrepare = "Triangle";
		ScreenManager.instance.GoToScene ("SoundShapeGameplay");
	}

	public void Squares()
	{
		SoundShapeManager.shapeToPrepare = "Squares";
		ScreenManager.instance.GoToScene ("SoundShapeGameplay");
	}

	public void Circles()
	{
		SoundShapeManager.shapeToPrepare = "Circles";
		ScreenManager.instance.GoToScene ("SoundShapeGameplay");
	}

	public void Rhomboids()
	{
		SoundShapeManager.shapeToPrepare = "Rhomboids";
		ScreenManager.instance.GoToScene ("SoundShapeGameplay");
	}
}
