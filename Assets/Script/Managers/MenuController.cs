using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class MenuController : MonoBehaviour {

	public GameObject content;
	public RectTransform RT;
	public Image right;
	public Image left;
	public ScrollRect scrollRect;

	protected Tween rT;
	protected Tween lT;
	protected bool checkScrollPosition = false;
	protected bool rH = false;
	protected bool lH = false;
	protected float prev = -1;

	protected bool mov = false;
	public bool moveLeft= false;
	protected float speedMax = 30;
	protected float speedMin = 7;

	protected List<GameObject> levelsPrefab = new List<GameObject>();
	public static int currLevel = 0;
	void Start()
	{
		//Cargamos los niveles desde aqui
		LevelManager.instance.getLevel("1");

		//Todos los niveles
		Level[] levels = LevelManager.instance.getAllLevels();

		//foreach(Level level in levels)
		//{
		//	GameObject tmp = (GameObject)Resources.Load("Menu/"+level.name+"_menu");
		//	GameObject go = ((GameObject)GameObject.Instantiate(tmp));
		//	float u = (Camera.main.orthographicSize*2*100)/Screen.height;
		//
		//	go.transform.SetParent(content.transform);
		//	go.GetComponent<MenuItems>().lvlName = level.name;
		//	go.GetComponent<LayoutElement>().minWidth = (Screen.height/4.5f)*u;
		//	go.GetComponent<LayoutElement>().minHeight = (Screen.height/5f)*u;
		//	go.transform.localPosition = new Vector3(go.transform.localPosition.x,go.transform.localPosition.y,0);
		//	levelsPrefab.Add(go);
		//}
		//
		//left.DOFade(0,0.2f).OnComplete(()=>{left.gameObject.SetActive(false);});
		//Debug.Log(SpacegramManager.);

		SpacegramManager.lvlToPrepare = "SN01_02";
		ScreenManager.instance.GoToScene ("Spacegram");
	}

	//void Update()
	//{
	//	if(checkScrollPosition)
	//	{
	//		if(prev == -1)
	//		{
	//			prev = scrollRect.velocity.x;
	//			return;
	//		}
	//
	//		if((int)prev*1000 != (int)scrollRect.velocity.x*1000)
	//		{
	//			prev = scrollRect.velocity.x;
	//			return;
	//		}
	//
	//		prev = -1;
	//		checkScrollPosition = false;
	//	
	//		float val = ((int)(scrollRect.horizontalNormalizedPosition*1000))/1000.0f;
	//
	//		if(val <= 0)
	//		{
	//			//Solo derecha
	//			right.DOFade(1,0.2f).OnStart(()=>{right.gameObject.SetActive(true);});
	//		}
	//		else if(val >= 1.0f)
	//		{
	//			//Solo izquierda
	//			left.DOFade(1,0.2f).OnStart(()=>{left.gameObject.SetActive(true);});
	//		}
	//		else
	//		{
	//			left.DOFade(1,0.2f).OnStart(()=>{left.gameObject.SetActive(true);});
	//			right.DOFade(1,0.2f).OnStart(()=>{right.gameObject.SetActive(true);});
	//		}
	//	}
	//	if(mov)
	//	{
	//		moveScrollShips(moveLeft);
	//	}
	//}
	//
	//public void checkPos()
	//{
	//
	//}
	//
	//public void onStartDrag()
	//{
	//	right.DOFade(0,0.2f).OnComplete(()=>{right.gameObject.SetActive(false);});
	//	left.DOFade(0,0.2f).OnComplete(()=>{left.gameObject.SetActive(false);});
	//	checkScrollPosition = false;
	//}
	//
	//public void onStopDrag()
	//{
	//	checkScrollPosition = true; 
	//}
	//
	//public void goToGameMenu()
	//{
	//	ScreenManager.instance.GoToScene("GameMenu");
	//}
	//
	//
	//public void moveScrollShips(bool l)
	//{
	//	Vector2 pos =RT.anchoredPosition;
	//	if(speedMin<speedMax)
	//	{
	//		speedMin += 0.4f;
	//	}
	//
	//	if(l)
	//	{
	//		pos.x += speedMin;
	//		RT.anchoredPosition = pos;
	//	}
	//	else
	//	{
	//		pos.x -= speedMin;
	//		RT.anchoredPosition = pos;
	//	}
	//
	//}
	//
	//public void onClick(bool go)
	//{
	//	if(go)
	//	{
	//		checkScrollPosition = false; 
	//		mov = true;
	//	}
	//	else
	//	{
	//		checkScrollPosition = true; 
	//		mov = false;
	//		speedMin = 7;
	//	}
	//
	//}
	//public void choose(bool left)
	//{
	//	if(left)
	//	{
	//		moveLeft = true;
	//	}
	//	else
	//	{
	//		moveLeft = false;
	//	}
	//}
}