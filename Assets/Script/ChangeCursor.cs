using UnityEngine;
using System.Collections;

public class ChangeCursor : MonoBehaviour {

	protected bool enter;
	protected bool click;
	public void changeCursor(string toChange)
	{
		switch (toChange) {
		case "over":
			CursorChanger.instance.overButton();
			break;
		case "down":
			CursorChanger.instance.downButton();
			break;
		case "exit":
			CursorChanger.instance.upChange();
			break;
		case "click":
			CursorChanger.instance.overButton();
			break;
		}
	}
}
