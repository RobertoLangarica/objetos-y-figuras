using UnityEngine;
using System.Collections;

public class OrdenaMenu : MonoBehaviour {

	public void selectPatrones()
	{
		ScreenManager.instance.GoToScene("PatronScene");
	}
	
	public void selectOrdenaTamaños()
	{
		ScreenManager.instance.GoToScene("OrdenaTamañosScene");
	}
}
