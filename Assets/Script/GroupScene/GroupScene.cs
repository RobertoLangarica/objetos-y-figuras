using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum EGroups
{
	SHAPE,
	COLOR,
	SIZE,
	FREE
}

public class GroupScene : MonoBehaviour 
{
	public static EGroups typeOfGroup;

	public int groupSize = 3;
	public Image containerGo;
	public List<GameObject> kindsOfShapes = new List<GameObject>();
	//public EGroups type;

	protected int totalGroups = 0;
	protected int currentLevel = 0;
	protected int maxLevel = 0;
	protected int shapesCount = 0;
	protected Rect containerRect;
	protected XMLLoader loader;
	protected GameObject buttonGo;
	protected GameObject[] currentShapes;
	protected GameObject[] containerImg;
	protected int[] availableColors;
	protected int[] previousShapes;
	protected float[] availableScales;
	protected Rect[] containersInRect;

	void Start()
	{
		loader = GameObject.FindObjectOfType<XMLLoader>();
		buttonGo = GameObject.Find("Calificar");
		buttonGo.SetActive (false);

		availableColors = new int[9]{0,1,2,3,4,5,6,7,8};

		availableScales = new float[]{0.1f,0.3f,0.5f,0.7f,1};

		
		GameObject tempGo = GameObject.Find("Containers");
		Vector3[] tempV3 = new Vector3[4];
		tempGo.GetComponent<RectTransform>().GetWorldCorners(tempV3);
		containerRect = new Rect(tempV3[0].x,tempV3[0].y,(tempV3[2].x-tempV3[0].x),(tempV3[2].y-tempV3[0].y));

		startLevel();
	}

	protected void startLevel()
	{
		if(currentShapes != null)
		{
			for(int i = 0;i < currentShapes.Length;i++)
			{
				Destroy(currentShapes[i]);
			}
			for(int i = 0;i < containerImg.Length;i++)
			{
				Destroy(containerImg[i]);
			}
		}
		readFromLoader();

		generateShapes (shapesCount);
		//totalGroups = 6;
		generateContainers();

		modifyShapes ();
	}

	protected void readFromLoader()
	{
		switch(typeOfGroup)
		{
		case(EGroups.COLOR):
		{
			maxLevel = loader.data.gLevel.byColor.Length;
			totalGroups = loader.data.gLevel.byColor[currentLevel].totalGroups;
			shapesCount = loader.data.gLevel.byColor[currentLevel].shapeNum;
			currentShapes = new GameObject[groupSize*totalGroups];
		}
			break;
		case(EGroups.SHAPE):
		{
			maxLevel = loader.data.gLevel.byShape.Length;
			totalGroups = loader.data.gLevel.byShape[currentLevel].totalGroups;
			shapesCount = loader.data.gLevel.byShape[currentLevel].shapeNum;
			currentShapes = new GameObject[groupSize*totalGroups];
		}
			break;
		case(EGroups.SIZE):
		{
			maxLevel = loader.data.gLevel.bySize.Length;
			totalGroups = loader.data.gLevel.bySize[currentLevel].totalGroups;
			shapesCount = loader.data.gLevel.bySize[currentLevel].shapeNum;
			currentShapes = new GameObject[groupSize*totalGroups];
		}
			break;
		case(EGroups.FREE):
		{
			maxLevel = loader.data.gLevel.freeStyle.Length;
			totalGroups = loader.data.gLevel.freeStyle[currentLevel].totalGroups;
			shapesCount = loader.data.gLevel.freeStyle[currentLevel].shapeNum;
			currentShapes = new GameObject[groupSize*shapesCount];
		}
			break;
		}
	}

