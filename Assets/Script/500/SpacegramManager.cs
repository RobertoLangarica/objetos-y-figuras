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
	public AudioClip positionatedAudio;

	protected Level currentLevel;
	protected Placeholder placeholder;
	protected GameObject reference;
	protected int currLevel = 0;
	// Use this for initialization
	void Start ()
	{
		input.onAnyDrag += onDrag;
		input.onDragFinish += onDragFinish;
		input.allowRotation = !cannotRotate;
		
		notification.onClose += onContinue;
		startNewLevel();
	}	
	void startNewLevel()
	{
		for(int i = 0;i < shapes.Length;i++)
		{
			if(shapes[i] != null)
			{
				input.onDragFinish -= shapes[i].GetComponent<Shape>().onRotationComplete;
				shapes[i].GetComponent<Shape>().destroy(0.5f);
			}
		}
		if(reference)
		{
			Destroy(reference);
		}
		if(placeholder)
		{
			Destroy(placeholder.gameObject);
		}
		input.gameObject.SetActive(true);


		Level[] levlDificultie = LevelManager.instance.getLevels(currLevel);
		int rand = Random.Range(0,levlDificultie.Length);
		
		lvlToPrepare = levlDificultie[rand].name;
		currLevel++;
		
		currentLevel = LevelManager.instance.getLevel(lvlToPrepare);
		
		if(currentLevel == null)
		{
			Debug.LogError("GM-> No existe el nivel especificado: "+lvlToPrepare);
			return;
		}
		
		initializeShapes();
	}

	void onDrag()
	{
		DOTween.Kill("SnapMove");
	}

	void onDragFinish()
	{
		checkForLevelComplete();
		DOTween.Play("SnapMove");
		if(input.selected != null)
		{
			if(input.selected.gameObject.GetComponent<Shape>().isPositionated)
			{
				//Flash de color
				SpriteRenderer sr = input.selected.spriteRenderer;
				input.selected.GetComponent<FlashColor>().startFlash(sr,0.6f);

				if(audioSource && positionatedAudio)
				{
					audioSource.PlayOneShot(positionatedAudio,1);
				}
			}
		}
	}

	void initializeReferenceImage()
	{
		foreach(GameObject shape in shapes)
		{
			shape.transform.Find("New Sprite").GetComponent<SpriteRenderer>().enabled = false;
		}

		GameObject tmp = (GameObject)Resources.Load("References/"+currentLevel.name+"_reference");

		reference = GameObject.Instantiate(tmp) as GameObject;
		
		reference.GetComponent<Renderer>().material.color = new Color(reference.GetComponent<Renderer>().material.color.r
		                                              ,reference.GetComponent<Renderer>().material.color.g
		                                              ,reference.GetComponent<Renderer>().material.color.b
		                                              ,0);
		reference.GetComponent<Renderer>().material.DOFade(1,1);
	}

	void initializeShapes()
	{
		GameObject go;
		continueBtn.interactable = true;
		if(AnalyticManager.instance)
		{
			AnalyticManager.instance.startGame();
		}
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
			sp.sortingOrder = input.nextSort;

			sp.onRotationComplete();
			input.onDragFinish += sp.onRotationComplete;

			go.transform.localPosition = new Vector3 (go.transform.localPosition.x,go.transform.localPosition.y,0);
		}

		//placeholder
		GameObject tmp = (GameObject)Resources.Load("Placeholders/"+lvlToPrepare);
		placeholder = ((GameObject)GameObject.Instantiate(tmp)).GetComponent<Placeholder>();
		placeholder.input = input.gameObject;

		//Inicializamos los valores para el placeholder
		initializePlaceholder();

		SetColor();
	}
	
	protected void SetColor()
	{
		int rand; 
		List<int> colorsShown = new List<int>();
		int count = 0;
		int repeatcount = 0;
		while(count < shapes.Length)
		{
			rand = Random.Range(1,System.Enum.GetValues(typeof(BaseShape.EShapeColor)).Length-1);
			
			if(colorsShown.Contains(rand))
			{
				if(repeatcount >(System.Enum.GetValues(typeof(BaseShape.EShapeColor)).Length-1)*1.5f )
				{
					//Se asigna un color repetido
					colorsShown.Clear();
					shapes[count].GetComponent<BaseShape>().color = (BaseShape.EShapeColor)rand;
					count++;
					repeatcount = 0;
				}
			}
			else
			{
				colorsShown.Add(rand);
				shapes[count].GetComponent<BaseShape>().color = (BaseShape.EShapeColor)rand;
				count++;
			}
			
			repeatcount++;
		}
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
		foreach(GameObject val in shapes)
		{
			if(val != null)
			{
				val.GetComponent<Shape>().isPositionated = false;
			}
		}
		if(placeholder.isCorrect())
		{
			placeholder.canTurnOn = false;
			if(AnalyticManager.instance)
			{
				AnalyticManager.instance.finsh("Construye","SpaceGram",currentLevel.name);
			}
			initializeReferenceImage();
			continueBtn.interactable = false;
			input.selected = null;
			input.gameObject.SetActive(false);
			notification.showToast("correcto",audioRight,2);
		}
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
				audioSource.PlayOneShot(audioWrong,1);
			}
		}
	}

	protected void removeShapesAndPlaceHolder()
	{
		foreach(GameObject shape in shapes)
		{
			Debug.Log("S");
			shape.GetComponent<Renderer>().material.DOFade(0,0.5f);
		}

		placeholder.GetComponent<Renderer>().material.DOFade(0,0.5f);
	}

	public void onContinue()
	{
		//Evitamos que se alcancen niveles no deseados
		/*if(UserDataManager.instance.level < LevelManager.instance.maxLevel)
		{
			UserDataManager.instance.level = UserDataManager.instance.level+1;
		}*/
		if(currLevel <4)
		{
			//Debug.Log("S");
			//removeShapesAndPlaceHolder();
			//ScreenManager.instance.GoToScene("SpacegramMenu");
			//eliminar nivel y cargar el nuevo
			startNewLevel();
		}
		else
		{
			Debug.Log("S");
			Debug.Log(GameObject.FindObjectOfType<FinishPopUp>());
			GameObject.FindObjectOfType<FinishPopUp>().show();
		}
	}

	public void exitGame()
	{
		ScreenManager.instance.GoToScene("Construye");
	}
}