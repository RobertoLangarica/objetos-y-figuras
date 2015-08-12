using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	protected float randomSpeed;
	protected float boundaryTop;
	protected float boundaryBottom;
	protected float boundaryLeft;
	protected float boundaryRight;
	// Use this for initialization
	void Start () {
		boundaryTop = Screen.height;
		boundaryBottom = -Screen.height;
		boundaryLeft = -Camera.main.aspect * Camera.main.orthographicSize;
		boundaryRight = Camera.main.aspect * Camera.main.orthographicSize;

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
		Debug.Log(boundaryBottom);
		if(transform.localPosition.y> boundaryTop*0.5f)
		{
			Debug.Log("S");
			transform.localPosition= new Vector2(transform.localPosition.x, boundaryBottom*(.5f));
		}
	}
}
