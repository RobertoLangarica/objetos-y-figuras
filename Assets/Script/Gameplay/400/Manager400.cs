using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Manager400 : MonoBehaviour {
	
	public TangramInput input;
	public RectTransform gameArea;
	public RectTransform shapesArea;

	protected Rect shapesRect;
	protected Container400[] containers;
	protected Shape400[] shapes;

	public GameObject square;

	// Use this for initialization
	void Start () {
		//(orthoSize*2*pixelsPerUnit)/screenHeight
		float u = (Camera.main.orthographicSize*2*100)/Screen.height;

		Vector3[] corners = new Vector3[4];

		//Area para las figuras (aqui aparecen)
		shapesArea.GetWorldCorners(corners);
		shapesRect = new Rect();
		shapesRect.xMin = corners[0].x;
		shapesRect.xMax = corners[2].x;
		shapesRect.yMin = corners[0].y;
		shapesRect.yMax = corners[1].y;

		//Contenedores para el ejercicio
		containers = gameArea.GetComponentsInChildren<Container400>();

		//Inicializamos las areas de los contenedores
		bool vertical = gameArea.GetComponent<VerticalLayoutGroup>() != null;

		gameArea.GetWorldCorners(corners);

		Vector2 min = Vector2.zero;
		Vector2 max = Vector2.zero;
		float size	= vertical ? (corners[1].y - corners[0].y) : (corners[2].x - corners[0].x);
		size/=containers.Length;

		for(int i= 0; i < containers.Length; i++)
		{
			if(vertical)
			{
				min.x = corners[0].x;
				max.x = corners[2].x;

				max.y = corners[1].y - size*i;
				min.y = corners[1].y - size*(i+1);
			}
			else
			{
				max.y = corners[1].y;
				min.y = corners[0].y;

				min.x = size*i + corners[0].x;
				max.x = size*(i+1) + corners[0].x;
			}

			containers[i].setArea(min,max);
		}



		input.onDragFinish+=onDragFinish;
		input.onDragStart+=onDragStart;

		//Inicializamos las figuras
	}

	void Update () 
	{

	}

	public void onDragFinish()
	{
		if(input.selected != null)
		{
			//Vemos dentro de cual contenedor está
			for(int i = 0; i < containers.Length; i++)
			{
				if(containers[i].isEmpty && containers[i].Contains(input.selected.transform.position))
				{
					containers[i].isEmpty = false;
					((Shape400)input.selected).container = containers[i];
					break;
				}
			}

		}
	}

	public void onDragStart()
	{
		if(input.selected != null)
		{
			//Shape400
			if(((Shape400)input.selected).container)
			{
				((Shape400)input.selected).container.isEmpty = true;
				((Shape400)input.selected).container = null;
			}
		}
	}

	public void onFinish()
	{
		/*
		 * Hacemos el chequeo de si alguna pieza esta huerfana de container
		 * pero visualmente si esta en uno y no se encima con nadie mas (una pieza por container)
		 * */
	}
}