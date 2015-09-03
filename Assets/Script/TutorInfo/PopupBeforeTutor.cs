using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupBeforeTutor : MonoBehaviour {

	public Image background;
	public GameObject content;
	public Text instructions;
	public RectTransform shapeContainer;
	public GameObject[] shapes;
	public BaseShape.EShapeColor[] colors;
	public string[] colorDisplayNames;
	public string screenToShow = "TutorInfo";

	protected GameObject[] displayShapes;


	//para la aniamcion
	protected delegate void OnAnimationEnd();
	protected OnAnimationEnd onEnd;
	
	protected bool showing;
	protected Vector3 currentScale;
	protected Vector3 initialScale;
	protected float inverseTime;
	protected float showElapsed;
	protected Vector3 destinationScale;
	protected float percent;

	void Start()
	{
		displayShapes = new GameObject[4];
	}

	// Use this for initialization
	void OnEnable () 
	{
		content.transform.localScale = Vector3.zero;

		if(displayShapes != null)
		{
			int r = Random.Range(0,shapes.Length);
			int color = Random.Range(0,colors.Length);
			int clicks = Random.Range(2,6);
			int correct = Random.Range(0,4);

			GameObject tmpCorrect = shapes[r];

			for(int i = 0; i < 4; i++)
			{
				if(correct == i)
				{
					displayShapes[i] = GameObject.Instantiate(tmpCorrect) as GameObject;
					displayShapes[i].GetComponent<BaseShape>().color = colors[color];
					BeforeTutorPopupShape bfs = displayShapes[i].GetComponent<BeforeTutorPopupShape>();
					bfs.clicksForTrigger = clicks;
					bfs.onTrigger += advance;
				}
				else
				{
					displayShapes[i] = GameObject.Instantiate(shapes[Random.Range(0,shapes.Length)]) as GameObject;

					int c = -1;

					while(c == -1)
					{
						c = Random.Range(0,colors.Length);
						c = c == color ? -1 : c;
					}
					displayShapes[i].GetComponent<BaseShape>().color = colors[c];
					BeforeTutorPopupShape bfs = displayShapes[i].GetComponent<BeforeTutorPopupShape>();
					bfs.clicksForTrigger = -1;
				}

				displayShapes[i].transform.localScale = Vector3.zero;
				displayShapes[i].transform.SetParent(shapeContainer.transform,false);

			}

			string text = "";

			#if (UNITY_STANDALONE || UNITY_EDITOR)
			text = "Haz click "+clicks+" veces en el ";
			#else
			text = "Toca "+clicks+" veces el ";
			#endif

			text += shapes[r].GetComponent<BeforeTutorPopupShape>().displayName + " ";
			text += colorDisplayNames[color]+".";

			instructions.text = text;
			show(true);
		}


	}

	void Update () 
	{
		if(showing)
		{
			percent = showElapsed*inverseTime;
			currentScale.x = Mathf.SmoothStep(initialScale.x,destinationScale.x,percent);
			currentScale.y = currentScale.x;
			currentScale.z = currentScale.x;

			content.transform.localScale = currentScale;

			if(currentScale == destinationScale)
			{
				showing = false;
				onEnd();
			}

			showElapsed+= Time.deltaTime;
		}
	}
	
	public void show(bool shouldShow)
	{
		if(showing){return;}

		showing = true;
		initialScale = content.transform.localScale;
		currentScale = initialScale;
		showElapsed = 0;
		percent = 0;

		onEnd -= completeClose;
		onEnd -= completeIntro;

		if(shouldShow)
		{
			
			StartCoroutine("closePopUp");
			inverseTime = 1/0.25f;
			destinationScale = Vector3.one;
			onEnd += completeIntro;
		}
		else
		{
			inverseTime = 1/0.15f;
			destinationScale = Vector3.zero;
			onEnd = completeClose;
		}
	}

	protected void completeIntro()
	{
		foreach(GameObject go in displayShapes)
		{
			go.GetComponent<BeforeTutorPopupShape>().show();
		}
	}

	protected void completeClose()
	{
		foreach(GameObject go in displayShapes)
		{
			GameObject.Destroy(go);
		}

		gameObject.SetActive(false);
	}

	protected void advance()
	{
		ScreenManager.instance.GoToScene(screenToShow);
	}

	IEnumerator closePopUp()
	{
		Debug.Log("S");
		yield return  new WaitForSeconds(10);
		Debug.Log("SSW");
		show(false);
	}
}
