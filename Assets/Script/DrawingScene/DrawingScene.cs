using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DrawingScene : MonoBehaviour 
{
	public XMLLoader xmlLoader;
	public Image image;
	public Button nextButton;
	public Text acknowledge;
	public Notification notification;
	public AudioSource audioSource;
	public AudioClip audioRight;

	protected Photo[] photos;
	protected int[] positions;
	protected int current = -1;
	protected Texture2D texture;
	protected ResourceRequest request;
	protected bool loading = false;

	protected DrawingInput pencilInput;

	protected float percent;
	protected Vector3 currentScale;
	//Para salir
	protected bool exit = false;
	protected float exitElapsed;
	protected float exitInverseTime;


	//Para entrar
	protected bool imageIn = false;
	protected float imageInElapsed;
	protected float imageInInverseTime;
	protected int pictureSeen = -1;

	protected FinishPopUp finishPopUp;


	// Use this for initialization
	void Start () 
	{
		photos = xmlLoader.data.photos;

		//Randomizamos las posiciones
		List<int> index= new List<int>();
		positions = new int[photos.Length]; 

		for(int i=0; i < photos.Length; i++)
		{
			index.Add(i);
		}

		int count = 0;
		while(count < photos.Length)
		{
			int i = Random.Range(0,index.Count);
			positions[count] = index[i];
			index.RemoveAt(i);
			count++;
		}

		pencilInput = FindObjectOfType<DrawingInput>();

		changeTexture(true);
		AnalyticManager.instance.startGame();

		finishPopUp = FindObjectOfType<FinishPopUp>();
	}

	protected void changeTexture(bool load)
	{	
		pictureSeen++;
		if(load)
		{
			advanceCurrent();
			texture = Resources.Load ("200Images/"+photo.name.Split('.')[0]) as Texture2D;
		}
		else
		{
			texture = request.asset as Texture2D;
			request = null;
		}

		acknowledge.text = photo.legal;
		AspectRatioFitter aspect = image.gameObject.GetComponent<AspectRatioFitter>();

		aspect.aspectRatio = (texture.width*1.0f)/texture.height;//haciendolo float
		Sprite tempSpt = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2 (0.5f, 0.5f));
		image.sprite = tempSpt;
	}

	void Update()
	{
		if(exit)
		{
			percent = exitElapsed*exitInverseTime;
			currentScale.x = Mathf.SmoothStep(currentScale.x,0,percent);
			currentScale.y = Mathf.SmoothStep(currentScale.y,0,percent);
			
			if(currentScale.x == 0 && currentScale.y == 0)
			{
				exit = false;
				image.sprite = null;
				Resources.UnloadAsset(texture);
				texture = null;
			}
			
			image.transform.localScale = currentScale;
			exitElapsed += Time.deltaTime;
		}
		else if(loading)
		{
			if(request.isDone)
			{
				loading = false;
				changeTexture(false);
				inCurrentImage();
			}
		}
		else if(imageIn)
		{
			percent = imageInElapsed*imageInInverseTime;
			currentScale.x = Mathf.SmoothStep(currentScale.x,1,percent);
			currentScale.y = Mathf.SmoothStep(currentScale.y,1,percent);
			
			if(currentScale.x == 1 && currentScale.y == 1)
			{
				imageIn = false;
				nextButton.interactable = true;
			}
			
			image.transform.localScale = currentScale;
			imageInElapsed += Time.deltaTime;
		}
	}

	public void showNextImage()
	{
		nextButton.interactable = false;

		notification.onClose += onCloseNotification;
		notification.showToast("correcto",audioRight,2.0f);
	}

	protected void onCloseNotification()
	{
		notification.onClose -= onCloseNotification;

		outCurrentImage();

		
		if(current == photos.Length-1)
		{
			//Pantalla final
			Debug.Log("Pantalla final");
			finishPopUp.show();
		}
		else
		{
			advanceCurrent();
			
			request = Resources.LoadAsync("200Images/"+photo.name.Split('.')[0]);
			loading = true;
		}
	}

	protected void outCurrentImage()
	{
		percent = 0;
		exitElapsed = 0;
		exitInverseTime = 1.0f/0.25f;
		currentScale = image.transform.localScale;
		exit = true;

		if(pencilInput)
		{
			pencilInput.erraseAll();
		}
	}
	protected void inCurrentImage()
	{
		percent = 0;
		imageInElapsed = 0;
		imageInInverseTime = 1.0f/0.25f;
		currentScale = image.transform.localScale;
		imageIn = true;
	}

	public Photo photo
	{
		get{return photos[current];}
	}

	protected void advanceCurrent()
	{
		current++;
		
		if(current >= positions.Length)
		{
			current = 0;
		}
	}


	void OnDestroy()
	{
		if(image)
		{
			image.sprite = null;
		}

		if(texture)
		{
			Resources.UnloadAsset(texture);
		}
		else
		{
			Resources.UnloadUnusedAssets();
		}
	}

	void OnDisable() {
		AnalyticManager.instance.finsh("Observa", "PicturesSeen",pictureSeen.ToString());
	}
}