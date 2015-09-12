using UnityEngine;
using System.Collections;

public class ConstruyeMenu : MonoBehaviour {

	void Start()
	{
		MenuController.currLevel=0;
	}
	public void selectSandBox()
	{
		ScreenManager.instance.GoToScene("SandBoxScene");
	}
	
	public void selectTangram()
	{
		TangramManager.tType = ETangramTypes.SAME_SHAPE;
		ScreenManager.instance.GoToScene("TangramScene");
	}
	
	public void selectAllPieces()
	{
		TangramManager.tType = ETangramTypes.ALL_SHAPES;
		ScreenManager.instance.GoToScene("TangramScene");
	}
	public void selectSpacegram()
	{
		ScreenManager.instance.GoToScene("SpacegramMenu");
	}
}
