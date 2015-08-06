using UnityEngine;
using System.Collections;

public class XMLLoader : MonoBehaviour
{
	public Levels data;

	void Awake()
	{
		TextAsset tempTxt = (TextAsset)Resources.Load ("Levels/levels");

		data = Levels.LoadFromText(tempTxt.text);
	}
}
