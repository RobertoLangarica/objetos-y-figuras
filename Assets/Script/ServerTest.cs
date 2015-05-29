using UnityEngine;
using System.Collections;

public class ServerTest : MonoBehaviour {

	public void changeScene(string scene)
	{
		Application.LoadLevel(scene);
	}
}
