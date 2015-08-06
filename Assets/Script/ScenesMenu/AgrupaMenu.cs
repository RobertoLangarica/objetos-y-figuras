using UnityEngine;
using System.Collections;

public class AgrupaMenu : MonoBehaviour 
{
	public void selectGroupType(string type)
	{
		switch(type)
		{
		case("color"):
		{GroupScene.typeOfGroup = EGroups.COLOR;}break;
		case("shape"):
		{GroupScene.typeOfGroup = EGroups.SHAPE;}break;
		case("size"):
		{GroupScene.typeOfGroup = EGroups.SIZE;}break;
		case("free"):
		{GroupScene.typeOfGroup = EGroups.FREE;}break;
		}
		ScreenManager.instance.GoToScene("GroupsScene");
	}
}
