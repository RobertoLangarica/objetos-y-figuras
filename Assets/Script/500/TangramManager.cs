using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public enum ETangramTypes
{
	SAME_SHAPE,
	ALL_SHAPES
}

public class TangramManager : MonoBehaviour 
{
	public static bool cannotRotate = false;
	public static ETangramTypes tType;

	public Button continueBtn;
	public List<GameObject> shapes = new List<GameObject>();
	public TangramInput input;
	public AudioSource audioSource; 
	public AudioClip audioWrong;
	public AudioClip audioRight;
	public AudioClip finalAudio;
	
	protected Level currentLevel;
	protected List<string> previousLevel = new List<string>();
	protected int currLevel = 1;
	protected List<Placeholder> placeholder = new List<Placeholder>();
	protected List<Level> fTypeAllShapes;
	protected XMLLoader loader;
	protected Notification notification;
	protected bool excerciseFinished;

	void Start ()
	{		
		loader = GameObject.FindObjectOfType<XMLLoader>();

		input.onAnyDrag += onDrag;
		input.onDragFinish += onDragFinish;
		input.allowRotation = !cannotRotate;

		levelStart();
		notification = GameObject.Find("Notification").GetComponent<Notification>();
		notification.onClose += levelStart;
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

	protected void levelStart()
	{
		AnalyticManager.instance.startGame();
		excerciseFinished = false;
		for(int i = 0;i < shapes.Count;i++)
		{
			if(shapes[i] != null)
			{
				input.onDragFinish -= shapes[i].GetComponent<Shape>().onRotationComplete;
				shapes[i].GetComponent<Shape>().destroy(0.5f);
			}
		}
		for(int i = placeholder.Count-1;i > -1 ;i--)
		{
			if(placeholder[i] != null)
			{
				Destroy(placeholder[i].gameObject);
			}
		}
		shapes = new List<GameObject>();
		placeholder = new List<Placeholder>();

		continueBtn.GetComponent<Button>().interactable = true;

		switch(tType)
		{
		case(ETangramTypes.SAME_SHAPE):
		{
			currentLevel = selectLevel();
		}
			break;
		case(ETangramTypes.ALL_SHAPES):
		{
			currentLevel = takeLevel();
		}
			break;
		}
		
		initializeShapes(currentLevel);

		GameObject tmp = (GameObject)Resources.Load("500/"+currentLevel.name);
		previousLevel.Add(currentLevel.name);
		placeholder.Add(((GameObject)GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity)).GetComponent<Placeholder>());
		Debug.Log (currentLevel.name);
		
		initializePlaceholder();

		input.gameObject.SetActive(true);
		
		SetColor();
	}

