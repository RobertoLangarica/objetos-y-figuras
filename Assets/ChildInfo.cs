using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ChildInfo : MonoBehaviour {

	protected GameObject findChildInfo;
	public static string PHOTO_PATH;
	public static string PHOTO_STORAGE_PATH;
	// Use this for initialization
	void Start () {
		findChildInfo = GameObject.Find("ChildInfo");
		PHOTO_PATH = Path.Combine(Application.persistentDataPath,"Pictures/Picture.jpg");
		PHOTO_STORAGE_PATH = Path.Combine(Application.persistentDataPath,"Pictures/");

		LoadImage();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadImage()
	{
		byte[] bytes;
		Texture2D myPic;
		bytes = System.IO.File.ReadAllBytes (PHOTO_PATH);
		myPic = new Texture2D(1,2);
		myPic.LoadImage(bytes);
		Sprite s = Sprite.Create(myPic,new Rect(0,0,myPic.width,myPic.height),new Vector2(0.5f,0.5f));
		findChildInfo.transform.FindChild("Picture").FindChild("RawImage").GetComponent<Image>().sprite = s;
	}
}
