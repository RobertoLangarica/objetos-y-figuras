using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class MenuController : MonoBehaviour {

	public GameObject content;
	public Image right;
	public Image left;
	public ScrollRect scrollRect;
	protected Tween rT;
	protected Tween lT;
	protected bool checkScrollPosition = false;
	protected bool rH = false;
	protected bool lH = false;
	protected float prev = -1;

	void Start()
	{
		//Cargamos los niveles desde aqui
		LevelManager.instance.getLevel("1");


		//Todos los niveles
		Level[] levels = LevelManager.instance.getAllLevels();

		foreach(Level level in levels)
		{
			GameObject tmp = (GameObject)Resources.Load("Menu/"+level.name+"_menu");
			GameObject go = ((GameObject)GameObject.Instantiate(tmp));
			go.transform.SetParent(content.transform);
			go.GetComponent<MenuItems>().lvlName = level.name;
			go.GetComponent<MenuItems>().lvlPurchseID = level.purchaseID;
			go.GetComponent<LayoutElement>().minWidth = Screen.width/3.0f;
			if(UserDataManager.instance.premiumVersion != "premiumVersion" && level.purchaseID == "spacegramShips062015")
			{
				go.GetComponent<Image>().color = new Color(255,0,0);
			}
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

	public void goToStartMenu()
	{
		ScreenManager.instance.GoToScene("StartMenu");
	}
}
