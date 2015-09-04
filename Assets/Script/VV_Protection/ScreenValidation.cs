using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class ScreenValidation : MonoBehaviour {

	public Button acceptBtn;
	public Image valid;
	public Image invalid;
	public InputField serialInput;

	protected VV_GameProtection protection;
	protected string serialValidated;

	// Use this for initialization
	void Start () 
	{
		valid.gameObject.SetActive(false);
		invalid.gameObject.SetActive(false);
		acceptBtn.interactable = false;
		serialInput.text = string.Empty;
		//serialInput.characterValidation = InputField.CharacterValidation.Alphanumeric;

		serialInput.onValidateInput += characterValidation;

		protection = GetComponent<VV_GameProtection>();
		protection.onSuccess += succes;
		protection.onError += fail;
	}
	
	private char characterValidation(string input, int charIndex, char addedChar)
	{
		Regex sintaxValidator = new Regex("[^A-Fa-f0-9]");

		if(charIndex == 4 || charIndex == 9 || charIndex == 14 || charIndex == 19  || charIndex == 24  || charIndex == 29 || charIndex == 34)
		{
			if(addedChar != '-')
			{
				//empty char
				return '\0';
			}
		}
		else if(sintaxValidator.IsMatch(addedChar.ToString()))
		{
			//empty char
			return '\0';
		}

		return addedChar;
	}
	
	public void onSerialChange()
	{
		//serialInput.characterValidation

		if(serialInput.text.Length < 39 )
		{
			invalid.gameObject.SetActive(false);
			valid.gameObject.SetActive(false);
			acceptBtn.interactable = false;
		}
		else
		{
			serialValidated = serialInput.text;
			protection.validateSerial(serialInput.text);
		}
	}

	protected void succes(string m)
	{
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

		//EL usuario cancela la accion
		if(ScreenManager.instance)
		{
			ScreenManager.instance.GoToScene("GameMenu");
		}
	}
}
