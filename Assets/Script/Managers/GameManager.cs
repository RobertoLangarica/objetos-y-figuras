using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	//Nivel que se debe de preparar
	public static string lvlToPrepare = "SN01_01";

	public Button sendBtn;
	public Button continueBtn;
	public GameObject[] shapes;

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
			continueBtn.gameObject.SetActive(false);
			sendBtn.gameObject.SetActive(true);
		}
		else
		{
			Debug.Log("GM-> Inicializando nuevo nivel.");
			initializeShapes();
			sendBtn.gameObject.SetActive(false);
			continueBtn.gameObject.SetActive(false);
		}
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

		shapes = new GameObject[pieces.Length];

		float min;
		float max;
		Shape.sort = -32767;

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
			GameObject shape = (GameObject)Resources.Load("Pieces/"+pieces[i].name);
			go = GameObject.Instantiate(shape,randPos,Quaternion.Euler(randRot)) as GameObject;
			shapes[i] = go;

			//Que la figura no se ponga en una rotacion invalida
			Shape sp = go.GetComponent<Shape>();
			sp.name = pieces[i].name;
			sp.onRotationComplete(0);
			sp.GetComponent<SpriteRenderer>().sortingOrder = Shape.sort+i;
			Shape.sort = Shape.sort +i;
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
		removeShapesAndPlaceHolder();
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
			AnalyticManager.instance.finishGame(currentLevel.name);
			//Lo marcamos como completo
			UserDataManager.instance.markLevelAsComplete(currentLevel.name);
			//Removemos las piezas y el placeholder
			removeShapesAndPlaceHolder();
			//Agregamos la imagen bonita de la nave y el boton de continue
			initializeReferenceImage();
			continueBtn.gameObject.SetActive(true);
			sendBtn.gameObject.SetActive(true);
			GameObject.FindObjectOfType<DragRecognizer>().enabled = false;

			for(int i =0; i<shapes.Length; i++)
			{
				shapes[i].transform.FindChild("Transform0").gameObject.SetActive(false);
			}
			//GameObject.FindObjectOfType<ShipsPanel>().refresh();

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
		}
	#else
		if(placeholder.isCorrect())
		{
			Debug.Log("GM-> Ejercicio correcto");
			//Lo marcamos como completo
			UserDataManager.instance.markLevelAsComplete(currentLevel.name);
			//Removemos las piezas y el placeholder
			removeShapesAndPlaceHolder();
			//Agregamos la imagen bonita de la nave y el boton de continue
			initializeReferenceImage();
			continueBtn.gameObject.SetActive(true);
			sendBtn.gameObject.SetActive(true);

			GameObject.FindObjectOfType<ShipsPanel>().refresh();

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

		}
	#endif
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
		ScreenManager.instance.GoToScene("MainMenu");
	}

	public void onSendShip()
	{
		ScreenManager.instance.myCurrentShip = lvlToPrepare;
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

		ScreenManager.instance.GoToScene("MainMenu");
	}

	public void exitGame()
	{
		ScreenManager.instance.GoToScene("MainMenu");
	}
}