	void initializeShapes(Level initLevel)
	{
		GameObject go;
		//AnalyticManager.instance.startGame();
		//Imagenes
		Piece[] pieces = initLevel.pieces;
		Pair[] pairs = initLevel.pairs;
		
		float min;
		float max;
		float posTemp = -0.03f;
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
			
			GameObject shape = (GameObject)Resources.Load("500/"+pieces[i].name);
			go = GameObject.Instantiate(shape,randPos,Quaternion.Euler(randRot)) as GameObject;
			Vector3 tempV3 = Vector3.zero;
			tempV3.z = 1;
			if(pieces[i].scale.Contains(","))
			{
				tempV3.x = tempV3.z = float.Parse(pieces[i].scale.Substring(0,pieces[i].scale.IndexOf(',')));
				tempV3.y = float.Parse(pieces[i].scale.Substring(pieces[i].scale.IndexOf(',')+1));
			}
			else
			{
				tempV3.x = tempV3.y = tempV3.z = float.Parse(pieces[i].scale);
			}
			//go.transform.FindChild("New Sprite").localScale = tempV3;
			go.transform.localScale = tempV3;

			if(pieces[i].scaleCiclre != null && pieces[i].scaleCiclre.Contains(","))
			{
				tempV3.x = float.Parse(pieces[i].scaleCiclre.Substring(0,pieces[i].scaleCiclre.IndexOf(',')));
				tempV3.y = float.Parse(pieces[i].scaleCiclre.Substring(pieces[i].scaleCiclre.IndexOf(',')+1));
			}
			else if(pieces[i].scaleCiclre != null)
			{
				tempV3.x = tempV3.y = float.Parse(pieces[i].scaleCiclre);
			}
			go.transform.FindChild("rotate").localScale = (tempV3)*1.7f;

			if(pieces[i].scaleSquare != null)
			{
				tempV3.x = float.Parse(pieces[i].scaleSquare.Substring(0,pieces[i].scaleSquare.IndexOf(',')));
				tempV3.y = float.Parse(pieces[i].scaleSquare.Substring(pieces[i].scaleSquare.IndexOf(',')+1));
			}
			Transform translateChild = go.transform.FindChild("translate");
			if(translateChild)
			{translateChild.localScale = tempV3;}

			if(initLevel.radius > 0)
			{
				go.GetComponent<CircleCollider2D>().radius = initLevel.radius;
			}
			if(pieces[i].radius > 0)
			{
				go.GetComponent<CircleCollider2D>().radius = pieces[i].radius;
			}

			shapes.Add(go);
			
			//Que la figura no se ponga en una rotacion invalida
			Shape sp = go.GetComponent<Shape>();
			sp.name = pieces[i].name;
			sp.onRotationComplete();
			input.onDragFinish += sp.onRotationComplete;
			
			go.transform.localPosition = new Vector3 (go.transform.localPosition.x,go.transform.localPosition.y,posTemp);
			posTemp -= .01f;
		}
	}
	
	protected void SetColor()
	{
		int rand; 
		List<int> colorsShown = new List<int>();
		int count = 0;
		int repeatcount = 0;
		Debug.Log("S");
		while(count < shapes.Count)
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

	protected Level selectLevel()
	{
		Level ndx = null;
		List<Level> selectable = new List<Level>(loader.data.levels500);

		if(previousLevel.Count > 0)
		{
			for(int i = 0;i < previousLevel.Count;i++)
			{
				for(int j = 0;j < selectable.Count;j++)
				{
					if(selectable[j].name == previousLevel[i])
					{
						selectable.RemoveAt(j);
						break;
					}
				}
			}
		}
		previousLevel.Clear();
		previousLevel = new List<string>();

		if(currLevel < 3)
		{
			for(int i = 0;i < selectable.Count;i++)
			{
				if(selectable[i].difficulty >= 3)
				{
					selectable.RemoveAt(i);
					i--;
				}
			}
			ndx = selectable[Random.Range(0,selectable.Count-1)];
		}
		else if(currLevel < 5)
		{
			for(int i = 0;i < selectable.Count;i++)
			{
				if(selectable[i].difficulty != 2)
				{
					selectable.RemoveAt(i);
					i--;
				}
			}
			ndx = selectable[Random.Range(0,selectable.Count-1)];
		}
		else
		{
			for(int i = 0;i < selectable.Count;i++)
			{
				if(selectable[i].difficulty != 3)
				{
					selectable.RemoveAt(i);
					i--;
				}
			}
			ndx = selectable[Random.Range(0,selectable.Count-1)];
		}
		return ndx;
	}

	protected Level takeLevel()
	{
		Level result = null;
		if(fTypeAllShapes == null)
		{
			fTypeAllShapes = new List<Level>();
			for(int i = 0;i < loader.data.levels500.Length;i++)
			{
				if(loader.data.levels500[i].fType == "allPieces")
				{
					fTypeAllShapes.Add(loader.data.levels500[i]);
				}
			}
		}
		if(fTypeAllShapes.Count != 0)
		{
			result = fTypeAllShapes[Random.Range(0,fTypeAllShapes.Count-1)];
			fTypeAllShapes.Remove(result);
		}
		return result;
	}

	protected void initializePlaceholder()
	{
		GameObject tmp = null;
		int count = 1;
		Level otherPieces = null;

		if(currLevel < 5 && tType == ETangramTypes.SAME_SHAPE)
		{
			foreach(Level val in loader.data.levels500)
			{
				if(val.fType == currentLevel.fType && !val.Equals(currentLevel))
				{
					if(currLevel < 3 && currentLevel.pieces[0].name == val.pieces[0].name)
					{
						tmp = (GameObject)Resources.Load("500/"+val.name);
						previousLevel.Add(val.name);
						placeholder.Add(((GameObject)GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity)).GetComponent<Placeholder>());
						placeholder[count].gameObject.GetComponent<SpriteRenderer>().enabled = false;
						count++;
					}
					if(currLevel > 2 && val.difficulty == 2)
					{
						tmp = (GameObject)Resources.Load("500/"+val.name);
						previousLevel.Add(val.name);
						placeholder.Add(((GameObject)GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity)).GetComponent<Placeholder>());
						placeholder[count].gameObject.GetComponent<SpriteRenderer>().enabled = false;
						count++;
						if(otherPieces == null && currentLevel.pieces[0].name != val.pieces[0].name)
						{
							otherPieces = val;
						}
					}
				}
			}
		}
		
		if(otherPieces != null)
		{
			initializeShapes(otherPieces);
		}
		
		for(int j = 0;j < placeholder.Count;j++)
		{
			foreach(Level val in loader.data.levels500)
			{
				if(placeholder[j].gameObject.name == (val.name+"(Clone)"))
				{
					for(int i = 0; i < val.pairs.Length; i++)
					{
						GameObject[] sps = getShapes(val.pairs[i].shapes);
						int[] angles = getAngles(val.pairs[i].angles);
						placeholder[j].fillChildInfo(val.pairs[i].piece,sps,angles,val.pairs[i].range);
					}
				}
			}
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
	
	public bool checkForLevelComplete()
	{
		foreach(GameObject val in shapes)
		{
			if(val != null)
			{
				val.GetComponent<Shape>().isPositionated = false;
			}
		}
		for(int i = 0;i < placeholder.Count;i++)
		{
			if(placeholder[i].isCorrect())
			{
				if(currLevel > 2 && currLevel < 5)
				{
					Shape[] temp = FindObjectsOfType<Shape>() as Shape[];
					for(int j = 0;j < temp.Length;j++)
					{
						if(temp[j].gameObject.name != placeholder[i].internalShapes[0].possibleAnswers[0].transform.parent.name)
						{
							input.onDragFinish -= temp[j].onRotationComplete;
							temp[j].destroy(0.5f);
						}
					}
				}
				input.selected = null;
				input.gameObject.SetActive(false);
				nextLevel();
				return true;
			}
		}
		return false;
	}
	
	public void onContinue()
	{
		int count = 0;
		if(!checkForLevelComplete())
		{
			for(int i = 0;i < shapes.Count;i++)
			{
				if(!shapes[i].GetComponent<Shape>().isPositionated)
				{
					shapes[i].GetComponent<ShakeTransform>().startAction(0.5f);
					count++;
				}
			}
			if(count == 0)
			{
				shapes[0].GetComponent<ShakeTransform>().startAction(0.5f);
			}
			if(audioSource && audioWrong)
			{
				audioSource.PlayOneShot(audioWrong);
			}
			return;
		}
	}

	protected void nextLevel()
	{
		excerciseFinished=true;
		AnalyticManager.instance.finsh("Construye", tType.ToString(),currentLevel.name);
		if(currLevel < 5 && tType == ETangramTypes.SAME_SHAPE)
		{
			currLevel++;
			notification.showToast("correcto",audioRight,2);
			continueBtn.GetComponent<Button>().interactable = false;
		}
		else if(fTypeAllShapes != null&& tType == ETangramTypes.ALL_SHAPES)
		{
			if(fTypeAllShapes.Count > 0)
			{
				notification.showToast("correcto",audioRight,2);
				continueBtn.GetComponent<Button>().interactable = false;
			}
		}
		else
		{
			GameObject.FindObjectOfType<FinishPopUp>().show();
		}
	}

	public void exitGame()
	{
		ScreenManager.instance.GoToScene("Construye");
	}

	void OnDisable() {
		if(!excerciseFinished)
		{
			AnalyticManager.instance.finsh("Construye", tType.ToString(),currentLevel.name,false);
		}
	}
}
