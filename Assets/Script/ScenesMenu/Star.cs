using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	protected float randomSpeed;
	protected float boundaryTop;
	protected float boundaryBottom;
	protected Vector2 newPos;

	// Use this for initialization
	void Start () {
		boundaryTop = Screen.height*0.5f;
		boundaryBottom = -Screen.height*0.5f;

		randomSpeed = Random.Range(0.1f,0.5f);

		transform.localScale = transform.localScale*Random.Range(1,2.0f);

		this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x,this.transform.eulerAngles.y,Random.Range(0,360));
		newPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update()
	{
		//No es necesario que sea exacto asi que hacemos o la suma o el igual
		if(newPos.y > boundaryTop)
		{
			newPos.y = boundaryBottom;
		}
		else
		{
			newPos.y += randomSpeed;
		}

		transform.localPosition = newPos;
	}
}
