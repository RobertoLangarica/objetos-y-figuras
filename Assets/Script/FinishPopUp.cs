using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FinishPopUp : MonoBehaviour 
{
	protected GameObject[] robots;

	// Use this for initialization
	void Start () 
	{
		transform.GetChild(0).gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;

		robots = GameObject.FindGameObjectsWithTag("RobotPopUp");
	}

	public void onExit()
	{
		ScreenManager.instance.showPrevScene();
	}

	public void show()
	{
		choseRobot();
		transform.GetChild(0).gameObject.GetComponent<RectTransform>().DOScale(new Vector3(1,1,1) ,1).SetEase(Ease.OutBack);
	}

	protected void choseRobot()
	{
		int rand = Random.Range(0,3);
		for(int i =0; i<robots.Length; i++)
		{
			robots[i].SetActive(false);
			if(i==rand)
			{
				robots[i].SetActive(true);
			}
		}
	}
}
