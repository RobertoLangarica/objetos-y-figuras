using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;
using System.Text.RegularExpressions;

public class VV_GameProtection : MonoBehaviour 
{
	[HideInInspector]
	public delegate void OnMessage(string message);
	[HideInInspector]
	public OnMessage onError;
	[HideInInspector]
	public OnMessage onSuccess;

	public bool _mustShowDebugInfo = true;

	void Start () 
	{
		onError		+= foo;
		onSuccess	+= foo;
	}

	public void foo(string message)
	{
		//Debug.Log ("Message: "+message);
	}

	public void validateSerial(string serial)
	{
		serial = serial.Replace("-",string.Empty);

		serial = serial.ToUpperInvariant();

		Regex sintaxValidator = new Regex("[^A-F0-9]");

		if(serial.Length != 32)
		{
			if(_mustShowDebugInfo)
				Debug.Log ("Largo inadecuado");
			
			//El tamaño no es el adecuado
			onError("El número de serie es inválido.");
		}
		else if(sintaxValidator.IsMatch(serial))
		{
			if(_mustShowDebugInfo)
				Debug.Log ("No es hexadecimal");
			//No viene en formato hexadecimal
			onError("El número de serie es inválido.");
		}

		string decrypted = Decrypt(serial);
		string[] parts = decrypted.Split('_');

		if(parts.Length != 3)
		{
			if(_mustShowDebugInfo)
				Debug.Log ("La cadena desencriptada es otra cosa");
			onError("El número de serie es inválido.");
		}
		else
		{

			if(parts[0].Equals("OYF") && parts[2].Equals("VV"))
			{
				Regex comparer = new Regex(@"\d{8}");

				if(parts[1].Length != 8)
				{
					if(_mustShowDebugInfo)
						Debug.Log ("Se desencripto otra cosa");
					onError("El número de serie es inválido.");
				}
				else if(comparer.IsMatch(parts[1]))
				{
					//Valido
					if(_mustShowDebugInfo)
						Debug.Log ("El serial se valido exitosamente.");
					onSuccess("El número de serie es válido.");
				}
				else
				{
					if(_mustShowDebugInfo)
						Debug.Log ("Se desencripto otra cosa");
					onError("El número de serie es inválido.");
				}
			}
			else
			{
				if(_mustShowDebugInfo)
					Debug.Log ("Se desencripto otra cosa");
				onError("El número de serie es inválido.");
			}
		}
	}

	protected string Decrypt(string text)
	{
		//Nos traemos la key e IV
		TextAsset asset = Resources.Load("KIV/OYFVV2015_KIV") as TextAsset;
		string[] lines = asset.text.Split("\n"[0]);
		MemoryStream msDecrypt = null;
		CryptoStream csDecrypt = null;

		//Leemos key e IV
		RijndaelManaged rm = new RijndaelManaged();
		rm.Key = Convert.FromBase64String(lines[0]);
		rm.IV = Convert.FromBase64String(lines[1]);

		try
		{
			byte[] textBytes = StringToByteArrayFastest(text.Replace("-",""));
			byte[] plainTextBytes = new byte[textBytes.Length];
			
			//Descifrado
			//Stream con la informacion cifrada
			msDecrypt = new MemoryStream(textBytes);
			// Crear un flujo de descifrado basado en el flujo de los datos
			csDecrypt = new CryptoStream(msDecrypt, rm.CreateDecryptor(rm.Key, rm.IV), CryptoStreamMode.Read);
			
			int decryptedBytesCount = csDecrypt.Read(plainTextBytes,0,plainTextBytes.Length);
			
			msDecrypt.Close();
			csDecrypt.Close();
			
			return Encoding.UTF8.GetString(plainTextBytes,0,decryptedBytesCount);
		}
		catch(Exception ex)
		{
			//Debug.Log ("Ocurrio un error al desencriptar "+ex.Message);
			return "";
		}
		finally
		{
			if(msDecrypt != null)
			{
				msDecrypt.Close();
			}

			if(csDecrypt != null)
			{
				csDecrypt.Close();
			}
		}
	}

	public byte[] StringToByteArrayFastest(string hex) {
		if (hex.Length % 2 == 1)
		{
			if(_mustShowDebugInfo)
				Debug.Log("The binary key cannot have an odd number of digits");
			onError("Formato inválido.");
		}
			
		
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