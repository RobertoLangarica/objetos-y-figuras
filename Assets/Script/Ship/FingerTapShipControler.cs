using UnityEngine;
using System.Collections;

public class FingerTapShipControler : MonoBehaviour {

	private TapRecognizer tR;
	// Use this for initialization
	void Start () {
		tR =  GetComponent<TapRecognizer>();
		tR.enabled = false;
		//Debug.Log(ShipControler.instance.startShip);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyUp(KeyCode.S))
		{
			tR.enabled = true;
		}
	}

	void OnTap( TapGesture gesture ) 
	{ 
		//if(ShipControler.instance.startShip)
		{
			if(gesture.Position.x+1>Screen.width*.5)
			{
				Debug.Log( "Tap gesture Right" );
				//ShipControler.instance.rotation(true);
			}
			if(gesture.Position.x <Screen.width*.5)
			{
				Debug.Log( "Tap gesture Left" );
				//ShipControler.instance.rotation(false);
			}
		}
	}
}
