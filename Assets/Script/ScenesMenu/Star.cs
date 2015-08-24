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

		transform.localScale = transform.localScale*Random.Range(1,2.0f);

		this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x,this.transform.eulerAngles.y,Random.Range(0,360));
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
