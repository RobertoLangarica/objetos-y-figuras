using UnityEngine;
using System.Collections;
using Soomla.Store;

public class MenuItems : MonoBehaviour {

	public string lvlName;
	public string lvlPurchseID;

	public void onClick()
	{
		if (lvlPurchseID == "spacegramShips062015" && UserDataManager.instance.premiumVersion != "premiumVersion") 
		{
			StoreInventory.BuyItem("premium_ver_ID");
			return;
		}
		SpacegramManager.lvlToPrepare = lvlName;
		ScreenManager.instance.GoToScene ("Spacegram");
	}
}
