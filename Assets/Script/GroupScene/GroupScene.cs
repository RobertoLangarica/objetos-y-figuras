using UnityEngine;
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
	public List<GameObject> kindsOfShapes = new List<GameObject>();
	//public EGroups type;

	protected int totalGroups = 2;
	protected int currentLevel = 0;
	protected int maxLevel = 0;
	protected XMLLoader loader = GameObject.FindObjectOfType<XMLLoader>();
	protected GameObject[] currentShapes;
	protected int[] availableColors;
	protected float[] availableScales;

	void Start()
	{
		loader = GameObject.FindObjectOfType<XMLLoader>();

		availableColors = new int[9]{0,1,2,3,4,5,6,7,8};

		availableScales = new float[]{0.1f,0.3f,0.5f,0.7f,1};

		currentShapes = new GameObject[groupSize*totalGroups];

		generateShapes ();

		modifyShapes ();
	}

	protected void generateShapes()
	{
		List<int> ndxs = new List<int>();
		for (int i = 0; i < kindsOfShapes.Count; i++) 
		{
			ndxs.Add(i);
		}

		switch (typeOfGroup) 
		{
		case(EGroups.SHAPE):
		{
			int rmdIdx = ndxs[Random.Range(0,ndxs.Count)];
			ndxs.RemoveAt(rmdIdx);
			int rmdIdx2 = ndxs[Random.Range(0,ndxs.Count)];
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

	protected void modifyShapes()
	{
		List<int> ndxs = new List<int>();
		int rmdIdx = 0;
		int rmdIdx2 = 0;

		switch (typeOfGroup) 
		{
		case(EGroups.SHAPE):
		{}
			break;
		case(EGroups.SIZE):
		{
			for (int i = 0; i < availableScales.Length; i++) 
			{
				ndxs.Add(i);
			}
			rmdIdx = ndxs[Random.Range(0,ndxs.Count)];
			ndxs.RemoveAt(rmdIdx);
			rmdIdx2 = ndxs[Random.Range(0,ndxs.Count)];
			for(int i = 0;i < currentShapes.Length;i++)
			{
				if(i < groupSize)
				{
					currentShapes[i].transform.localScale = new Vector3(availableScales[rmdIdx],availableScales[rmdIdx],1);
				}
				else
				{
					currentShapes[i].transform.localScale = new Vector3(availableScales[rmdIdx2],availableScales[rmdIdx2],1);
				}
			}
		}
			break;
		case(EGroups.COLOR):
		{
			for (int i = 0; i < availableColors.Length; i++) 
			{
				ndxs.Add(i);
			}
			rmdIdx = ndxs[Random.Range(0,ndxs.Count)];
			ndxs.RemoveAt(rmdIdx);
			rmdIdx2 = ndxs[Random.Range(0,ndxs.Count)];
			for(int i = 0;i < currentShapes.Length;i++)
			{
				if(i < groupSize)
				{
					//currentShapes[i].transform.renderer.material.color = availableColors[rmdIdx];
				}
				else
				{
					//currentShapes[i].transform.renderer.material.color = availableColors[rmdIdx2];
				}
			}
		}
			break;
		}
	}
}