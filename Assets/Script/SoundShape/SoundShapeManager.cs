using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundShapeManager : MonoBehaviour {

	public static string shapeToPrepare = "Triangle";
	public GameObject content;
	public GameObject[] shapes;

	protected int pieces = 4;
	
	protected int divs = 9;
	protected int divsX = 3;
	protected int divsY = 3;
	// Use this for initialization
	void Start () {

		initializeShapes();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void initializeShapes()
	{
		GameObject go;
		shapes = new GameObject[pieces];
		float min;
		float max;
		Vector2[] pos = new Vector2[divs];
		for(int j = 1, l =0;j < divsX+1;j++,l++)
		{
			for(int k = 1;k < divsY+1;k++)
			{
				pos[l] = new Vector2((Camera.main.aspect * Camera.main.orthographicSize)/(divsX*j),(Camera.main.orthographicSize)/(divsY*k));
			}

		}
		for(int i = 0; i < pieces; i++)
		{

			GameObject shape = (GameObject)Resources.Load("SoundShapes/"+shapeToPrepare+i);
			Debug.Log(pos[i] + "jnk");
			go = GameObject.Instantiate(shape) as GameObject;
			//shapes[i] = go;		
			go.transform.SetParent(content.transform);
			//go.transform.localPosition = new Vector3 (go.transform.localPosition.x,go.transform.localPosition.y,0);
		}
	}

}
