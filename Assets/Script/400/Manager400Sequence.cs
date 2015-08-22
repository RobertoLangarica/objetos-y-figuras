using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Manager400Sequence : MonoBehaviour {

	public TangramInput input;
	public Notification notification;
	public RectTransform gameArea;
	public RectTransform shapesArea;
	public Button finishButton;
	public int maxSize = 6;
	public int minSize = 0;
	public bool useFullRangeForPositionate = false;
	public GameObject[] allowedGeometricShapes;
	public AudioSource audioSource;
	public AudioClip audioWrong;
	public AudioClip audioRight;
	public AudioClip finalAudio;
	
	protected Rect shapesRect;
	protected Container400[] containers;
	protected List<Shape400> shapes;
	protected List<Shape400> placeholders;
	protected int currentStage = -1;
	protected bool vertical;
	protected bool excerciseFinished;

	protected FinishPopUp finishPopUp;
	// Use this for initialization
	void Start () 
	{
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
		vertical = gameArea.GetComponent<VerticalLayoutGroup>() != null;

		shapes = new List<Shape400>();
		placeholders = new List<Shape400>();
		
		input.onDragFinish+=onDragFinish;
		input.onDragStart+=onDragStart;
		
		//Inicializamos el ejercicio
		showNextExcercise();

		//Se llama starGame en el analytic para setear el tiempo = 0
		AnalyticManager.instance.startGame();

		finishPopUp = FindObjectOfType<FinishPopUp>();
	}

	protected void setContainersAreas(int containersCount)
	{
		Vector3[] corners = new Vector3[4];

		gameArea.GetWorldCorners(corners);
		
		Vector2 min = Vector2.zero;
		Vector2 max = Vector2.zero;
		float size	= vertical ? (corners[1].y - corners[0].y) : (corners[2].x - corners[0].x);
		size/=containersCount;
		
		for(int i= 0; i < containersCount; i++)
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
	}
	
	/**
	 * Muestra el siguienet ejercicio o la pantalla de final si es necesario
	 * */
	protected void showNextExcercise()
	{
		if(currentStage >= 0)
		{
			AnalyticManager.instance.finsh("Ordena","Secuencia" ,currentStage.ToString());
		}
		currentStage++;
		
		if(currentStage > 11)//12 pantallas
		{
			excerciseFinished=true;
			showFinalScreen();	
		}
		else
		{
			buildNextStage();
		}
		//Se llama starGame en el analytic para setear el tiempo = 0
		AnalyticManager.instance.startGame();
	}
	
	/**
	 * Limpia el ejercicio actual y despues de un delay manda mostrar el siguiente.
	 * */
	IEnumerator cleanCurrentExcercise()
	{
		foreach(Shape400 s in shapes)
		{
			s.destroy(0.5f);
		}
		
		foreach(Shape400 s in placeholders)
		{
			s.destroy(0.5f);
		}


		foreach(Container400 s in containers)
		{
			s.hide(true,true);
		}

		shapes.Clear();
		placeholders.Clear();
		
		yield return new WaitForSeconds(0.5f);
		finishButton.interactable = true;
		showNextExcercise();
	}
	
	/**
	 * Construye los ejercicios (currentStage debe de venir previamente validado)
	 **/ 
	protected void buildNextStage()
	{
		int shapesCount = 0;
		int colorsCount = 0;
		int containersCount = 0;
		int optionsCount = 0;
		int size = Random.Range(minSize,maxSize);
		List<int> tmpFigures = new List<int>();
		List<int> prevIndex = new List<int>();
		int refsCount = 0;
		bool cloneFirstShape = false;

		//Configuración
		switch(currentStage)
		{
		case 0:
		case 1:
			shapesCount = 1;
			colorsCount = 2;
			refsCount	= 3;
			containersCount = 6;
			optionsCount = 5;
			break;
		case 2:
		case 3:
			shapesCount = 2;
			colorsCount = 1;
			refsCount	= 4;
			containersCount = 7;
			optionsCount = 4;
			break;
		case 4:
		case 5:
			shapesCount = 1;
			colorsCount = 3;
			refsCount	= 5;
			containersCount = 8;
			optionsCount = 6;

			size = minSize;
			break;
		case 6:
		case 7:
			shapesCount = 3;
			colorsCount = 1;
			refsCount	= 5;
			containersCount = 8;
			optionsCount = 6;

			size = minSize;
			break;
		case 8:
		case 9:
			shapesCount = 2;
			colorsCount = 3;
			refsCount	= 5;
			containersCount = 8;
			optionsCount = 6;
			cloneFirstShape = true;//La primera se repite 2 veces

			size = minSize;
			break;
		case 10:
		case 11:
			shapesCount = 4;
			colorsCount = 4;
			refsCount	= 5;
			containersCount = 8;
			optionsCount = 6;

			size = minSize;
			break;
		}

		//Obtenemos las figuras
		while(tmpFigures.Count < shapesCount)
		{
			int index = Random.Range(0,allowedGeometricShapes.Length);

			if(!tmpFigures.Contains(index))
			{
				tmpFigures.Add (index);
			}
		}

		if(cloneFirstShape)
		{
			tmpFigures.Add (tmpFigures[0]);
		}
		
		//Obtenemos los colores
		List<int> colors = new List<int>();
		
		while(colors.Count < colorsCount)
		{
			int color = Random.Range(0,System.Enum.GetValues(typeof(BaseShape.EShapeColor)).Length-1);
			
			if(!colors.Contains(color))
			{
				colors.Add(color);
			}
		}


		//Areas para los contenedores
		setContainersAreas(containersCount);

		int count = 0;
		int currentColor;
		int currentShape;

		//Placeholders y contenedores
		for(int i = 0; i < containers.Length; i++)
		{
			containers[i].gameObject.SetActive((i < containersCount) ? true:false);
			containers[i].next = -1;
			containers[i].isEmpty = true;

			if(i < containersCount)
			{
				Vector3 pos = new Vector3(containers[i].getCenter().x,
				                          containers[i].getCenter().y,0);

				currentColor = count < colors.Count ? colors[count]:colors[count%colors.Count];
				currentShape = count < tmpFigures.Count ? tmpFigures[count]:tmpFigures[count%tmpFigures.Count];

				containers[i].value = currentShape;
				containers[i].secondValue = currentColor;
				containers[i].active = true;

				if(i < refsCount)
				{
					containers[i].isEmpty = false;
					containers[i].hide(false);

					placeholders.Add( (GameObject.Instantiate(
						allowedGeometricShapes[currentShape],pos,Quaternion.identity) as GameObject).GetComponent<Shape400>());
					placeholders[i].setSizeByInt(size);
					placeholders[i].setColorByInt(currentColor);
					placeholders[i].enabled(false);

					//Los quito de la capa arrastrable
					placeholders[i].gameObject.layer = LayerMask.NameToLayer("Background");
					for(int ii = 0; ii < placeholders[i].transform.childCount; ii++)
					{
						placeholders[i].transform.GetChild(ii).gameObject.layer = LayerMask.NameToLayer("Background");
					}

					containers[i].active = false;

				}
				else
				{
					//Solo los que esperan respuesta tienen next valido
					containers[i].next = i == containersCount-1? -1:i+1;

					shapes.Add( (GameObject.Instantiate(
						allowedGeometricShapes[currentShape],pos,Quaternion.identity) as GameObject).GetComponent<Shape400>());

					shapes[shapes.Count-1].setSizeByInt(size);
					shapes[shapes.Count-1].setColorByInt(currentColor);
					shapes[shapes.Count-1].value = currentShape;
					shapes[shapes.Count-1].secondValue = currentColor;

					if(i != refsCount)
					{
						containers[i].active = false;
						containers[i].hide(true);
					}
					else
					{
						containers[i].hide(false);
					}
				}

				count++;
			}
		}


		//Instanciamos las figuras arrastrables
		while(shapes.Count < optionsCount)
		{
			currentColor = count < colors.Count ? colors[count]:colors[count%colors.Count];
			currentShape = count < tmpFigures.Count ? tmpFigures[count]:tmpFigures[count%tmpFigures.Count];

			shapes.Add( (GameObject.Instantiate(
				allowedGeometricShapes[currentShape]) as GameObject).GetComponent<Shape400>());

			shapes[shapes.Count-1].value = currentShape;
			shapes[shapes.Count-1].secondValue = currentColor;
			shapes[shapes.Count-1].setSizeByInt(size);
			shapes[shapes.Count-1].setColorByInt(currentColor);

			count++;
		}

		//Las posicionamos
		List<int> positions = new List<int>();

		for(int i = 1; i < containersCount; i++)
		{
			positions.Add(i);
		}

		int idx;
		int c = 0;
		while(c < shapes.Count)
		{
			Vector3 pos = Vector3.zero;
			float gap = 0;
			int i;
			idx = Random.Range(0,positions.Count);
			i = positions[idx];
			positions.RemoveAt(idx);
			
			if(useFullRangeForPositionate)
			{
				pos.y = Random.Range(shapesRect.yMin,shapesRect.yMax);
				pos.x = Random.Range(shapesRect.xMin,shapesRect.xMax);
			}
			else if(vertical)
			{
				pos.x = shapesRect.center.x;
				//pos.y = Random.Range(shapesRect.yMin,shapesRect.yMax);
				
				//Contenido en el container
				//gap = (containers[i].max.y-containers[i].min.y)*0.25f;
				//pos.y = Random.Range(containers[i].min.y+gap,containers[i].max.y-gap);
				pos.y = containers[i].getCenter().y;
			}
			else
			{
				pos.y = shapesRect.center.y;
				//pos.x = Random.Range(shapesRect.xMin,shapesRect.xMax);
				
				//Contenido en el container
				//gap = (containers[i].max.x-containers[i].min.x)*0.25f;
				//pos.x = Random.Range(containers[i].min.x+gap,containers[i].max.x-gap);
				pos.x = containers[i].getCenter().x;
			}

			shapes[c].transform.position = pos;

			c++;

		}
	}
	
	protected GameObject getRandomObject(GameObject[] source,ref List<int> shown)
	{
		int index = -1;
		int count = 0;
		
		while(index == -1)
		{
			count++;
			
			index = Random.Range(0,source.Length);

			if(shown.Contains(index))
			{
				//Que no se repita por siempre
				if(count < source.Length*1.5f)
				{
					index = -1;
				}
				else
				{
					//Se va repetir pero no queremos que continue el ciclo
					shown.Clear();
				}
			}
		}
		
		return source[index];
	}
	
	protected void showFinalScreen()
	{
		Debug.Log("PANTALLA FINAL");

		finishPopUp.show();
	}
	
	public void onFinishExcercise()
	{
		finishButton.interactable = false;
		checkAndShowIfExcerciseIsCorrect();
	}
	
	protected void checkAndShowIfExcerciseIsCorrect()
	{
		List<Shape400> badShapes = new List<Shape400>();

		int shapesInContainer = 0;
		int containersForShapes = 1;

		foreach(Shape400 s in shapes)
		{
			if(s.container)
			{
				shapesInContainer++;
			}
		}

		foreach(Container400 c in containers)
		{
			if(c.next != -1)
			{
				containersForShapes++;
			}
		}

		if(shapesInContainer < containersForShapes)
		{
			//Figuras fuera de lugar
			foreach(Shape400 s in shapes)
			{
				if(!s.container)
				{s.GetComponent<ShakeTransform>().startAction(0.5f);}
			}
			
			if(audioSource && audioWrong)
			{
				audioSource.PlayOneShot(audioWrong);
			}
			
			finishButton.interactable = true;
		}
		else
		{
			foreach(Shape400 s in shapes)
			{
				if(s.container)
				{
					if(s.value != s.container.value || s.secondValue != s.container.secondValue)
					{
						badShapes.Add(s);
					}
				}
			}
			
			if(badShapes.Count > 0)
			{
				//Figuras fuera de lugar
				foreach(Shape400 s in badShapes)
				{
					s.GetComponent<ShakeTransform>().startAction(0.5f);
				}
				
				if(audioSource && audioWrong)
				{
					audioSource.PlayOneShot(audioWrong);
				}
				
				finishButton.interactable = true;
			}
			else
			{
				//Ejercicio correcto
				notification.onClose += onNotificationRightComplete;
				notification.showToast("correcto",audioRight,2);
				excerciseFinished=true;
			}
		}
	}
	
	protected bool isCorrect()
	{
		int shapesInContainer = 0;
		int containersForShapes = 1;
		
		foreach(Shape400 s in shapes)
		{
			if(s.container)
			{
				shapesInContainer++;
			}
		}
		
		foreach(Container400 c in containers)
		{
			if(c.next != -1)
			{
				containersForShapes++;
			}
		}
		
		if(shapesInContainer < containersForShapes)
		{
			return false;
		}
		else
		{
			foreach(Shape400 s in shapes)
			{
				if(s.container)
				{
					if(s.value != s.container.value || s.secondValue != s.container.secondValue)
					{
						return false;
					}
				}
			}
		}

		return true;
	}
	
	protected void onNotificationRightComplete()
	{
		notification.onClose -= onNotificationRightComplete;
		StartCoroutine("cleanCurrentExcercise");
	}
	
	public void onDragFinish()
	{
		if(input.selected != null)
		{
			//Vemos dentro de cual contenedor está
			for(int i = 0; i < containers.Length; i++)
			{
				if(containers[i].active && containers[i].Contains(input.selected.transform.position))
				{
					if(!containers[i].isEmpty)
					{
						foreach(Shape400 s in shapes)
						{
							if(s.container && s.container.GetInstanceID() == containers[i].GetInstanceID())
							{
								s.container = null;
								Vector2 pos = shapesRect.center;
								
								if(vertical || useFullRangeForPositionate)
								{
									pos.y = s.transform.position.y;
								}
								else
								{
									pos.x = s.transform.position.x;
								}
								
								s.moveTo(pos);
								break;
							}
						}
					}

					if(containers[i].next != -1)
					{
						containers[containers[i].next].active = true;
						containers[containers[i].next].hide(false);
					}

					containers[i].isEmpty = false;
					((Shape400)input.selected).container = containers[i];
					
					if(isCorrect())
					{
						onFinishExcercise();
					}
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
}
