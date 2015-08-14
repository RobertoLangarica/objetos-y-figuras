using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TangramManager : MonoBehaviour 
{
	public static bool cannotRotate = false;

	public Button continueBtn;
	public GameObject[] shapes;
	public TangramInput input;
	
	protected Level currentLevel;
	protected int currLevel = 1;
	protected List<Placeholder> placeholder = new List<Placeholder>();
	protected XMLLoader loader;
	
	void Start ()
	{		
		loader = GameObject.FindObjectOfType<XMLLoader>();

		currentLevel = loader.data.levels500[selectLevel()];

		initializeShapes();
		continueBtn.gameObject.SetActive(false);

		
		input.onAnyDrag += onDrag;
		input.onDragFinish += onDragFinish;
		input.allowRotation = !cannotRotate;
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

	void initializeShapes()
	{
		GameObject go;
		//AnalyticManager.instance.startGame();
		//Imagenes
		Piece[] pieces = currentLevel.pieces;
		Pair[] pairs = currentLevel.pairs;
		
		shapes = new GameObject[pieces.Length];
		
		float min;
		float max;
		float posTemp = -0.03f;
		int rdmColor = 0;
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
				tempV3.x = float.Parse(pieces[i].scale.Substring(0,pieces[i].scale.IndexOf(',')));
				tempV3.y = float.Parse(pieces[i].scale.Substring(pieces[i].scale.IndexOf(',')+1));
			}
			else
			{
				tempV3.x = tempV3.y = float.Parse(pieces[i].scale);
			}
			go.transform.localScale = tempV3;
			shapes[i] = go;
			
			//Que la figura no se ponga en una rotacion invalida
			Shape sp = go.GetComponent<Shape>();
			sp.name = pieces[i].name;
			sp.onRotationComplete();
			input.onDragFinish += sp.onRotationComplete;
			sp.GetComponent<SpriteRenderer>().sortingOrder = Shape.sort+i;
			Shape.sort = Shape.sort +i;
			
			go.transform.localPosition = new Vector3 (go.transform.localPosition.x,go.transform.localPosition.y,posTemp);
			posTemp -= .01f;

			rdmColor = Random.Range(0,8);
			go.GetComponent<BaseShape>().color = (BaseShape.EShapeColor)rdmColor;
		}
		
		//placeholder
		GameObject tmp = (GameObject)Resources.Load("500/"+currentLevel.name);
		placeholder.Add(((GameObject)GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity)).GetComponent<Placeholder>());
		
		//Inicializamos los valores para el placeholder
		initializePlaceholder();
	}

	protected int selectLevel()
	{
		int ndx = -1;
		if(currLevel < 5)
		{
			List<Level> selectable = new List<Level>(loader.data.levels500);
			for(int i = 0;i < selectable.Count;i++)
			{
				if(selectable[i].difficulty == 2)
				{
					selectable.RemoveAt(i);
					i--;
				}
			}
			ndx = Random.Range(0,selectable.Count-1);
		}
		else
		{
			List<Level> selectable = new List<Level>(loader.data.levels500);
			for(int i = 0;i < selectable.Count;i++)
			{
				if(selectable[i].difficulty == 1)
				{
					selectable.RemoveAt(i);
					i--;
				}
			}
			ndx = Random.Range(0,selectable.Count-1);
		}
		return ndx;
	}

	protected void initializePlaceholder()
	{
		Pair[] pairs = currentLevel.pairs;
		GameObject tmp = null;
		int count = 1;
		
		for(int i = 0; i < pairs.Length; i++)
		{
			GameObject[] sps = getShapes(pairs[i].shapes);
			int[] angles = getAngles(pairs[i].angles);

			if(currLevel > 2 && currLevel < 5)
			{
				foreach(Level val in loader.data.levels500)
				{
					if(val.fType == currentLevel.fType && !val.Equals(currentLevel))
					{
						tmp = (GameObject)Resources.Load("500/"+val.name);
						placeholder.Add(((GameObject)GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity)).GetComponent<Placeholder>());
						placeholder[count].gameObject.GetComponent<SpriteRenderer>().enabled = false;
						placeholder[count].fillChildInfo(pairs[i].piece,sps,angles,pairs[i].range);
						count++;
					}
				}
			}
			placeholder[0].fillChildInfo(pairs[i].piece,sps,angles,pairs[i].range);
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
		for(int i = 0;i < placeholder.Count;i++)
		{
			if(placeholder[i].isCorrect())
			{
				if(currLevel > 2 && currLevel < 5)
				{
				}
				continueBtn.gameObject.SetActive(true);
			}
		}
	}
	
	public void onContinue()
	{
	}

	public void exitGame()
	{
		ScreenManager.instance.GoToScene("MainMenu");
	}
}