	protected void generateShapes(int quantity)
	{
		List<int> ndxs = new List<int>();
		for (int i = 0; i < kindsOfShapes.Count; i++) 
		{
			ndxs.Add(i);
		}
		List<int> yRnd = new List<int>();
		for (int i = 1; i < (currentShapes.Length+1); i++) 
		{
			yRnd.Add(i);
		}

		if(previousShapes != null)
		{
			for(int i = 0;i < previousShapes.Length;i++)
			{

				ndxs.RemoveAt(ndxs.IndexOf(previousShapes[i]));
			}
		}

		int count = 0;
		int rmdIdx = 0;
		int group = 0;

		GameObject tempGo = GameObject.Find("Start");
		Vector3[] tempV3 = new Vector3[4];
		tempGo.GetComponent<RectTransform>().GetWorldCorners(tempV3);
		float aThird = ((tempV3[2].x - tempV3[0].x)*0.333f);
		float yDif = (tempV3[2].y - tempV3[0].y)/(currentShapes.Length+1);
		float yPos = tempV3[0].y;
		previousShapes = new int[quantity];

		for(int i = 0;i < currentShapes.Length;i++)
		{
			if((i%groupSize) == 0)
			{
				group++;
				if(count < quantity)
				{
					rmdIdx = ndxs[Random.Range(0,ndxs.Count-1)];
					ndxs.RemoveAt(ndxs.IndexOf(rmdIdx));
					previousShapes[count] = rmdIdx;
					count++;
				}
			}
			Vector3 randPos = Vector3.zero;
			randPos.x = Random.Range(tempV3[0].x+aThird,tempV3[2].x+(aThird*2));
			randPos.z = 0;
			int rdmDif = Random.Range(0,yRnd.Count-1);
			randPos.y = yPos+(yDif*(yRnd[rdmDif]));
			yRnd.RemoveAt(rdmDif);
			currentShapes[i] = GameObject.Instantiate(kindsOfShapes[rmdIdx],randPos,Quaternion.identity) as GameObject;
			currentShapes[i].GetComponent<GroupFigure>().group = group;
			currentShapes[i].GetComponent<GroupFigure>().finishAction = evaluateShape;
		}
	}

	protected void modifyShapes()
	{
		switch (typeOfGroup) 
		{
		case(EGroups.SHAPE):
		{
			int rdmColor = Random.Range(0,7);
			for(int i = 0;i < currentShapes.Length;i++)
			{
				currentShapes[i].GetComponent<GroupFigure>().color = (BaseShape.EShapeColor)rdmColor;
				currentShapes[i].GetComponent<GroupFigure>().size = BaseShape.EShapeSize.SIZE2;
			}
		}
			break;
		case(EGroups.SIZE):
		{
			int group = -1;
			int rdmColor = Random.Range(0,7);
			int currSize = 0;
			List<int> sizeArr = new List<int>(){0,1,2,3,4};
			for(int i = 0;i < currentShapes.Length;i++)
			{
				currentShapes[i].GetComponent<GroupFigure>().color = (BaseShape.EShapeColor)rdmColor;
				if(group == -1 || group != currentShapes[i].GetComponent<GroupFigure>().group)
				{
					currSize = sizeArr[Random.Range(0,sizeArr.Count-1)];
					sizeArr.RemoveAt(sizeArr.IndexOf(currSize));
					group = currentShapes[i].GetComponent<GroupFigure>().group;
				}
				currentShapes[i].GetComponent<GroupFigure>().size = (BaseShape.EShapeSize)currSize;
			}
		}
			break;
		case(EGroups.COLOR):
		{
			int group = -1;
			int rdmColor = 0;
			List<int> sizeArr = new List<int>(){0,1,2,3,4,5,6,7,8};
			for(int i = 0;i < currentShapes.Length;i++)
			{
				if(group == -1 || group != currentShapes[i].GetComponent<GroupFigure>().group)
				{
					rdmColor = sizeArr[Random.Range(0,sizeArr.Count-1)];
					sizeArr.RemoveAt(sizeArr.IndexOf(rdmColor));
					group = currentShapes[i].GetComponent<GroupFigure>().group;
				}
				currentShapes[i].GetComponent<GroupFigure>().size = BaseShape.EShapeSize.SIZE2;
				currentShapes[i].GetComponent<GroupFigure>().color = (BaseShape.EShapeColor)rdmColor;
			}
		}
			break;
		case(EGroups.FREE):
		{
			int askedColors = loader.data.gLevel.freeStyle[currentLevel].colorNum;
			int askedSizes = loader.data.gLevel.freeStyle[currentLevel].sizeNum;
			int tempInt = 0;
			int[] selectColor = new int[askedColors];
			int[] selectSize = new int[askedSizes];
			List<int> sizeArr = new List<int>(){0,1,2,3,4};
			List<int> colorArr = new List<int>(){0,1,2,3,4,5,6,7,8};

			for(int i = 0;i < selectColor.Length;i++)
			{
				tempInt = colorArr[Random.Range(0,colorArr.Count-1)];
				colorArr.RemoveAt(colorArr.IndexOf(tempInt));
				selectColor[i] = tempInt;
			}

			for(int i = 0;i < selectSize.Length;i++)
			{
				tempInt = sizeArr[Random.Range(0,sizeArr.Count-1)];
				sizeArr.RemoveAt(sizeArr.IndexOf(tempInt));
				selectSize[i] = tempInt;
			}

			for(int i = 0,j = 0,k = 0;i < currentShapes.Length;i++,j++,k++)
			{
				currentShapes[i].GetComponent<GroupFigure>().size = (BaseShape.EShapeSize)selectSize[j];
				currentShapes[i].GetComponent<GroupFigure>().color = (BaseShape.EShapeColor)selectColor[k];
				if(j == (askedSizes-1))j = -1;
				if(k == (askedColors-1))k = -1;
			}
		}
			break;
		}
	}

