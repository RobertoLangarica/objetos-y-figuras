using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	protected float randomSpeed;
	protected float boundaryTop;
	protected float boundaryBottom;
	// Use this for initialization
	void Start () {
		boundaryTop = Screen.height;
		boundaryBottom = -Screen.height;

		randomSpeed = Random.Range(0.1f,0.5f);
	}
	
	// Update is called once per frame
	void Update()
	{
		transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y+randomSpeed);
		checkBounds();
	}

	void checkBounds()
	{
		if(transform.localPosition.y> boundaryTop*0.5f)
		{
			transform.localPosition= new Vector2(transform.localPosition.x, boundaryBottom*(.5f));
		}
	}
}
