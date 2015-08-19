using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SelectColorBaseShape : MonoBehaviour 
{
	protected GameObject[] pieces;
	// Use this for initialization
	void Start () 
	{
		pieces = GameObject.FindGameObjectsWithTag("MenuShape");
		SetColor();
	}

	protected void SetColor()
	{
		int rand; 
		List<int> colorsShown = new List<int>();
		int count = 0;
		int repeatcount = 0;

		while(count < pieces.Length)
		{
			rand = Random.Range(0,System.Enum.GetValues(typeof(BaseShape.EShapeColor)).Length-1);

			if(colorsShown.Contains(rand))
			{
				if(repeatcount >(System.Enum.GetValues(typeof(BaseShape.EShapeColor)).Length-1)*1.5f )
				{
					//Se asigna un color repetido
					colorsShown.Clear();
					pieces[count].GetComponent<BaseShape>().color = (BaseShape.EShapeColor)rand;
					count++;
					repeatcount = 0;
				}
			}
			else
			{
				colorsShown.Add(rand);
				pieces[count].GetComponent<BaseShape>().color = (BaseShape.EShapeColor)rand;
				count++;
			}

			repeatcount++;
		}
	}
}
	