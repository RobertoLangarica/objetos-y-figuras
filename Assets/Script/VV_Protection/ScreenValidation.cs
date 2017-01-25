using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

public class ScreenValidation : MonoBehaviour {

	public Button acceptBtn;
	public GameObject working;
	public Image valid;
	public Image invalid;
	public InputField[] serialInputs;
	public GameObject outOfReachPopup;

	protected VV_GameProtection protection;
	protected string serialValidated;

	//Para el copypaste
	protected bool pasteProcessed = false;

	struct KeyPressedInfo
	{
		public bool recentlyPressed;
		public int lastFramePressed;
	};

	private KeyPressedInfo ctrlKey;
	private KeyPressedInfo vKey;
	private bool pasteExecuted;

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
		
	//Para manejar al paste
	void Update()
	{

		if(!pasteProcessed)	
		{
			pasteExecuted = false;

			//El comando paste se ejcuta aun cuando hay 1 frame de diferencia entre teclas
			if(Input.GetKey(KeyCode.V))
			{
				vKey.lastFramePressed = Time.frameCount;

				if(ctrlKey.lastFramePressed-1 == vKey.lastFramePressed || ctrlKey.lastFramePressed+1 == vKey.lastFramePressed || ctrlKey.lastFramePressed == vKey.lastFramePressed)
				{
					pasteExecuted = true;
				}
			}

			if(Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				ctrlKey.lastFramePressed = Time.frameCount;

				if(ctrlKey.lastFramePressed-1 == vKey.lastFramePressed || ctrlKey.lastFramePressed+1 == vKey.lastFramePressed || ctrlKey.lastFramePressed == vKey.lastFramePressed)
				{
					pasteExecuted = true;
				}
			}

			if(pasteExecuted)
			{
				pasteProcessed = true;
				doPaste();
			}
		}
		else 
		{
			#if UNITY_STANDALONE_OSX
			//Mac
			if(!Input.GetKey(KeyCode.V) || (!Input.GetKey(KeyCode.LeftCommand) && !Input.GetKey(KeyCode.RightCommand)))
			{
				pasteProcessed = false;
			}
			#else
			//Windows
			if(!Input.GetKeyUp(KeyCode.V) || (!Input.GetKeyUp(KeyCode.LeftControl) && !Input.GetKeyUp(KeyCode.RightControl)))
			{
			pasteProcessed = false;
			}
			#endif

		}
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
			serialValidated = serial.ToUpperInvariant();
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

	protected void doPaste()
	{
		string clipboard = UnityEditor.EditorGUIUtility.systemCopyBuffer;

		//Espacios de 4 caracteres
		for(int i = 0; i < serialInputs.Length; i++)
		{
			serialInputs[i].text = getSubstring(4,clipboard,i);	
		}

	}

	protected string getSubstring(int size,string target,int sizeMultipleToSkip = 0)
	{
		string result = "";

		for(int i = size*sizeMultipleToSkip; i < size*sizeMultipleToSkip + size; i++)
		{
			if(i < target.Length)
			{
				result += target.Substring(i,1);
			}
			else
			{
				break;
			}
		}

		return result;
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
		acceptBtn.interactable = false;
		working.SetActive(true);
		blockTextInputs();

		UserDataManager.instance.currentSerial = serialValidated;
		SerialValidator.instance.OnSerialActivated += OnSerialActivated;
		SerialValidator.instance.OnSerialActivationFailed += OnSerialActivationFailed;
		SerialValidator.instance.OnSerialActivationOutOfReach += OnSerialActivationOutOfReach;
		SerialValidator.instance.activateSerial(serialValidated);
	}

	public void OnSerialActivated()
	{
		UserDataManager.instance.isAPirateGame = false;
		UserDataManager.instance.saveSerialNumber(UserDataManager.instance.currentSerial);

		if(ScreenManager.instance)
		{
			//Continua jugando
			ScreenManager.instance.GoToScene("GameMenu");
		}
	}

	public void OnSerialActivationFailed()
	{
		UserDataManager.instance.saveBlockedSerialNumber(UserDataManager.instance.currentSerial);
		UserDataManager.instance.isAPirateGame = true;

		if(ScreenManager.instance)
		{
			//Evitamos el regreso de pantallas
			ScreenManager.instance.backAllowed = false;
			//ScreenManager.instance.GoToSceneDelayed("Blocked",5);
			ScreenManager.instance.GoToScene("Blocked");
		}
	}

	public void OnSerialActivationOutOfReach()
	{
		outOfReachPopup.SetActive(true);
	}

	public void hideOutOfReachPopup()
	{
		acceptBtn.interactable = true;
		working.SetActive(false);
		outOfReachPopup.SetActive(false);
		unblockTextInputs();
	}

	protected void blockTextInputs()
	{
		foreach(InputField input in serialInputs)
		{
			input.enabled = false;
		}
	}

	protected void unblockTextInputs()
	{
		foreach(InputField input in serialInputs)
		{
			input.enabled = true;
		}
	}

	void OnDestroy() {
		SerialValidator.instance.OnSerialActivated -= OnSerialActivated;
		SerialValidator.instance.OnSerialActivationFailed -= OnSerialActivationFailed;
		SerialValidator.instance.OnSerialActivationOutOfReach -= OnSerialActivationOutOfReach;
	}
}
