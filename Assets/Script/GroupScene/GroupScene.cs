using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupScene : MonoBehaviour 
{
	public enum EGroups
	{
		SHAPE,
		COLOR,
		SIZE
	}

	public static string typeOfGroup;

	public int groupSize = 3;
	public List<GameObject> kindsOfShapes = new List<GameObject>();

	protected int totalGroups = 2;
	public EGroups type;
	protected GameObject[] currentShapes;

	void Start()
	{
		currentShapes = new GameObject[groupSize*totalGroups];

		generateShapes ();
	}

	protected void generateShapes()
	{
		List<int> ndxs = new List<int>();
		for (int i = 0; i < kindsOfShapes.Count; i++) 
		{
			ndxs.Add(i);
		}

		switch (type) 
		{
		case(EGroups.SHAPE):
		{
			int rmdIdx = Random.Range(0,ndxs.Count);
			ndxs.RemoveAt(rmdIdx);
			int rmdIdx2 = Random.Range(0,ndxs.Count);
			for(int i = 0;i < currentShapes.Length;i++)
			{
				if(i < groupSize)
				{
					currentShapes[i] = GameObject.Instantiate(kindsOfShapes[rmdIdx]) as GameObject;
				}
				else
				{
					currentShapes[i] = GameObject.Instantiate(kindsOfShapes[rmdIdx2]) as GameObject;
				}
			}
		}
			break;
		case(EGroups.COLOR):
		{
			for(int i = 0;i < currentShapes.Length;i++)
			{
				currentShapes[i] = GameObject.Instantiate(kindsOfShapes[Random.Range(0,kindsOfShapes.Count)]) as GameObject;
			}
		}
			break;
		case(EGroups.SIZE):
		{
			GameObject rdm = kindsOfShapes[Random.Range(0,kindsOfShapes.Count)];
			for(int i = 0;i < currentShapes.Length;i++)
			{
				currentShapes[i] = GameObject.Instantiate(rdm) as GameObject;
			}
		}
			break;
		}
	}
}