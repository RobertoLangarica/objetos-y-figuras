using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;

public class InternalShape : MonoBehaviour 
{
	public bool checkScale = false;

	[HideInInspector]
	public List<GameObject> possibleAnswers = new List<GameObject>();
	[HideInInspector]
	public bool correctPiece;
	[HideInInspector]
	public int[] requiredAngle;
	[HideInInspector]
	public float range;
	[HideInInspector]
	public float distRange = 0.3f;
	[HideInInspector]
	public Placeholder father;

	public bool isOption(GameObject go)
	{
		for(int i = 0;i < possibleAnswers.Count;i++)
		{
			if(possibleAnswers[i] != null)
			{
				if(possibleAnswers[i].transform.parent.gameObject.Equals(go))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool isWithinRange(GameObject go)
	{
		float sqrDist = (transform.position - go.transform.position).sqrMagnitude;

		if (sqrDist < distRange) 
		{
			return true;
		}
		return false;
	}

	public bool calculateAngle(GameObject go)
	{
		Shape shp = go.GetComponent<Shape>();
		bool flag = false;
		if(go.name == "GCircle" && requiredAngle[0] > 360)
		{
			return true;
		}
		for(int i = 0;i < requiredAngle.Length;i++)
		{
			if((shp.currentRotation >= (requiredAngle[i] - range)) && (shp.currentRotation <= (requiredAngle[i] + range)))
			{
				flag = true;
			}
		}
		return flag;
	}

	int getClosestAngle(GameObject go)
	{
		float dist = 400;
		int result = 0;
		Shape shp = go.GetComponent<Shape>();
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

	public bool havevSameScale(GameObject go)
	{
		Vector2 goScale = new Vector2(Mathf.Abs(Mathf.Round(go.transform.localScale.x *1000)*0.001f),Mathf.Abs(Mathf.Round(go.transform.localScale.y *1000)*0.001f));
		Vector2 mScale = new Vector2(Mathf.Abs(Mathf.Round(transform.localScale.x *1000)*0.001f),Mathf.Abs(Mathf.Round(transform.localScale.y *1000)*0.001f));
		if(goScale == mScale)
		{
			return true;
		}
		return false;
	}

	public void setPiece(GameObject go)
	{
		int rotAngle = 0;

		correctPiece = true;
		go.GetComponent<Shape>().isPositionated = true;
		DOTween.Kill("SnapMove",true);
		go.transform.DOMove(transform.position,0.2f).SetEase(Ease.InOutSine).SetId("SnapMove").OnComplete(()=>{tweenCallback();});
		rotAngle = getClosestAngle(go);
		go.transform.DOLocalRotate(new Vector3(0,0,rotAngle),0.2f).SetEase(Ease.InOutSine).SetId("SnapMove");
	}

	protected void tweenCallback()
	{
		if(father != null)
		{
			father.turnOnInput();
		}
	}
}
