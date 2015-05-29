using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Analyzer : MonoBehaviour 
{/*
	public List<Pair> toAnalizeVertexes = new List<Pair>();
	public List<Shape> allPieces = new List<Shape>();

	void Start()
	{
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			vertexesAreCorrect();
		}
	}

	public bool vertexesAreCorrect()
	{
		Vector2 vect1 = Vector2.zero;
		Vector2 vect2 = Vector2.zero;
		float dist = 0;
		float required = 0;

		foreach(Pair val in toAnalizeVertexes)
		{
			vect1 = new Vector2(allPieces[val.piece1].pieceVertexes[val.vertex1].position.x,allPieces[val.piece1].pieceVertexes[val.vertex1].position.y); 
			vect2 = new Vector2(allPieces[val.piece2].pieceVertexes[val.vertex2].position.x,allPieces[val.piece2].pieceVertexes[val.vertex2].position.y); 
			dist = Vector2.SqrMagnitude(vect1-vect2);
			required = val.distance * val.distance;
			if(dist > required)
			{
				return false;
			}
		}
		return true;
	}*/
}