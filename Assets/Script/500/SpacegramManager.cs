using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class SpacegramManager : MonoBehaviour
{
	//Nivel que se debe de preparar
	public static string lvlToPrepare = "SN01_01";
	public static bool cannotRotate = false;

	//public Button sendBtn;
	public Button continueBtn;
	public Notification notification;
	public GameObject[] shapes;
	public TangramInput input;
	public AudioSource audioSource; 
	public AudioClip audioWrong;
	public AudioClip audioRight;
	public AudioClip finalAudio;

	protected Level currentLevel;
	protected Placeholder placeholder;
	protected GameObject reference;
	protected ClientManager client;

	// Use this for initialization
	void Start ()
	{
		client = GameObject.FindObjectOfType<ClientManager>();

		currentLevel = LevelManager.instance.getLevel(lvlToPrepare);
		
		if(currentLevel == null)
		{
			Debug.LogError("GM-> No existe el nivel especificado: "+lvlToPrepare);
			return;
		}

		if(UserDataManager.instance.isLevelComplete(currentLevel.name))
		{
			Debug.Log("GM-> Nivel completado previamente.");
			//initializeReferenceImage();
			initializeShapes();
			//continueBtn.gameObject.SetActive(false);
			//sendBtn.gameObject.SetActive(true);
		}
		else
		{
			Debug.Log("GM-> Inicializando nuevo nivel.");
			initializeShapes();
			//sendBtn.gameObject.SetActive(false);
			//continueBtn.gameObject.SetActive(false);
		}

		input.onAnyDrag += onDrag;
		input.onDragFinish += onDragFinish;
		input.allowRotation = !cannotRotate;

		notification.onClose += onContinue;
	}	

	void onDrag()
	{
		DOTween.Kill("SnapMove");
	}

	void onDragFinish()
	{
		checkForLevelComplete();
		DOTween.Play("SnapMove");
	}

	void initializeReferenceImage()
	{
		GameObject tmp = (GameObject)Resources.Load("References/"+currentLevel.name+"_reference");

		reference = GameObject.Instantiate(tmp) as GameObject;
		
		reference.renderer.material.color = new Color(reference.renderer.material.color.r
		                                              ,reference.renderer.material.color.g
		                                              ,reference.renderer.material.color.b
		                                              ,0);
		reference.renderer.material.DOFade(1,1);
	}

	void initializeShapes()
	{
		GameObject go;
		AnalyticManager.instance.startGame();
		//Imagenes
		Piece[] pieces = currentLevel.pieces;
		Pair[] pairs = currentLevel.pairs;

		shapes = new GameObject[pieces.Length];

		float min;
		float max;
		for(int i = 0; i < pieces.Length; i++)
		{
			if(Random.value < 0.5f)
			{
				//Izquierda
				min = Screen.width * .05f;
				max = Screen.width*0.2f;
			}
			else
			{
				//Derecha
				min = Screen.width*0.8f;
				max = Screen.width - Screen.width * .05f;
			}
			//
			Vector3 randPos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(min,max),Random.Range(Screen.height*0.2f,Screen.height*0.8f),0));
			Vector3 randRot = new Vector3(0,0,(Random.Range(0,5)*15));
			randPos.z = 0;

			if(cannotRotate)
			{
				for(int j = 0;j < pairs.Length;j++)
				{
					string[] names = pairs[j].shapes.Split(new char[1]{','});
					if(names[0] == pieces[i].name)
					{	
						int[] angles = getAngles(pairs[j].angles);
						randRot.z = angles[0];
						break;
					}
				}
			}

			GameObject shape = (GameObject)Resources.Load("Pieces/"+pieces[i].name);
			go = GameObject.Instantiate(shape,randPos,Quaternion.Euler(randRot)) as GameObject;
			shapes[i] = go;

			//Que la figura no se ponga en una rotacion invalida
			Shape sp = go.GetComponent<Shape>();
			sp.name = pieces[i].name;
			sp.baseStart();//Forzamos un basestart
			sp.color = (BaseShape.EShapeColor)Random.Range(0,System.Enum.GetValues(typeof(BaseShape.EShapeColor)).Length-1);
			sp.sortingOrder = input.nextSort;

			sp.onRotationComplete();
			input.onDragFinish += sp.onRotationComplete;

			go.transform.localPosition = new Vector3 (go.transform.localPosition.x,go.transform.localPosition.y,0);
		}

		//placeholder
		GameObject tmp = (GameObject)Resources.Load("Placeholders/"+lvlToPrepare);
		placeholder = ((GameObject)GameObject.Instantiate(tmp)).GetComponent<Placeholder>();

		//Inicializamos los valores para el placeholder
		initializePlaceholder();
	}

	protected void initializePlaceholder()
	{
		Pair[] pairs = currentLevel.pairs;

		for(int i = 0; i < pairs.Length; i++)
		{
			GameObject[] sps = getShapes(pairs[i].shapes);
			int[] angles = getAngles(pairs[i].angles);

			placeholder.fillChildInfo(pairs[i].piece,sps,angles,pairs[i].range);
		}
	}

	protected GameObject[] getShapes(string target)
	{
		List<GameObject> result = new List<GameObject>();
		string[] names = target.Split(new char[1]{','});

		foreach(string name in names)
		{
			foreach(GameObject go in shapes)
			{
				if(go.GetComponent<Shape>().name == name)
				{
					result.Add(go);
				}
			}
		}

		return result.ToArray();
	}

	protected int[] getAngles(string target)
	{
		List<int> result = new List<int>();
		string[] angles = target.Split(new char[1]{','});

		foreach(string angle in angles)
		{
			result.Add(int.Parse(angle));
		}
		
		return result.ToArray();
	}
	
	public void checkForLevelComplete()
	{
		/*Debug.Log("GM-> Ejercicio correcto");
		//Lo marcamos como completo
		UserDataManager.instance.markLevelAsComplete(currentLevel.name);
		//Removemos las piezas y el placeholder
		//removeShapesAndPlaceHolder();
		//Agregamos la imagen bonita de la nave y el boton de continue
		initializeReferenceImage();
		continueBtn.gameObject.SetActive(true);
		
		GameObject.FindObjectOfType<ShipsPanel>().refresh();

		//Mandamos la nave
		if(client)
		{
			client.shipName = lvlToPrepare;
			client.isShipReady = true;//Esto envia la nave
		}

		return;*/
		#if UNITY_EDITOR
		if(placeholder.isCorrect())
		{
			Debug.Log("GM-> Ejercicio correcto");
			AnalyticManager.instance.finsh("Construye","SpaceGram",currentLevel.name);
			//Lo marcamos como completo
			UserDataManager.instance.markLevelAsComplete(currentLevel.name);
			//Removemos las piezas y el placeholder
			//removeShapesAndPlaceHolder();
			//Agregamos la imagen bonita de la nave y el boton de continue
			initializeReferenceImage();
			continueBtn.interactable = false;
			//sendBtn.gameObject.SetActive(true);
			GameObject.FindObjectOfType<DragRecognizer>().enabled = false;
			input.selected = null;
			input.gameObject.SetActive(false);

			//Mandamos la nave
			if(client)
			{
				client.shipName = lvlToPrepare;
				client.isShipReady = true;//Esto envia la nave
			}

			if((UserDataManager.instance.getCompletedLevels().Length%3) == 0)
			{
				if(UserDataManager.instance.level < LevelManager.instance.maxLevel)
				{
					UserDataManager.instance.level = UserDataManager.instance.level+1;
				}
			}
			notification.showToast("correcto",audioRight,2);
		}
	#else
		if(placeholder.isCorrect())
		{
			Debug.Log("GM-> Ejercicio correcto");
			//Lo marcamos como completo
			UserDataManager.instance.markLevelAsComplete(currentLevel.name);
			//Removemos las piezas y el placeholder
			//removeShapesAndPlaceHolder();
			//Agregamos la imagen bonita de la nave y el boton de continue
			initializeReferenceImage();
			continueBtn.interactable = false;
			input.selected = null;
			input.gameObject.SetActive(false);

			//Mandamos la nave
			if(client)
			{
				client.shipName = lvlToPrepare;
				client.isShipReady = true;//Esto envia la nave
			}
			
			if((UserDataManager.instance.getCompletedLevels().Length%3) == 0)
			{
				if(UserDataManager.instance.level < LevelManager.instance.maxLevel)
				{
					UserDataManager.instance.level = UserDataManager.instance.level+1;
				}
			}
			notification.showToast("correcto",audioRight,2);
		}
	#endif
	}

	public void verifyExcercise()
	{
		foreach(GameObject val in shapes)
		{
			if(val != null)
			{
				val.GetComponent<Shape>().isPositionated = false;
			}
		}
		if(!placeholder.isCorrect())
		{
			for(int i = 0;i < shapes.Length;i++)
			{
				if(!shapes[i].GetComponent<Shape>().isPositionated)
				{
					shapes[i].GetComponent<ShakeTransform>().startAction(0.5f);
				}
			}
			if(audioSource && audioWrong)
			{
				audioSource.PlayOneShot(audioWrong);
			}
		}
	}

	protected void removeShapesAndPlaceHolder()
	{
		foreach(GameObject shape in shapes)
		{
			shape.renderer.material.DOFade(0,0.5f);
		}

		placeholder.renderer.material.DOFade(0,0.5f);
	}

	public void onContinue()
	{
		//Evitamos que se alcancen niveles no deseados
		/*if(UserDataManager.instance.level < LevelManager.instance.maxLevel)
		{
			UserDataManager.instance.level = UserDataManager.instance.level+1;
		}*/
		ScreenManager.instance.GoToScene("SpacegramMenu");
	}

	public void onSendShip()
	{
		ShipTravelController.myCurrentShip = lvlToPrepare;
		ScreenManager.instance.GoToScene("Space");
	}

	public void sendShip()
	{
		//Mandamos la nave
		if(client)
		{
			client.shipName = lvlToPrepare;
			client.isShipReady = true;//Esto envia la nave
		}

		ScreenManager.instance.GoToScene("SpacegramMenu");
	}

	public void exitGame()
	{
		ScreenManager.instance.GoToScene("SpacegramMenu");
	}
}