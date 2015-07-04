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
	public EGroups type;

	protected int totalGroups = 2;
	protected GameObject[] currentShapes;
	protected Color[] availableColors;
	protected float[] availableScales;

	void Start()
	{
		availableColors = new Color[3]{
			new Color(255,0,0),
			new Color(0,255,0),
			new Color(0,0,255)
		};

		availableScales = new float[]{0.5f,1,1.5f,2,2.5f,3};

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

		switch (type) 
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

		switch (type) 
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
					currentShapes[i].transform.renderer.material.color = availableColors[rmdIdx];
				}
				else
				{
					currentShapes[i].transform.renderer.material.color = availableColors[rmdIdx2];
				}
			}
		}
			break;
		}
	}
}