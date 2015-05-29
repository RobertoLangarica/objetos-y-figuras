using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class WebCamPhotoCamera : MonoBehaviour 
{
	public static string PHOTO_PATH;
	public static string PHOTO_PATHRESIZED;
	public static string PHOTO_STORAGE_PATH;
	WebCamTexture webCamTexture;
	WebCamTexture webCamTextureTEST;
	public RawImage BackgroundTexture;
	protected bool pictureTaken;

	protected GameObject Continue;
	protected GameObject TakePicture;
	protected ClientManager client;

	public Sprite click;
	public Sprite againclick;
	public bool nameSelected;


	void Awake()
	{
		TakePicture = GameObject.Find("TakePicture");
		Continue = GameObject.Find("PictureTaken");
		client = GameObject.FindObjectOfType<ClientManager>();

		if(client)
		{
			client.isPhotoReady = false;
			client.isShipReady = false;
			client.isNameReady = false;
		}

		//PictureTaken.SetActive(false);
	}

	void Start() 
	{
		#if UNITY_EDITOR
		PHOTO_PATH = Path.Combine(Application.persistentDataPath,"Pictures/Picture.jpg");
		PHOTO_PATHRESIZED = Path.Combine(Application.persistentDataPath,"Pictures/PictureResized.jpg");
		PHOTO_STORAGE_PATH = Path.Combine(Application.persistentDataPath,"Pictures/");

		DirectoryInfo dir = new DirectoryInfo(PHOTO_STORAGE_PATH);
		if(!dir.Exists)
		{
			dir.Create();
		}
		
		BackgroundTexture = gameObject.GetComponent<RawImage>();
		//BackgroundTexture.pixelInset = new Rect(0,0,Screen.width,Screen.height);
		
		
		
		WebCamDevice[] devices = WebCamTexture.devices;
		for(int i=0; i<devices.Length; i++)
		{
			if(devices[i].isFrontFacing)
			{
				webCamTexture = new WebCamTexture(devices[i].name);
			}
		}
		
		if(webCamTexture)
		{
			renderer.material.mainTexture = webCamTexture;
			webCamTexture.Play();
		}
		
		//GameObject.FindObjectOfType<ShipsPanel>().refresh();
		#else
		PHOTO_PATH = Path.Combine(Application.persistentDataPath,"Pictures/Picture.jpg");
		PHOTO_PATHRESIZED = Path.Combine(Application.persistentDataPath,"Pictures/PictureResized.jpg");
		PHOTO_STORAGE_PATH = Path.Combine(Application.persistentDataPath,"Pictures/");
		DirectoryInfo dir = new DirectoryInfo(PHOTO_STORAGE_PATH);
		if(!dir.Exists)
		{
			dir.Create();
		}

		BackgroundTexture = gameObject.GetComponent<RawImage>();

		WebCamDevice[] devices = WebCamTexture.devices;
		for(int i=0; i<devices.Length; i++)
		{
			if(devices[i].isFrontFacing)
			{
				webCamTexture = new WebCamTexture(devices[i].name);
			}
		}

		if(webCamTexture)
		{
			renderer.material.mainTexture = webCamTexture;
			webCamTexture.Play();
		}
	#endif
	}
	void Update()
	{
		nameSelect();
		if(webCamTexture && webCamTexture.isPlaying && !pictureTaken)
		{
			BackgroundTexture.texture = webCamTexture;
		}
		if(pictureTaken&&nameSelected)
		{
			TakePicture.transform.GetChild(0).FindChild("Continuar").GetComponent<Button>().interactable =true;
		}
		else
		{
			TakePicture.transform.GetChild(0).FindChild("Continuar").GetComponent<Button>().interactable =false;
		}


		#if UNITY_EDITOR
		TakePicture.transform.GetChild(0).FindChild("Continuar").GetComponent<Button>().interactable =true;
		#endif
	}

	public void TakePhoto()
	{
		if( webCamTexture && webCamTexture.isPlaying && !pictureTaken)
		{
			pictureTaken = true;

			Texture2D photo = new Texture2D(640, 480);
			photo.SetPixels(webCamTexture.GetPixels());
			//photo.Resize(128,128);
			photo.Apply();
			//Al parecer android tiene pedos con el encoding PNG
			byte[] bytes = photo.EncodeToJPG();

			if(!File.Exists(PHOTO_PATH))
			{
				Debug.Log("Creating PATH");
				FileStream stream;
				stream = File.Create(PHOTO_PATH);
				stream.Close();
			}

			Debug.Log("Opening PATH");
			//File.WriteAllBytes(PHOTO_PATH, bytes);
			System.IO.File.WriteAllBytes (PHOTO_PATH,bytes);

			StartCoroutine(fade());
			Texture2D myPic;


			bytes = System.IO.File.ReadAllBytes (PHOTO_PATH);
			myPic = new Texture2D(1,1);
			myPic.LoadImage(bytes);
			
			BackgroundTexture.texture = myPic;
			TakePicture.transform.GetChild(0).FindChild("TakePicture").GetComponent<Image>().sprite = click;

			Texture2D resizedPhoto = ScaleTexture(photo, (int)(60*1.33f),60);
			
			//Al parecer android tiene pedos con el encoding PNG
			resizedPhoto.Compress(false);
			bytes = resizedPhoto.EncodeToJPG();
			System.IO.File.WriteAllBytes (PHOTO_PATHRESIZED,bytes);

			if(client)
				client.isPhotoReady = true;//Esto envia la foto
		}
		else if (pictureTaken)
		{
			pictureTaken = false;
			TakePicture.transform.GetChild(0).FindChild("TakePicture").GetComponent<Image>().sprite = againclick;
		}
	}
	IEnumerator fade()
	{
		BackgroundTexture.color = Color.red;
		yield return new WaitForSeconds(.2f);
		BackgroundTexture.color = Color.white;
	}
	public void next()
	{
		#if UNITY_EDITOR
		ScreenManager.instance.GoToScene("MainMenu");
		return;
		#endif

		if(!webCamTexture && nameSelected)
		{
			if(client)
			{
				client.playerName = UserDataManager.instance.name;
				client.isNameReady = true;//Esto envia el nombre
			}

			ScreenManager.instance.GoToScene("MainMenu");
		}
		else if(webCamTexture && pictureTaken && nameSelected)
		{
			if(client)
			{
				client.playerName = UserDataManager.instance.name;
				client.isNameReady = true;//Esto envia el nombre
			}

			ScreenManager.instance.GoToScene("MainMenu");
			webCamTexture.Stop();
		}
	}
	public void buttonDisabled()
	{
		//TakePicture.transform.GetChild(0).FindChild("Continuar").GetComponent<Button>().Interactable = true;
	}
	public void nameSelect()
	{
		nameSelected = true;
		/*if(InputField.instance.myName.text != "")
		{
			nameSelected = true;
		}
		else
		{
			nameSelected = false;
		}*/
	}

	private Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight)
	{
		Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,true);
		Color[] rpixels=result.GetPixels(0);
		float incX=((float)1/source.width)*((float)source.width/targetWidth);
		float incY=((float)1/source.height)*((float)source.height/targetHeight);
		for(int px=0; px<rpixels.Length; px++) {
			rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth),
			                                      incY*((float)Mathf.Floor(px/targetWidth)));
		}
		result.SetPixels(rpixels,0);
		result.Apply();
		return result;
	}

	public void resetData()
	{
		UserDataManager.instance.cleanData();
	}

}