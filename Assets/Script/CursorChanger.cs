using UnityEngine;
using System.Collections;

public class CursorChanger : MonoBehaviour {
	public static CursorChanger instance;
	public Texture2D overButtonTexture;
	public Texture2D downButtonTexture;
	public Texture2D overDragTexture;
	public Texture2D downDragTexture;
	public Texture2D pencilTexture;
	public Texture2D ereaserTexture;

	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	public bool bPencil;
	void Awake()
	{
		instance = this;
	}
	
	public void upChange() 
	{
		Debug.Log("up");
		changer(null);
	}

	public void overButton() 
	{
		Debug.Log("over");
		changer(overButtonTexture);
	}

	public void downButton()
	{
		Debug.Log("down");
		changer(downButtonTexture);
	}

	public void overDrag()
	{
		Debug.Log("overDrag");
		changer(overDragTexture);
	}

	public void downDrag()
	{
		Debug.Log("upDrag");
		changer(downDragTexture);
	}

	public void pencil()
	{
		Debug.Log("pencil");
		changer(pencilTexture);
	}
	
	public void ereaser()
	{
		Debug.Log("ereaser");
		changer(ereaserTexture);
	}

	protected void changer(Texture2D texture)
	{
		if(!bPencil)
		{
			//Cursor.SetCursor(texture, hotSpot, cursorMode);
		}
	}
}
