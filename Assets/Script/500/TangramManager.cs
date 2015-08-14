﻿using UnityEngine;
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
	protected Level previousLevel;
	protected int currLevel = 1;
	protected List<Placeholder> placeholder = new List<Placeholder>();
	protected XMLLoader loader;
	
	void Start ()
	{		
		loader = GameObject.FindObjectOfType<XMLLoader>();

		input.onAnyDrag += onDrag;
		input.onDragFinish += onDragFinish;
		input.allowRotation = !cannotRotate;

		levelStart();
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

	void levelStart()
	{
		if(currentLevel != null)
		{
			previousLevel = currentLevel;
		}

		currentLevel = loader.data.levels500[selectLevel()];
		
		initializeShapes(currentLevel);
		
		GameObject tmp = (GameObject)Resources.Load("500/"+currentLevel.name);
		placeholder.Add(((GameObject)GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity)).GetComponent<Placeholder>());
		
		initializePlaceholder();
		
		continueBtn.gameObject.SetActive(false);
		input.gameObject.SetActive(true);
	}

	void initializeShapes(Level initLevel)
	{
		GameObject go;
		//AnalyticManager.instance.startGame();
		//Imagenes
		Piece[] pieces = initLevel.pieces;
		Pair[] pairs = initLevel.pairs;
		
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
			//go.transform.FindChild("New Sprite").localScale = tempV3;
			go.transform.localScale = tempV3;

			if(pieces[i].scaleCiclre != null)
			{
				tempV3.x = float.Parse(pieces[i].scaleCiclre.Substring(0,pieces[i].scaleCiclre.IndexOf(',')));
				tempV3.y = float.Parse(pieces[i].scaleCiclre.Substring(pieces[i].scaleCiclre.IndexOf(',')+1));
			}
			go.transform.FindChild("rotate").localScale = tempV3;

			if(initLevel.radius > 0)
			{
				go.GetComponent<CircleCollider2D>().radius = initLevel.radius;
			}

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
	}

	protected int selectLevel()
	{
		int ndx = -1;
		List<Level> selectable = new List<Level>(loader.data.levels500);

		if(previousLevel != null)
		{
			selectable.Remove(previousLevel);
		}

		if(currLevel < 3)
		{
			for(int i = 0;i < selectable.Count;i++)
			{
				if(selectable[i].difficulty == 3)
				{
					selectable.RemoveAt(i);
					i--;
				}
			}
			ndx = Random.Range(0,selectable.Count-1);
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
			ndx = Random.Range(0,selectable.Count-1);
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
			ndx = Random.Range(0,selectable.Count-1);
		}
		return 4;
	}

	protected void initializePlaceholder()
	{
		Pair[] pairs = currentLevel.pairs;
		GameObject tmp = null;
		int count = 1;
		Level otherPieces = null;

		if(currLevel < 5)
		{
			foreach(Level val in loader.data.levels500)
			{
				if(val.fType == currentLevel.fType && !val.Equals(currentLevel))
				{
					if(currLevel < 3 && currentLevel.pieces[0].name == val.pieces[0].name)
					{
						tmp = (GameObject)Resources.Load("500/"+val.name);
						placeholder.Add(((GameObject)GameObject.Instantiate(tmp,Vector3.zero,Quaternion.identity)).GetComponent<Placeholder>());
						placeholder[count].gameObject.GetComponent<SpriteRenderer>().enabled = false;
						count++;
					}
					if(currLevel > 2 && val.difficulty == 2)
					{
						tmp = (GameObject)Resources.Load("500/"+val.name);
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

		for(int i = 0; i < pairs.Length; i++)
		{
			GameObject[] sps = getShapes(pairs[i].shapes);
			int[] angles = getAngles(pairs[i].angles);
			for(int j = 0;j < placeholder.Count;j++)
			{
				placeholder[j].fillChildInfo(pairs[i].piece,sps,angles,pairs[i].range);
			}
		}

		if(otherPieces != null)
		{
			initializeShapes(otherPieces);
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
					for(int j = 0;j < placeholder.Count;j++)
					{
						if(j != i && placeholder[j].internalShapes[0].possibleAnswers[0] != null)
						{
							if(!placeholder[j].internalShapes[0].possibleAnswers[0].Equals(placeholder[i].internalShapes[0].possibleAnswers[0]))
							{
								for(int k = placeholder[j].internalShapes[0].possibleAnswers.Count-1;k > -1 ;k--)
								{
									Destroy(placeholder[i]);
								}
							}
						}
					}
				}
				continueBtn.gameObject.SetActive(true);
				input.selected = null;
				input.gameObject.SetActive(false);
				break;
			}
		}
	}
	
	public void onContinue()
	{
		if(currLevel < 5)
		{
			currLevel++;
			for(int i = 0;i < shapes.Length;i++)
			{
				Destroy(shapes[i]);
			}
			for(int i = placeholder.Count-1;i > -1 ;i--)
			{
				Destroy(placeholder[i]);
			}
			levelStart();
		}
		else
		{
			exitGame();
		}
	}

	public void exitGame()
	{
		ScreenManager.instance.GoToScene("MainMenu");
	}
}
