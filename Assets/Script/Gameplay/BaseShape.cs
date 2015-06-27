using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseShape : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	
	public string sortingLayer
	{
		get{return spriteRenderer.sortingLayerName;}
		set{spriteRenderer.sortingLayerName = value;}
	}
	
	public int sortingOrder
	{
		get{return spriteRenderer.sortingOrder;}
		set{spriteRenderer.sortingOrder = value;}
	}
	
	public Color color
	{
		get{return spriteRenderer.color;}
		set{spriteRenderer.color = value;}
	}
}
