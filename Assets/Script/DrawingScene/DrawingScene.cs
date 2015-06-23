using UnityEngine;
using System.Collections;

public class DrawingScene : MonoBehaviour 
{
	public static string shape = "1";

	public SpriteRenderer img;

	// Use this for initialization
	void Start () 
	{

		changeTexture(shape);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void onGoBack()
	{
		ScreenManager.instance.GoToScene("Conoce");
	}

	protected void changeTexture(string nName)
	{	
		Texture2D texture = Resources.Load (nName) as Texture2D;

		Debug.Log (texture);
		Sprite tempSpt = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2 (0.5f, 0.5f));
		
		img.sprite = tempSpt;
		
		img.sprite.name = "Fondo";
		img.material.mainTexture = texture as Texture;
		img.material.shader = Shader.Find ("Sprites/Default");
	}
}