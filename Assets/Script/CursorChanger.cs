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
	public Texture2D pencilTexture;
	public Texture2D ereaserTexture;

	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	public bool bPencil;

	
	EventTrigger eventTrigger = null;

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

	protected void searchButtons()
	{
		GameObject[] buttons;
		buttons = GameObject.FindGameObjectsWithTag("UIButton");

		foreach(GameObject button in buttons)
		{
			eventTrigger = button.AddComponent<EventTrigger>();
			//button.GetComponent<Button>().OnPointerEnter.AddListener(() => { overButton(); });
			//button.GetComponent<Button>().OnPointerExit.AddListener(() => { upChange(); }); 
			//button.GetComponent<Button>().OnPointerDown.AddListener(() => { downButton(); }); 
			//button.GetComponent<Button>().onClick.AddListener(() => { overButton(); }); 
			eventTrigger = button.GetComponent<EventTrigger>();

			Debug.Log(eventTrigger);

			AddEventTrigger(ereaser, EventTriggerType.PointerEnter);
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
		searchButtons();
	}
}
