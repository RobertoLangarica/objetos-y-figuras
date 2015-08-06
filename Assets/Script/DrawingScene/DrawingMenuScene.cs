using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DrawingMenuScene : MonoBehaviour 
{
	void Start()
	{
		XMLLoader loader = GameObject.FindObjectOfType<XMLLoader>();
		GameObject go = null;
		GameObject panel = transform.GetChild(1).gameObject;
		bool first = false;

		foreach(DrawingImage val in loader.data.dImages)
		{
			string captured = val.name;
			if(!first)
			{
				first = true;
				go = transform.GetChild(0).gameObject;
			}
			else
			{
				go = GameObject.Instantiate(go) as GameObject;
			}
			Debug.Log(go);
			go.name = captured;
			go.transform.SetParent(panel.transform);
			go.GetComponent<Button>().onClick.RemoveAllListeners();
			go.GetComponent<Button>().onClick.AddListener(() => selectNewScene(captured+"_large"));
			go.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			go.GetComponent<RectTransform>().offsetMax = Vector2.zero;

			Texture2D texture = Resources.Load (captured+"_tiny") as Texture2D;

			Sprite tempSpt = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2 (0.5f, 0.5f));
			go.GetComponent<Image>().sprite = tempSpt;
		}
	}

	public void selectNewScene(string sceneToGo)
	{
		DrawingScene.shape = sceneToGo;
		ScreenManager.instance.GoToScene("DrawingScene");
	}
}