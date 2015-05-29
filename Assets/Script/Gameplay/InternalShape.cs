using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;

public class InternalShape : MonoBehaviour 
{
	[HideInInspector]
	public List<GameObject> possibleAnswers = new List<GameObject>();
	//[HideInInspector]
	public bool correctPiece;
	[HideInInspector]
	public int[] requiredAngle;
	[HideInInspector]
	public float range;

	public string firstCorrectPiece = "";

	void OnTriggerEnter2D(Collider2D other) 
	{
		foreach(GameObject val in possibleAnswers)
		{
			if(other.gameObject.Equals(val))
			{
				if(calculateAngle(other.gameObject))
				{
					if(firstCorrectPiece == "")
					{
						int rotAngle = 0;

						firstCorrectPiece = val.transform.parent.name;
						correctPiece = true;
						DOTween.Kill("SnapMove",true);
						other.transform.parent.transform.DOMove(transform.position,1).SetEase(Ease.InOutSine).SetId("SnapMove");
						rotAngle = getClosestAngle(other.gameObject);
						other.transform.parent.transform.DOLocalRotate(new Vector3(0,0,rotAngle),0.5f).SetEase(Ease.InOutSine).SetId("SnapMove");
						DOTween.Pause("SnapMove");
					}
				}
			}
		}
	}

	//void Update()
	//{
	//	Debug.Log(possibleAnswers.Capacity);
	//	for(int i=0; i<possibleAnswers.Capacity; i++)
	//	{
	//		if(firstCorrectPiece == possibleAnswers[i].transform.parent.name)
	//		{
	//			possibleAnswers[i].transform.parent.renderer.material.color = Color.black;
	//		}
	//		else
	//		{
	//			possibleAnswers[i].transform.parent.renderer.material.color = Color.red;
	//		}
	//	}
	//}

	bool calculateAngle(GameObject go)
	{
		Shape shp = go.transform.parent.GetComponent<Shape>();
		bool flag = false;
		for(int i = 0;i < requiredAngle.Length;i++)
		{
			if((shp.currentRotation >= (requiredAngle[i] - range)) && (shp.currentRotation <= (requiredAngle[i] + range)))
			{
				flag = true;
			}
		}
		return flag;
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		DOTween.Kill("SnapMove",true);
		if(other.transform.parent.name == firstCorrectPiece)
		{
			firstCorrectPiece = "";
			correctPiece = false;
		}
	}
	
	void OnTriggerStay2D(Collider2D other) 
	{
		if(firstCorrectPiece == "")
		{
			foreach(GameObject val in possibleAnswers)
			{
				if(other.gameObject.Equals(val))
				{
					if(calculateAngle(other.gameObject))
					{
						if(firstCorrectPiece == "")
						{
							int rotAngle = 0;
							
							firstCorrectPiece = val.transform.parent.name;
							correctPiece = true;
							DOTween.Kill("SnapMove",true);
							other.transform.parent.transform.DOMove(transform.position,1).SetEase(Ease.InOutSine).SetId("SnapMove");
							rotAngle = getClosestAngle(other.gameObject);
							other.transform.parent.transform.DOLocalRotate(new Vector3(0,0,rotAngle),0.5f).SetEase(Ease.InOutSine).SetId("SnapMove");
							DOTween.Pause("SnapMove");
						}
					}
				}
			}
		}
	}

	int getClosestAngle(GameObject go)
	{
		float dist = 400;
		int result = 0;
		Shape shp = go.transform.parent.GetComponent<Shape>();
		for (int i = 0; i < requiredAngle.Length; i++) 
		{
			if(Mathf.Abs(requiredAngle[i] - shp.currentRotation) < dist)
			{
				dist = Mathf.Abs(requiredAngle[i] - shp.currentRotation);
				result = requiredAngle[i];
			}
		}
		return result;
	}
}
