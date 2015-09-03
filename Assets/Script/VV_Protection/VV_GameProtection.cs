using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;

public class VV_GameProtection : MonoBehaviour 
{

	void Start () 
	{
		TextAsset asset = Resources.Load("OYFVV2015_KIV") as TextAsset;
		StreamReader reader = new StreamReader(asset.text);
	}
}