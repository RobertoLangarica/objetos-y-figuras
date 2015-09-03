using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;

public class VV_GameProtection : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		TextAsset asset = Resources.Load("OYFVV2015_KIV") as TextAsset;
		StreamReader reader = new StreamReader(asset.text);

		Stream s = new MemoryStream(asset.bytes);
		BinaryReader br = new BinaryReader(s);
	}
}