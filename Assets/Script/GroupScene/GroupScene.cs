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
	public AudioSource audioSource; 
	public AudioClip audioWrong;
	public AudioClip audioRight;
	public AudioClip finalAudio;
	public GameObject pencil;
	public List<GameObject> kindsOfShapes = new List<GameObject>();

	protected int totalGroups = 0;
	protected int currentLevel = 0;
	protected int maxLevel = 0;
	protected int shapesCount = 0;
	protected bool lastDetected = false;
	protected GameObject continueBtn;
	protected Rect containerRect;
	protected XMLLoader loader;
	protected Notification notification;
	protected GameObject[] currentShapes;
	protected GameObject[] containerImg;
	protected int[] availableColors;
	protected int[] previousShapes;
	protected float[] availableScales;
	protected Rect[] containersInRect;
	protected bool excerciseFinished;

	void Start()
	{
		loader = GameObject.FindObjectOfType<XMLLoader>();

		availableColors = new int[9]{0,1,2,3,4,5,6,7,8};

		availableScales = new float[]{0.1f,0.3f,0.5f,0.7f,1};

		GameObject tempGo = GameObject.Find("Containers");
		Vector3[] tempV3 = new Vector3[4];
		tempGo.GetComponent<RectTransform>().GetWorldCorners(tempV3);
		containerRect = new Rect(tempV3[0].x,tempV3[0].y,(tempV3[2].x-tempV3[0].x),(tempV3[2].y-tempV3[0].y));

		continueBtn = GameObject.Find("BtnCalificar");

		notification = GameObject.Find("Notification").GetComponent<Notification>();
		notification.onClose += nextLevel;

		Question tempQ = FindObjectOfType<Question>() as Question;
		GameObject.Find("ClueBtn").GetComponent<Button>().onClick.AddListener(() => tempQ.questionSound(typeOfGroup.ToString()));

		if(typeOfGroup != EGroups.FREE)
		{
			pencil.SetActive(false);
		}
		AnalyticManager.instance.startGame();
		startLevel();
	}

	protected void startLevel()
	{
		excerciseFinished = false;
		continueBtn.GetComponent<Button>().interactable = true;
		if(currentShapes != null)
		{
			for(int i = 0;i < currentShapes.Length;i++)
			{
				currentShapes[i].GetComponent<GroupFigure>().destroy(0.5f);
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
			Rotate(currentShapes[i]);
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
		Vector2[] anchors = new Vector2[1];
		/*float nWidth = 0;
		float nHeight = 0;
		Vector3[] nPos = new Vector3[totalGroups];
		containersInRect = new Rect[totalGroups];*/

		switch(totalGroups)
		{
		case(1):
		{
			anchors = new Vector2[2];
			anchors[0] = new Vector2(0.05f,0.05f);
			anchors[1] = new Vector2(0.95f,0.95f);
		}
			break;
		case(2):
		{
			anchors = new Vector2[4];
			anchors[0] = new Vector2(0.05f,0.05f);
			anchors[1] = new Vector2(0.49f,0.95f);
			anchors[2] = new Vector2(0.51f,0.05f);
			anchors[3] = new Vector2(0.95f,0.95f);
		}
			break;
		case(3):
		{
			anchors = new Vector2[6];
			anchors[0] = new Vector2(0.01f,0.05f);
			anchors[1] = new Vector2(0.3233f,0.95f);
			anchors[2] = new Vector2(0.3433f,0.05f);
			anchors[3] = new Vector2(0.6566f,0.95f);
			anchors[4] = new Vector2(0.6766f,0.05f);
			anchors[5] = new Vector2(0.9899f,0.95f);
		}
			break;
		case(4):
		{
			anchors = new Vector2[8];
			anchors[0] = new Vector2(0.05f,0.05f);
			anchors[1] = new Vector2(0.49f,0.48f);
			anchors[2] = new Vector2(0.51f,0.05f);
			anchors[3] = new Vector2(0.95f,0.48f);
			anchors[4] = new Vector2(0.05f,0.52f);
			anchors[5] = new Vector2(0.49f,0.95f);
			anchors[6] = new Vector2(0.51f,0.52f);
			anchors[7] = new Vector2(0.95f,0.95f);
		}
			break;
		case(6):
		{
			anchors = new Vector2[12];
			anchors[0] = new Vector2(0.01f,0.05f);
			anchors[1] = new Vector2(0.3233f,0.48f);
			anchors[2] = new Vector2(0.3433f,0.05f);
			anchors[3] = new Vector2(0.6566f,0.48f);
			anchors[4] = new Vector2(0.6766f,0.05f);
			anchors[5] = new Vector2(0.9899f,0.48f);
			anchors[6] = new Vector2(0.01f,0.52f);
			anchors[7] = new Vector2(0.3233f,0.95f);
			anchors[8] = new Vector2(0.3433f,0.52f);
			anchors[9] = new Vector2(0.6566f,0.95f);
			anchors[10] = new Vector2(0.6766f,0.52f);
			anchors[11] = new Vector2(0.9899f,0.95f);
		}
			break;
		}

		containersInRect = new Rect[totalGroups];
		containerImg = new GameObject[totalGroups];
		for(int i = 0,j = 0;i < totalGroups;i++,j += 2)
		{
			GameObject tempGo = GameObject.Instantiate(containerGo.gameObject) as GameObject;
			tempGo.name = i.ToString();
			tempGo.GetComponent<RectTransform>().anchorMin = anchors[j];
			tempGo.GetComponent<RectTransform>().anchorMax = anchors[j+1];
			tempGo.GetComponent<RectTransform>().SetParent(GameObject.Find("Containers").transform,false);
			Vector3[] tempV3 = new Vector3[4];
			tempGo.GetComponent<RectTransform>().GetWorldCorners(tempV3);
			containersInRect[i] = new Rect(tempV3[0].x,tempV3[0].y,(tempV3[2].x-tempV3[0].x),(tempV3[2].y-tempV3[0].y));
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
		}
		if (wasLastPiece () && typeOfGroup != EGroups.FREE) 
		{
			lastDetected = true;
			verifyExcersice();
		}
	}

	protected void shapeFeedback(bool isCorrect)
	{
		if(isCorrect)
		{
			notification.showToast("correcto",audioRight,2);
			continueBtn.GetComponent<Button>().interactable = false;
		}
		else
		{
			if(audioSource && audioWrong)
			{
				audioSource.PlayOneShot(audioWrong);
			}
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
		bool lose = false;
		bool firstCheck = true;
		int tempIndx = -2;
		int bigGroup = 0;
		Vector2 currPos = Vector2.zero;
		List<int> calif = new List<int>();

		if(typeOfGroup != EGroups.FREE)
		{
			for (int i = 0;i < containersInRect.Length;i++) 
			{
				calif.Clear();
				firstCheck = true;
				for(int j = 0;j < currentShapes.Length;j++)
				{
					currPos = new Vector2(currentShapes[j].transform.localPosition.x,currentShapes[j].transform.localPosition.y);
					if(containersInRect[i].Contains(currPos))
					{
						if(firstCheck)
						{
							tempIndx = calif.IndexOf(currentShapes[j].GetComponent<GroupFigure>().group);
							if(tempIndx == -1)
							{
								calif.Add(currentShapes[j].GetComponent<GroupFigure>().group);
								calif.Add(10);
								if(calif.Count > 2)lose = true;
							}
							else
							{
								calif[tempIndx+1] = calif[tempIndx+1] +10;
							}
						}
						else
						{
							if(currentShapes[j].GetComponent<GroupFigure>().group != bigGroup && !lastDetected)
							{
								currentShapes[j].GetComponent<ShakeTransform>().startAction(0.5f);
								lose = true;
							}
						}
					}
					if(j == currentShapes.Length-1 && firstCheck)
					{
						firstCheck = false;
						j = -1;
						for(int k = 1;k < calif.Count;k += 2)
						{
							if(calif[k] > bigGroup)bigGroup = calif[k];
						}
						if(calif.Count > 0)
						{
							bigGroup = calif[calif.IndexOf(bigGroup)-1];
						}
					}
				}
			}
		}
		for(int j = 0;j < currentShapes.Length;j++)
		{
			currPos = new Vector2(currentShapes[j].transform.localPosition.x,currentShapes[j].transform.localPosition.y);
			if(!containerRect.Contains(currPos) && !lastDetected)
			{
				currentShapes[j].GetComponent<ShakeTransform>().startAction(0.5f);
				lose = true;
			}
		}
		lastDetected = false;
		if(lose)
		{
			shapeFeedback(false);
			return;
		}
		shapeFeedback(true);
	}

	protected void nextLevel()
	{
		excerciseFinished=true;
		AnalyticManager.instance.finsh("Construye", typeOfGroup.ToString(),currentLevel.ToString());
		if (currentLevel < (maxLevel-1)) 
		{
			currentLevel++;
			startLevel();
		}
		else 
		{
			GameObject.FindObjectOfType<FinishPopUp>().show();
		}
	}

	protected void Rotate (GameObject toRotate) 
	{
		float randvalue = Random.Range(0,2);
		randvalue = Random.Range(0,360);
		toRotate.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x,this.transform.eulerAngles.y,randvalue);
	}
	void OnDisable() {
		if(!excerciseFinished)
		{
			AnalyticManager.instance.finsh("Construye", typeOfGroup.ToString(),currentLevel.ToString(),false);
		}
	}
}