	protected void generateContainers()
	{
		float nWidth = 0;
		float nHeight = 0;
		Vector3[] nPos = new Vector3[totalGroups];
		containersInRect = new Rect[totalGroups];

		switch(totalGroups)
		{
		case(1):
		{
			nWidth = ((containerRect.width * 100)/(containerGo.rectTransform.rect.width))*0.01f;
			nHeight = ((containerRect.height *100)/(containerGo.rectTransform.rect.height))*0.01f;
			nPos[0] = new Vector3(containerRect.x+(containerRect.width * 0.5f),containerRect.y+(containerRect.height * 0.5f),0);
			containersInRect[0] = new Rect(containerRect.x,containerRect.y,containerRect.width,containerRect.height);
		}
			break;
		case(2):
		{
			nWidth = (((containerRect.width * 0.5f)*100)/(containerGo.rectTransform.rect.width))*0.01f;
			nHeight = ((containerRect.height *100)/(containerGo.rectTransform.rect.height))*0.01f;
			nPos[0] = new Vector3(containerRect.x+(containerRect.width * 0.25f),containerRect.y+(containerRect.height * 0.5f),0);
			nPos[1] = new Vector3(containerRect.x+(containerRect.width * 0.75f),containerRect.y+(containerRect.height * 0.5f),0);
			containersInRect[0] = new Rect(containerRect.x,containerRect.y,containerRect.width*0.5f,containerRect.height);
			containersInRect[1] = new Rect(containerRect.x+(containerRect.width*0.5f),containerRect.y,containerRect.width*0.5f,containerRect.height);
		}
			break;
		case(3):
		{
			nWidth = (((containerRect.width * 0.333f)*100)/(containerGo.rectTransform.rect.width))*0.01f;
			nHeight = ((containerRect.height *100)/(containerGo.rectTransform.rect.height))*0.01f;
			nPos[0] = new Vector3(containerRect.x+(containerRect.width * 0.1666f),containerRect.y+(containerRect.height * 0.5f),0);
			nPos[1] = new Vector3(containerRect.x+(containerRect.width * 0.5f),containerRect.y+(containerRect.height * 0.5f),0);
			nPos[2] = new Vector3(containerRect.x+(containerRect.width * 0.83333f),containerRect.y+(containerRect.height * 0.5f),0);
			containersInRect[0] = new Rect(containerRect.x,containerRect.y,containerRect.width*0.3333f,containerRect.height);
			containersInRect[1] = new Rect(containerRect.x+(containerRect.width*0.3333f),containerRect.y,containerRect.width*0.3333f,containerRect.height);
			containersInRect[2] = new Rect(containerRect.x+(containerRect.width*0.6666f),containerRect.y,containerRect.width*0.3333f,containerRect.height);
		}
			break;
		case(4):
		{
			nWidth = (((containerRect.width * 0.5f)*100)/(containerGo.rectTransform.rect.width))*0.01f;
			nHeight = (((containerRect.height * 0.5f) *100)/(containerGo.rectTransform.rect.height))*0.01f;
			nPos[0] = new Vector3(containerRect.x+(containerRect.width * 0.25f),containerRect.y+(containerRect.height * 0.25f),0);
			nPos[1] = new Vector3(containerRect.x+(containerRect.width * 0.75f),containerRect.y+(containerRect.height * 0.25f),0);
			nPos[2] = new Vector3(containerRect.x+(containerRect.width * 0.25f),containerRect.y+(containerRect.height * 0.75f),0);
			nPos[3] = new Vector3(containerRect.x+(containerRect.width * 0.75f),containerRect.y+(containerRect.height * 0.75f),0);
			containersInRect[0] = new Rect(containerRect.x,containerRect.y,containerRect.width*0.5f,containerRect.height*0.5f);
			containersInRect[1] = new Rect(containerRect.x+(containerRect.width*0.5f),containerRect.y,containerRect.width*0.5f,containerRect.height*0.5f);
			containersInRect[2] = new Rect(containerRect.x,containerRect.y+(containerRect.height*0.5f),containerRect.width*0.5f,containerRect.height*0.5f);
			containersInRect[3] = new Rect(containerRect.x+(containerRect.width*0.5f),containerRect.y+(containerRect.height*0.5f),containerRect.width*0.5f,containerRect.height*0.5f);
		}
			break;
		case(6):
		{
			nWidth = (((containerRect.width * 0.333f)*100)/(containerGo.rectTransform.rect.width))*0.01f;
			nHeight = (((containerRect.height * 0.5f) *100)/(containerGo.rectTransform.rect.height))*0.01f;
			nPos[0] = new Vector3(containerRect.x+(containerRect.width * 0.1666f),containerRect.y+(containerRect.height * 0.25f),0);
			nPos[1] = new Vector3(containerRect.x+(containerRect.width * 0.5f),containerRect.y+(containerRect.height * 0.25f),0);
			nPos[2] = new Vector3(containerRect.x+(containerRect.width * 0.83333f),containerRect.y+(containerRect.height * 0.25f),0);
			nPos[3] = new Vector3(containerRect.x+(containerRect.width * 0.1666f),containerRect.y+(containerRect.height * 0.75f),0);
			nPos[4] = new Vector3(containerRect.x+(containerRect.width * 0.5f),containerRect.y+(containerRect.height * 0.75f),0);
			nPos[5] = new Vector3(containerRect.x+(containerRect.width * 0.83333f),containerRect.y+(containerRect.height * 0.75f),0);
			containersInRect[0] = new Rect(containerRect.x,containerRect.y,containerRect.width*0.3333f,containerRect.height*0.5f);
			containersInRect[1] = new Rect(containerRect.x+(containerRect.width*0.3333f),containerRect.y,containerRect.width*0.3333f,containerRect.height*0.5f);
			containersInRect[2] = new Rect(containerRect.x+(containerRect.width*0.6666f),containerRect.y,containerRect.width*0.3333f,containerRect.height*0.5f);
			containersInRect[3] = new Rect(containerRect.x,containerRect.y+(containerRect.height*0.5f),containerRect.width*0.3333f,containerRect.height*0.5f);
			containersInRect[4] = new Rect(containerRect.x+(containerRect.width*0.3333f),containerRect.y+(containerRect.height*0.5f),containerRect.width*0.3333f,containerRect.height*0.5f);
			containersInRect[5] = new Rect(containerRect.x+(containerRect.width*0.6666f),containerRect.y+(containerRect.height*0.5f),containerRect.width*0.3333f,containerRect.height*0.5f);
		}
			break;
		}

		containerImg = new GameObject[totalGroups];
		for(int i = 0;i < totalGroups;i++)
		{
			GameObject tempGo = GameObject.Instantiate(containerGo.gameObject) as GameObject;
			tempGo.name = i.ToString();
			tempGo.GetComponent<RectTransform>().localScale = new Vector3(nWidth,nHeight,1);
			tempGo.GetComponent<RectTransform>().position = nPos[i];
			tempGo.GetComponent<RectTransform>().SetParent(GameObject.Find("Containers").transform);
			containerImg[i] = tempGo;
		}
	}
	
