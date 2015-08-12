using UnityEngine;
using System.Collections;

public class BackgroundStars : MonoBehaviour {

	protected GameObject star;
	protected GameObject starCarpet;
	public int howManyStars = 1;
	// Use this for initialization
	void Start () {

		star = (GameObject)Resources.Load("Star");
		starCarpet = GameObject.Find("starCarpet");
		showRandomStars();
	}

	protected void showRandomStars()
	{
		Vector2 randomPos = new Vector2(Random.Range(0f,1f),(Random.Range(0f,1f)));
		for(int i=0; i<howManyStars; i++)
		{
			randomPos = new Vector2(Random.Range(0f,1f),(Random.Range(0f,1f)));
			GameObject go;
			go = (GameObject)GameObject.Instantiate(star);
			go.transform.parent = starCarpet.transform;
		
			go.GetComponent<RectTransform>().anchorMax = new Vector2(randomPos.x+0.03f,randomPos.y+0.04f);
			go.GetComponent<RectTransform>().anchorMin = new Vector2(randomPos.x,randomPos.y);
			go.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
			go.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
			go.transform.localScale = new Vector3(1,1,1);
		}
	}
}
