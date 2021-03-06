﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;

public class Placeholder : MonoBehaviour 
{
	public List<InternalShape> internalShapes = new List<InternalShape>();

	[HideInInspector]
	public GameObject input;
	[HideInInspector]
	public bool canTurnOn = true;

	public void fillChildInfo(int idPlaceHolderChild, GameObject[] shapes,int[] angle,float range)
	{
		InternalShape intShp = transform.GetChild(idPlaceHolderChild).gameObject.GetComponent<InternalShape>();
		intShp.father = this;
		int i = 0;

		for(i = 0;i < shapes.Length;i++)
		{
			intShp.possibleAnswers.Add(shapes[i].transform.GetChild(0).gameObject);
		}
		intShp.requiredAngle = new int[angle.Length];
		for(i = 0;i < angle.Length;i++)
		{
			intShp.requiredAngle[i] = angle[i];
		}
	}

	public bool isCorrect()
	{
		//DOTween.Kill("SnapMove");
		GameObject[] tempshps = null;
		if((SpacegramManager)FindObjectOfType (typeof(SpacegramManager)) != null)
		{
			tempshps = ((SpacegramManager)FindObjectOfType (typeof(SpacegramManager))).shapes;
		}
		else if((TangramManager)FindObjectOfType (typeof(TangramManager)) != null)
		{
			tempshps = ((TangramManager)FindObjectOfType (typeof(TangramManager))).shapes.ToArray();
		}

		for (int i = 0; i < internalShapes.Count; i++) 
		{
			internalShapes [i].correctPiece = false;
			for(int j = 0;j < tempshps.Length;j++)
			{
				if(tempshps[j] != null)
				{
					if(internalShapes[i].isOption(tempshps[j]))
					{
						if(internalShapes[i].isWithinRange(tempshps[j]))
						{
							if(internalShapes[i].calculateAngle(tempshps[j]))
							{
								if(!internalShapes[i].checkScale)
								{
									internalShapes[i].setPiece(tempshps[j]);
									input.SetActive(false);
								}
								else
								{
									if(internalShapes[i].havevSameScale(tempshps[j]))
									{
										internalShapes[i].setPiece(tempshps[j]);
										input.SetActive(false);
									}
								}
							}
						}
					}
				}
			}
		}

		foreach(InternalShape val in internalShapes)
		{
			if(!val.correctPiece)
			{
				return false;
			}
		}
		return true;
	}

	public void turnOnInput()
	{
		if(canTurnOn)
		{
			input.SetActive(true);
		}
	}
}