	protected void evaluateShape(GameObject shape)
	{
		Vector2 currPos = new Vector2(shape.transform.localPosition.x,shape.transform.localPosition.y);
		List<int> evaluation = new List<int>();
		int ndx = 0;

		if(containerRect.Contains(currPos) && typeOfGroup != EGroups.FREE)
		{
			for(int i = 0;i < containersInRect.Length;i++)
			{
				if(containersInRect[i].Contains(currPos))
				{
					evaluation.Add(shape.GetComponent<GroupFigure>().group);
					evaluation.Add(10);
					for(int j = 0;j < currentShapes.Length;j++)
					{
						currPos = new Vector2(currentShapes[j].transform.localPosition.x,currentShapes[j].transform.localPosition.y);
						if(!currentShapes[j].Equals(shape) && containersInRect[i].Contains(currPos))
						{
							ndx = evaluation.IndexOf(currentShapes[j].GetComponent<GroupFigure>().group);
							if(ndx != -1)
							{
								evaluation[ndx+1] = evaluation[ndx+1] + 10;
							}
							else
							{
								evaluation.Add(currentShapes[j].GetComponent<GroupFigure>().group);
								evaluation.Add(10);
							}
						}
					}
					break;
				}
			}
			int max = 0;
			bool tie = false;
			ndx = 0;
			for(int i = 1;i < evaluation.Count;i+=2)
			{
				if(evaluation[i] == max)
				{
					tie = true;
				}
				if(evaluation[i] > max)
				{
					max = evaluation[i];
					ndx = evaluation[i-1];
				}
			}
			if(shape.GetComponent<GroupFigure>().group == ndx && tie == false)
			{
				shapeFeedback(true);
			}
			else
			{
				shapeFeedback(false);
			}
		}
		if (wasLastPiece ()) 
		{
			buttonGo.SetActive(true);
		}
	}

