using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Placeholder : MonoBehaviour 
{
	public List<InternalShape> internalShapes = new List<InternalShape>();

	public void fillChildInfo(int idPlaceHolderChild, GameObject[] shapes,int[] angle,float range)
	{
		InternalShape intShp = transform.GetChild(idPlaceHolderChild).gameObject.GetComponent<InternalShape>();
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
		intShp.range = range;
	}

	public bool isCorrect()
	{
		foreach(InternalShape val in internalShapes)
		{
			if(!val.correctPiece)
			{
				return false;
			}
		}
		return true;
	}
}
