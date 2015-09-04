using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class CursorChanger : MonoBehaviour {
	public static CursorChanger instance;
	public Texture2D overButtonTexture;
	public Texture2D downButtonTexture;
	public Texture2D overDragTexture;
	public Texture2D downDragTexture;
	public Texture2D rotateTexture;
	public Texture2D pencilTexture;
	public Texture2D ereaserTexture;

	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	public bool bPencil;
	
	protected string currentState = "";
	
	EventTrigger eventTrigger = null;

	void Awake()
	{
		instance = this;
	}
	
	public void upChange() 
	{
		changer(null,"up");
	}

	public void overButton() 
	{
		changer(overButtonTexture,"over");
	}

	public void downButton()
	{
		changer(downButtonTexture,"down");
	}

	public void overDrag()
	{
		changer(overDragTexture,"overDrag");
	}

	public void downDrag()
	{
		changer(downDragTexture,"Drag texture");
	}

	public void rotate()
	{
		changer(rotateTexture,"rotate");
	}

	public void pencil()
	{
		hotSpot = new Vector2(0,pencilTexture.height*0.98f);
		changer(pencilTexture,"pencil");
	}
	
	public void ereaser()
	{
		hotSpot = new Vector2(ereaserTexture.width*0.5f,ereaserTexture.height*0.5f);
		changer(ereaserTexture,"ereaser");
	}

	/*void Update()
	{
		if(bPencil)
		{
			hotSpot = new Vector2(0,pencilTexture.height);
			Cursor.SetCursor(pencilTexture, Vector2.zero,CursorMode.Auto);
		}
	}*/

	protected void changer(Texture2D texture,string state)
	{
		if(!bPencil)
		{
			if(!currentState.Equals(state))
			{
				Debug.Log ("Cmbiando a estado: "+state);
				currentState = state;
				Cursor.SetCursor(texture, hotSpot, cursorMode);
				hotSpot = Vector2.zero;
			}
		}
	}

	protected void searchButtons()
	{
		GameObject[] buttons;
		GameObject[] piecesConoce;
		buttons = GameObject.FindGameObjectsWithTag("UIButton");
		piecesConoce = GameObject.FindGameObjectsWithTag("MenuShape");
		foreach(GameObject button in buttons)
		{
			//eventTrigger = button.AddComponent<EventTrigger>();
			eventTrigger = button.GetComponent<EventTrigger>();

			//Debug.Log(button);

			AddEventTrigger(overButton, EventTriggerType.PointerEnter);
			AddEventTrigger(upChange, EventTriggerType.PointerExit);
			AddEventTrigger(downButton, EventTriggerType.PointerDown);
			AddEventTrigger(overButton, EventTriggerType.PointerClick);
		}
		foreach(GameObject pieces in piecesConoce)
		{
			//eventTrigger = pieces.AddComponent<EventTrigger>();
			eventTrigger = pieces.GetComponent<EventTrigger>();
			
			//Debug.Log(pieces);
			
			AddEventTrigger(overButton, EventTriggerType.PointerEnter);
			AddEventTrigger(upChange, EventTriggerType.PointerExit);
			AddEventTrigger(downButton, EventTriggerType.PointerDown);
			AddEventTrigger(overButton, EventTriggerType.PointerClick);
		}
	}

	private void AddEventTrigger(UnityAction action, EventTriggerType triggerType)
	{
		// Create a nee TriggerEvent and add a listener
		EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();

		trigger.AddListener((eventData) => action()); // you can capture and pass the event data to the listener

		// Create and initialise EventTrigger.Entry using the created TriggerEvent
		EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

		// Add the EventTrigger.Entry to delegates list on the EventTrigger
		eventTrigger.delegates.Add(entry);
	}

	void OnLevelWasLoaded()
	{
		bPencil=false;
		changer(null,"up");
		StartCoroutine("waiting");
	}

	IEnumerator waiting()
	{
		yield return new WaitForSeconds(.1f);
		searchButtons();
	}
}
