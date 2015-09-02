using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

public class VV_GameProtection : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		//saveKey("VVOYF2015");

		string original = "0000000_OYFVV2015";
		string encrypted1 = Encrypt(original,"VVOYF2015");
		string encrypted2 = Encrypt(original,"VVOYF2015");
		string decrypted1 = Decrypt(encrypted1,"VVOYF2015");
		string decrypted2 = Decrypt(encrypted2,"VVOYF2015");

		Debug.Log (encrypted1);
		Debug.Log (decrypted1);
		Debug.Log (encrypted2);
		Debug.Log (decrypted2);
	}

	public void saveKey(string fileName)
	{
		RijndaelManaged rm = new RijndaelManaged();
		rm.GenerateKey();
		string keyString = Convert.ToBase64String(rm.Key);
		StreamWriter keyFile = null;

		keyFile = File.CreateText(fileName);
		keyFile.Write(keyString);
		keyFile.Close();
	}

	public string Encrypt(string text,string keyFileName)
	{
		RijndaelManaged rm = new RijndaelManaged();
		StreamReader keyStream;
		byte[] encrypted;
		rm.GenerateIV();

		keyStream = File.OpenText(keyFileName);
		rm.Key = Convert.FromBase64String(keyStream.ReadToEnd());
		keyStream.Close();

		// Create a decryptor to perform the stream transform.
		ICryptoTransform encryptor = rm.CreateEncryptor(rm.Key, rm.IV);
		
		// Create the streams used for encryption. 
		using (MemoryStream msEncrypt = new MemoryStream())
		{
			using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			{
				using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
				{
					
					//Write all data to the stream.
					swEncrypt.Write(text);
				}
				encrypted = msEncrypt.ToArray();
			}
		}

		return Convert.ToBase64String(encrypted);
	}

	public string Decrypt(string text,string keyFileName)
	{
		RijndaelManaged rm = new RijndaelManaged();
		StreamReader keyStream;
		string result;

		rm.GenerateIV();
		
		keyStream = File.OpenText(keyFileName);
		rm.Key = Convert.FromBase64String(keyStream.ReadToEnd());
		keyStream.Close();
		
		// Create a decryptor to perform the stream transform.
		ICryptoTransform encryptor = rm.CreateEncryptor(rm.Key, rm.IV);
		
		// Create a decrytor to perform the stream transform.
		ICryptoTransform decryptor = rm.CreateDecryptor(rm.Key, rm.IV);
		
		// Create the streams used for decryption. 
		using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(text)))
		{
			using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
			{
				using (StreamReader srDecrypt = new StreamReader(csDecrypt))
				{
					
					// Read the decrypted bytes from the decrypting stream 
					// and place them in a string.
					result = srDecrypt.ReadToEnd();
				}
			}
		}
		
		return Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(result));
	}


	static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
	{	
		// Declare the string used to hold 
		// the decrypted text. 
		string plaintext = null;
		
		// Create an RijndaelManaged object 
		// with the specified key and IV. 
		using (RijndaelManaged rijAlg = new RijndaelManaged())
		{
			rijAlg.Key = Key;
			rijAlg.IV = IV;
			
			// Create a decrytor to perform the stream transform.
			ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
			
			// Create the streams used for decryption. 
			using (MemoryStream msDecrypt = new MemoryStream(cipherText))
			{
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (StreamReader srDecrypt = new StreamReader(csDecrypt))
					{
						
						// Read the decrypted bytes from the decrypting stream 
						// and place them in a string.
						plaintext = srDecrypt.ReadToEnd();
					}
				}
			}
			
		}
		
		return plaintext;
		
	}
}