﻿using UnityEngine;
using System.Collections;

public class FinishPopUp : MonoBehaviour 
{
	protected GameObject[] robots;

	// Use this for initialization
	void Start () 
	{
		transform.localScale = Vector3.zero;

		robots = GameObject.FindGameObjectsWithTag("RobotPopUp");
	}

	public void onExit()
	{
		ScreenManager.instance.showPrevScene();
	}

	public void show()
	{}

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
