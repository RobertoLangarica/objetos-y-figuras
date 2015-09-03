using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;

public class VV_GameProtection : MonoBehaviour 
{

	void Start () 
	{
		validateSerial("BE94-1164-D2AD-90A1-7B6B-B200-C46C-9A0A");
		validateSerial("2946-B4FA-3864-CC00-14FF-0FDF-6BB8-D67B");
	}

	public void validateSerial(string serial)
	{
		string decrypted = Decrypt(serial);

		Debug.Log(decrypted);
	}

	protected string Decrypt(string text)
	{
		//Nos traemos la key e IV
		TextAsset asset = Resources.Load("KIV/OYFVV2015_KIV") as TextAsset;
		string[] lines = asset.text.Split("\n"[0]);

		//Leemos key e IV
		RijndaelManaged rm = new RijndaelManaged();
		rm.Key = Convert.FromBase64String(lines[0]);
		rm.IV = Convert.FromBase64String(lines[1]);
		
		byte[] textBytes = StringToByteArrayFastest(text.Replace("-",""));
		byte[] plainTextBytes = new byte[textBytes.Length];
		
		//Descifrado
		//Stream con la informacion cifrada
		MemoryStream msDecrypt = new MemoryStream(textBytes);
		// Crear un flujo de descifrado basado en el flujo de los datos
		CryptoStream csDecrypt = new CryptoStream(msDecrypt, rm.CreateDecryptor(rm.Key, rm.IV), CryptoStreamMode.Read);
		
		int decryptedBytesCount = csDecrypt.Read(plainTextBytes,0,plainTextBytes.Length);
		
		msDecrypt.Close();
		csDecrypt.Close();
		
		return Encoding.UTF8.GetString(plainTextBytes,0,decryptedBytesCount);
	}

	public byte[] StringToByteArrayFastest(string hex) {
		if (hex.Length % 2 == 1)
			throw new Exception("The binary key cannot have an odd number of digits");
		
		byte[] arr = new byte[hex.Length >> 1];
		
		for (int i = 0; i < hex.Length >> 1; ++i)
		{
			arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
		}
		
		return arr;
	}
	
	public int GetHexVal(char hex) {
		int val = (int)hex;
		//For uppercase A-F letters:
		return val - (val < 58 ? 48 : 55);
		//For lowercase a-f letters:
		//return val - (val < 58 ? 48 : 87);
		//Or the two combined, but a bit slower:
		//return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
	}
}