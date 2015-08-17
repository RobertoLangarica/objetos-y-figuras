using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Manager400Sequence : MonoBehaviour {

	public TangramInput input;
	public Notification notification;
	public RectTransform gameArea;
	public RectTransform shapesArea;
	public int maxSize = 6;
	public int minSize = 0;
	public bool useFullRangeForPositionate = false;
	public GameObject[] allowedGeometricShapes;
	public Button finishButton;
	public AudioSource audioSource;
	public AudioClip audioWrong;
	public AudioClip audioRight;
	public AudioClip finalAudio;
	
	protected Rect shapesRect;
	protected Container400[] containers;
	protected List<Shape400> shapes;
	protected List<Shape400> placeholders;
	protected List<int> shapesShown;
	protected List<int> colorShown;
	protected int currentStage = -1;
	protected bool vertical;
	
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
		
		
		shapesShown = new List<int>();
		colorShown = new List<int>();
		shapes = new List<Shape400>();
		placeholders = new List<Shape400>();
		
		input.onDragFinish+=onDragFinish;
		input.onDragStart+=onDragStart;
		
		//Inicializamos el ejercicio
		showNextExcercise();
	}
	
	/**
	 * Muestra el siguienet ejercicio o la pantalla de final si es necesario
	 * */
	protected void showNextExcercise()
	{
		currentStage++;
		
		if(currentStage > 11)//12 pantallas
		{
			showFinalScreen();	
		}
		else
		{
			buildNextStage();
		}
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
		
		foreach(Container400 c in containers)
		{
			c.isEmpty = true;
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
		int shapesCount;
		int colorsCount;
		int localMinSize = minSize;
		int localMaxSize = maxSize;
		
		//Configuración
		switch(currentStage)
		{
		case 0:
		case 1:
			shapesCount = 1;
			colorsCount = 2;
			break;
		case 2:
		case 3:
			shapesCount = 2;
			colorsCount = 1;
		case 4:
		case 5:
			shapesCount = 1;
			colorsCount = 3;
			break;
		case 6:
		case 7:
			shapesCount = 3;
			colorsCount = 1;
			break;
		case 8:
		case 9:
			shapesCount = 3;
			colorsCount = 1;
			break;
		}
		
		
		GameObject tmp;
		List<int> sizes = new List<int>();
		
		//Randomizando los tamaños (evitando que se repita alguno)
		while(sizes.Count < containers.Length)
		{
			int val = Random.Range(localMinSize,localMaxSize);
			
			if(!sizes.Contains(val))
			{
				sizes.Add(val);
			}
		}

		//Obtenemos la figura
		tmp = getRandomObject(allowedGeometricShapes,ref shapesShown);
		
		//Color aleatorio de entre los 9 disponibles (evitando repetidos)
		int color = -1;
		int colorCount = 0;
		
		while(color == -1)
		{
			color = Random.Range(0,8);
			
			if(colorShown.Contains(color))
			{
				if(colorCount < 14)
				{
					color = -1;
				}
				else
				{
					//se va repetir
					colorShown.Clear();
				}
				
			}
			
			colorCount++;
		}
		
		colorShown.Add(color);
		
		
		//Instanciamos las figuras arrastrables
		for(int i = 0; i < containers.Length; i++)
		{
			Vector3 pos = Vector3.zero;
			
			float gap = 0;
			
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
				gap = (containers[i].max.y-containers[i].min.y)*0.25f;
				pos.y = Random.Range(containers[i].min.y+gap,containers[i].max.y-gap);
			}
			else
			{
				pos.y = shapesRect.center.y;
				//pos.x = Random.Range(shapesRect.xMin,shapesRect.xMax);
				
				//Contenido en el container
				gap = (containers[i].max.x-containers[i].min.x)*0.25f;
				pos.x = Random.Range(containers[i].min.x+gap,containers[i].max.x-gap);
			}
			
			shapes.Add( (GameObject.Instantiate(tmp,pos,Quaternion.identity) as GameObject).GetComponent<Shape400>());
			shapes[i].value = sizes[i];
			shapes[i].setSizeByInt(sizes[i]);
			shapes[i].setColorByInt(color);
		}
		
		//Placeholders y contenedores
		
		//Ordenamos los valores
		sizes.Sort ();
		
		for(int i = 0; i < containers.Length; i++)
		{
			Vector3 pos = new Vector3(containers[i].getCenter().x,
			                          containers[i].getCenter().y,0);
			
			containers[i].secondValue = sizes[i];
			
			placeholders.Add( (GameObject.Instantiate(tmp,pos,Quaternion.identity) as GameObject).GetComponent<Shape400>());
			placeholders[i].value = sizes[i];
			placeholders[i].setSizeByInt(sizes[i]);
			placeholders[i].setColorByInt(color);
			placeholders[i].enabled(false);
			placeholders[i].alpha = 0.4f;
			
			//Los quito de la capa arrastrable
			placeholders[i].gameObject.layer = LayerMask.NameToLayer("Background");
			for(int ii = 0; ii < placeholders[i].transform.childCount; ii++)
			{
				placeholders[i].transform.GetChild(ii).gameObject.layer = LayerMask.NameToLayer("Background");
			}
			
			containers[i].value = sizes[i];
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
			//index = 1;
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
	}
	
	public void onFinishExcercise()
	{
		finishButton.interactable = false;
		checkAndShowIfExcerciseIsCorrect();
	}
	
	protected void checkAndShowIfExcerciseIsCorrect()
	{
		List<Shape400> badShapes = new List<Shape400>();
		
		foreach(Shape400 s in shapes)
		{
			if(!s.container)
			{
				badShapes.Add(s);
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
			List<Shape400> first	= new List<Shape400>();
			List<Shape400> second	= new List<Shape400>();
			List<Shape400> refList;
			
			foreach(Shape400 s in shapes)
			{
				if(s.value == s.container.value)
				{
					first.Add(s);
				}
				else if(s.value == s.container.secondValue)
				{
					second.Add (s);
				}
				else
				{
					badShapes.Add(s);
				}
			}
			
			if(first.Count > second.Count)
			{
				refList = second;
			}
			else
			{
				refList = first;
			}
			
			badShapes.AddRange(refList);
			
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
			}
		}
	}
	
	protected bool isCorrect()
	{
		foreach(Shape400 s in shapes)
		{
			if(!s.container)
			{
				return false;
			}
		}
		
		List<Shape400> first	= new List<Shape400>();
		List<Shape400> second	= new List<Shape400>();
		List<Shape400> refList;
		
		foreach(Shape400 s in shapes)
		{
			if(s.value == s.container.value)
			{
				first.Add(s);
			}
			else if(s.value == s.container.secondValue)
			{
				second.Add (s);
			}
			else
			{
				return false;
			}
		}
		
		if(first.Count != containers.Length && second.Count != containers.Length)
		{
			return false;
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
				if(containers[i].Contains(input.selected.transform.position))
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
