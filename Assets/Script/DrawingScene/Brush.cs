using UnityEngine;
using System.Collections;

public class Brush : MonoBehaviour 
{	
	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.transform.name == "Erraser") 
		{
			GameObject.Find("DrawTool").GetComponent<DrawingInput>().erraseThisBrush(gameObject);
		}
	}
}