	protected void shapeFeedback(bool isCorrect)
	{
		if(isCorrect)
		{
			//Debug.Log ("Correcto");
			//GameObject.FindObjectOfType<Notification>().questionSound("0");
		}
		else
		{
			//Debug.Log ("Incorrecto");
		}
	}

	protected bool wasLastPiece()
	{
		Vector2 currPos = Vector2.zero;
		for (int i = 0; i < currentShapes.Length; i++) 
		{
			currPos = new Vector2(currentShapes[i].transform.localPosition.x,currentShapes[i].transform.localPosition.y);
			if(!containerRect.Contains(currPos))
			{
				return false;
			}
		}
		return true;
	}

	public void verifyExcersice()
	{
		int contGroup = -1;
		Vector2 currPos = Vector2.zero;

		if(typeOfGroup != EGroups.FREE)
		{
			for (int i = 0;i < containersInRect.Length;i++) 
			{
				for(int j = 0;j < currentShapes.Length;j++)
				{
					currPos = new Vector2(currentShapes[j].transform.localPosition.x,currentShapes[j].transform.localPosition.y);
					if(containersInRect[i].Contains(currPos))
					{
						if(contGroup == -1)
						{
							contGroup = currentShapes[j].GetComponent<GroupFigure>().group;
						}
						else if(currentShapes[j].GetComponent<GroupFigure>().group != contGroup)
						{
							Debug.Log ("Ejercicio mal");
							return;
						}
					}
				}
				contGroup = -1;
			}
		}
		buttonGo.SetActive(false);
		nextLevel();
	}

	protected void nextLevel()
	{
		if (currentLevel < (maxLevel-1)) 
		{
			currentLevel++;
			startLevel();
		}
		else 
		{
			Debug.Log ("Go Bak");
			ScreenManager.instance.GoToScene("Agrupa");
		}
	}
}