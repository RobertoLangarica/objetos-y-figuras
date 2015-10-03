using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

public class ScreenValidation : MonoBehaviour {

	public Button acceptBtn;
	public Image valid;
	public Image invalid;
	public InputField[] serialInputs;

	protected VV_GameProtection protection;
	protected string serialValidated;

	// Use this for initialization
	void Start () 
	{
		valid.gameObject.SetActive(false);
		invalid.gameObject.SetActive(false);
		acceptBtn.interactable = false;


		foreach(InputField input in serialInputs)
		{
			input.text = string.Empty;
			input.onValidateInput += characterValidation;
		}

		protection = GetComponent<VV_GameProtection>();
		protection.onSuccess += succes;
		protection.onError += fail;


		serialInputs[0].Select();
		serialInputs[0].ActivateInputField();
	}
	
	private char characterValidation(string input, int charIndex, char addedChar)
	{
		Regex sintaxValidator = new Regex("[^A-Fa-f0-9]");

		if(sintaxValidator.IsMatch(addedChar.ToString()))
		{
			//empty char
			return '\0';
		}

		return addedChar;
	}
	
	public void onSerialChange(int index)
	{
		string serial = getFullSerialString();

		if(serial.Length < 32 )
		{
			invalid.gameObject.SetActive(false);
			valid.gameObject.SetActive(false);
			acceptBtn.interactable = false;
		}
		else
		{
			serialValidated = serial;
			protection.validateSerial(serial);
		}

		if(index < serialInputs.Length-1 && serialInputs[index].text.Length == 4)
		{
			serialInputs[index+1].Select();
			serialInputs[index+1].ActivateInputField();
		}
	}

	protected string getFullSerialString()
	{
		int l = serialInputs.Length;
		StringBuilder builder = new StringBuilder("");
		for(int i = 0; i < l; i++)
		{
			builder.Append(serialInputs[i].text);
		}

		return builder.ToString();
	}

	protected void succes(string m)
	{
		//¿Ya esta bloqueado el numero de serie?
		if(UserDataManager.instance.isPreviouslyBlocked(serialValidated))
		{
			fail ("");
			return;
		}

		valid.gameObject.SetActive(true);
		invalid.gameObject.SetActive(false);

		acceptBtn.interactable = true;
	}

	protected void fail(string m)
	{
		valid.gameObject.SetActive(false);
		invalid.gameObject.SetActive(true);
		acceptBtn.interactable = false;
	}

	public void onClose()
	{
		//EL usuario cancela la accion
		if(ScreenManager.instance)
		{
			ScreenManager.instance.GoToScene("GameMenu");
		}
	}

	public void onAccept()
	{
		//Ya no es pirata
		UserDataManager.instance.isAPirateGame = false;
		if(AnalyticManager.instance)
		{
			AnalyticManager.instance.serialCodeSend(serialValidated);
		}

		//Guardamos el numero de serie
		UserDataManager.instance.currentSerial = serialValidated;
		UserDataManager.instance.saveSerialNumber(serialValidated);

		//Vemos si esta bloqueado el numero de serie mientras todo continua normal
		if(SerialBlocker.instance)
		{
			//Guardamos el serial como instalacion
			SerialBlocker.instance.saveSerialAsInstalled(UserDataManager.instance.currentSerial);
			SerialBlocker.instance.askIsTheSerialIsBlocked(UserDataManager.instance.currentSerial);
		}

		if(ScreenManager.instance)
		{
			ScreenManager.instance.GoToScene("GameMenu");
		}
	}
}
