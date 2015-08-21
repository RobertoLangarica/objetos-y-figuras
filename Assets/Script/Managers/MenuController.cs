using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Soomla.Store;

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


	void Start()
	{
		//Cargamos los niveles desde aqui
		LevelManager.instance.getLevel("1");
		
		StoreEvents.OnMarketPurchase += onMarketPurchase;

		//Todos los niveles
		Level[] levels = LevelManager.instance.getAllLevels();

		foreach(Level level in levels)
		{
			GameObject tmp = (GameObject)Resources.Load("Menu/"+level.name+"_menu");
			GameObject go = ((GameObject)GameObject.Instantiate(tmp));
			float u = (Camera.main.orthographicSize*2*100)/Screen.height;

			go.transform.SetParent(content.transform);
			go.GetComponent<MenuItems>().lvlName = level.name;
			go.GetComponent<LayoutElement>().minWidth = (Screen.width/8.0f)*u;
			go.transform.localPosition = new Vector3(go.transform.localPosition.x,go.transform.localPosition.y,0);
			levelsPrefab.Add(go);
		}

		left.DOFade(0,0.2f);
	}

	void Update()
	{
		if(checkScrollPosition)
		{
			if(prev == -1)
			{
				prev = scrollRect.velocity.x;
				return;
			}

			if((int)prev*1000 != (int)scrollRect.velocity.x*1000)
			{
				prev = scrollRect.velocity.x;
				return;
			}

			prev = -1;
			checkScrollPosition = false;
		
			float val = ((int)(scrollRect.horizontalNormalizedPosition*1000))/1000.0f;

			if(val <= 0)
			{
				//Solo derecha
				right.DOFade(1,0.2f);
			}
			else if(val >= 1.0f)
			{
				//Solo izquierda
				left.DOFade(1,0.2f);
			}
			else
			{
				left.DOFade(1,0.2f);
				right.DOFade(1,0.2f);
			}
		}
		if(mov)
		{
			moveScrollShips(moveLeft);
		}
	}

	public void checkPos()
	{

	}

	public void onStartDrag()
	{
		right.DOFade(0,0.2f);
		left.DOFade(0,0.2f);
		checkScrollPosition = false;
	}

	public void onStopDrag()
	{
		checkScrollPosition = true; 
	}

	public void goToGameMenu()
	{
		ScreenManager.instance.GoToScene("GameMenu");
	}


	public void moveScrollShips(bool l)
	{
		Vector2 pos =RT.anchoredPosition;
		if(speedMin<speedMax)
		{
			speedMin += 0.4f;
		}

		if(l)
		{
			pos.x += speedMin;
			RT.anchoredPosition = pos;
		}
		else
		{
			pos.x -= speedMin;
			RT.anchoredPosition = pos;
		}

	}

	public void onClick(bool go)
	{
		if(go)
		{
			checkScrollPosition = false; 
			mov = true;
		}
		else
		{
			checkScrollPosition = true; 
			mov = false;
			speedMin = 7;
		}

	}
	public void choose(bool left)
	{
		if(left)
		{
			moveLeft = true;
		}
		else
		{
			moveLeft = false;
		}
	}

	
	public void onMarketPurchase(PurchasableVirtualItem pvi, string payload,Dictionary<string, string> extra) 
	{
		for (int i = 0; i < levelsPrefab.Count; i++) 
		{
			if(levelsPrefab[i].GetComponent<MenuItems>().lvlPurchseID == "spacegramShips062015")
			{
				levelsPrefab[i].GetComponent<Image>().color = new Color(255,255,255);
			}

		}
	}
}