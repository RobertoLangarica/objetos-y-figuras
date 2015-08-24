using UnityEngine;
using System.Collections;
using DG.Tweening.Core;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class SandboxShape : BaseShape 
{
	public float rotateAmount = 15;

	public void onDragFinish()
	{
		float rot = transform.rotation.eulerAngles.z;
		float mod = rot%rotateAmount;

		//solo rotaciones multiplo permitidas
		if(mod != 0)
		{
			rot -= mod;
			if(mod > rotateAmount*0.5f)
			{
				//Hacia arriba
				rot += rotateAmount;
			}
			transform.eulerAngles = new Vector3(0,0,rot);
		}
	}	
}
