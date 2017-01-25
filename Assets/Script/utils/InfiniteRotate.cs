using UnityEngine;
using System.Collections;

public class InfiniteRotate : MonoBehaviour 
{	
	public float rotateSpeed = 10.0f;

	protected Vector3 angles;

	void Start()
	{
		angles = new Vector3(0,0,1);
	}

	void Update () 
	{
		this.transform.Rotate(angles*rotateSpeed*Time.deltaTime);
	}
}
