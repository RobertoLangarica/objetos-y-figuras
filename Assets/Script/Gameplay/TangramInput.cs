using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TangramInput : MonoBehaviour {

	public string selectedLayerName;
	public string normalLayerName;
	public bool rotateOnlyWhenSelected = false;
	public bool allowRotation = true;
	public AudioSource dragSound;

	[HideInInspector]
	protected BaseShape _selected;
	[HideInInspector]
	public bool ignoreNextRotation = false;

	//Control de propiedades visuales como posicionamiento, rotacion y profundidad
	protected Vector3 pos;
	protected float cu;
	protected int _sort;
	protected bool rotating = false;
	protected Vector3 initVector;
	protected Vector3 currentVector;
	protected Vector3 initialRotation;

	//Para controlar cuando el gesto viene de mas de un dedo
	protected bool isDragging = false;
	protected int lastDragFrame;
	protected int fingerCount = 0;
	protected Dictionary<int,BaseShape> multipleFingersSelection;
	protected Dictionary<int,BaseShape> multipleFingersRotateSelection;

	//Para notificar estados del drag a otros objetos
	public delegate void DOnDragFinish();
	public delegate void DOnDrag();
	[HideInInspector]
	public DOnDragFinish onDragFinish;
	[HideInInspector]
	public DOnDrag onAnyDrag;

	//Para el audio
	protected float elapsedDragTime;
	protected float dragSpeed;
	

	void Awake()
	{
		cu = (Camera.main.orthographicSize*2)/Screen.height;
		_sort = -1;

		onDragFinish = foo;
		onAnyDrag = foo;
	}

	void Start()
	{
		multipleFingersSelection = new Dictionary<int, BaseShape>();
		multipleFingersRotateSelection = new Dictionary<int, BaseShape>();
		lastDragFrame = -1;
		if(dragSound)
		{
			dragSound.pitch = 0;
		}
	}

	protected void foo(){}
	
	public int nextSort
	{
		get
		{
			if(_sort == int.MaxValue)
			{
				_sort = -1;
			}
			
			return ++_sort;
		}
	}

	public BaseShape selected {
		get {
			return _selected;
		}
		set {
			if(_selected != null)
			{
				_selected.translateHandler = false;
				_selected.rotateHandler = false;
			}
			_selected = value;
		}
	}

	void OnDrag(DragGesture gesture) 
	{
		//Solo se ejecuta una vez por frame (para que el multifinger funcione sin encimarse)
		if(lastDragFrame == Time.frameCount)
		{
			return;
		}

		lastDragFrame = Time.frameCount;

		onAnyDrag();

		switch(gesture.Phase)
		{
		case ContinuousGesturePhase.Started:
			isDragging = true;
			bool checkMultiFinger = false;

			if(gesture.Raycast.Hits2D != null && gesture.Raycast.Hits2D.Length > 0)
			{
				if(dragSound)
				{
					dragSound.Play();
					elapsedDragTime = 0;
				}

				//Tiene mas prioridad la seleccion por multidedo
				if(fingerCount > 1)
				{
					BaseShape forMove;
					forMove = getSelectedFromFingers();

					if(forMove != null)
					{
						//Por si se deselecciona
						if(_selected != null)
						{
							_selected.translateHandler = false;
							_selected.rotateHandler = false;
						}

						_selected = forMove;
						rotating = false;
						return;
					}
				}

				Array.Sort(gesture.Raycast.Hits2D,delegate(RaycastHit2D hit1, 
				                                           RaycastHit2D hit2) {
					if(hit1.collider)
					{
						if(hit2.collider)
						{
							BaseShape obj1;
							BaseShape obj2;

							if(hit1.collider.gameObject.name.Equals("move"))
							{
								obj1 = hit1.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
							}
							else
							{
								obj1 = hit1.collider.gameObject.GetComponent<BaseShape>();
							}

							if(hit2.collider.gameObject.name.Equals("move"))
							{
								obj2 = hit2.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
							}
							else
							{
								obj2 = hit2.collider.gameObject.GetComponent<BaseShape>();
							}
							
							return -obj1.spriteRenderer.sortingOrder.CompareTo(obj2.spriteRenderer.sortingOrder);
						}
						else
						{
							return -1;
						}
					}
					
					return 1;
					
				});
				
				BaseShape firstMove = null;
				BaseShape firstRotate = null;
				bool move = false;//Indica si el movimiento fue primero que la rotacion
				rotating = allowRotation;
				
				foreach(RaycastHit2D hit in gesture.Raycast.Hits2D)
				{
					if(hit.collider)
					{
						if(firstMove == null && hit.collider.gameObject.name.Equals("move"))
						{
							firstMove = hit.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
							
							if(firstRotate == null)
							{
								move = true;
							}
						}
						else if(firstRotate == null && allowRotation && !hit.collider.gameObject.name.Equals("move"))
						{
							firstRotate = hit.collider.gameObject.GetComponent<BaseShape>();
						}
					}
					
					if(firstMove != null && firstRotate != null)
					{
						break;
					}
				}

				//Alguien se mueve o se rota
				if(firstMove != null || firstRotate != null)
				{
					BaseShape newSelected = null;

					if(_selected == null)
					{
						//si no hay nada seleccionado el movimiento tiene prioridad
						move = true;
					}
					
					if(firstMove != null && firstRotate == null)
					{
						//Se mueve
						newSelected = firstMove;
						rotating = false;
					}
					else if(firstRotate != null && firstMove == null)
					{
						//Se rota
						if(ignoreNextRotation)
						{
							newSelected = firstRotate;
							rotating = false;
						}
						else 
						{
							if((rotateOnlyWhenSelected && firstRotate.rotateHandler) || !rotateOnlyWhenSelected)
							{
								newSelected = firstRotate;
								rotating = true;
								initVector = Camera.main.ScreenToWorldPoint(new Vector3(gesture.StartPosition.x,gesture.StartPosition.y))
									- newSelected.transform.position;
								initVector.z = 0;
								initialRotation = newSelected.transform.eulerAngles;
							}
						}
					}
					else if(firstMove.GetInstanceID() == firstRotate.GetInstanceID() || move)
					{
						//Se mueve
						newSelected = firstMove;
						rotating = false;
					}
					else
					{
						//Se rota
						if(ignoreNextRotation)
						{
							newSelected = firstRotate;
							rotating = false;
						}
						else 
						{
							if((rotateOnlyWhenSelected && firstRotate.rotateHandler) || !rotateOnlyWhenSelected)
							{
								newSelected = firstRotate;
								rotating = true;
								initVector = Camera.main.ScreenToWorldPoint(new Vector3(gesture.StartPosition.x,gesture.StartPosition.y))
									- newSelected.transform.position;
								initVector.z = 0;
								initialRotation = newSelected.transform.eulerAngles;
								
								
							}
						}
					}

					if(newSelected != null)
					{
						//quitamos el selected anterior
						//Por si se deselecciona
						if(_selected != null)
						{
							_selected.translateHandler = false;
							_selected.rotateHandler = false;
						}

						_selected = newSelected;
						_selected.sortingLayer = selectedLayerName;
					}
					else if(_selected != null)
					{
						//Esta seleccionado con mas dedos?
						checkMultiFinger = true;
					}

				}
				else if(_selected != null)
				{
					//Esta seleccionado con mas dedos?
					checkMultiFinger = true;
				}
			}
			else if(_selected != null)
			{
				//Esta seleccionado con mas dedos?
				checkMultiFinger = true;
			}

			//Si no hay nadie seleccionado quiza el multifinger lo tenga
			if(checkMultiFinger || _selected == null)
			{
				rotating = false;
				BaseShape forMove;
				forMove = getSelectedFromFingers();

				//Por si se deselecciona
				if(_selected != null)
				{
					_selected.translateHandler = false;
					_selected.rotateHandler = false;
				}

				if(forMove != null)
				{
					_selected = forMove;
				}
				else if(allowRotation)
				{
					//Rotando?
					_selected = getSelectedFromRotatingFingers();
					rotating = _selected != null;
				}
				else
				{
					//Nada seleccionado
					_selected = null;
				}
			}
			break;
			
		case ContinuousGesturePhase.Updated:
			if(_selected != null )
			{
				if(dragSound)
				{
					if((gesture.ElapsedTime-elapsedDragTime) == 0)
					{
						dragSpeed = gesture.DeltaMove.sqrMagnitude;
						dragSound.pitch = percent(0,9000000,dragSpeed)*3;
						elapsedDragTime = gesture.ElapsedTime;
					}
					else
					{
						dragSpeed = gesture.DeltaMove.sqrMagnitude/((gesture.ElapsedTime-elapsedDragTime)*(gesture.ElapsedTime-elapsedDragTime));
						dragSound.pitch = percent(0,9000000,dragSpeed)*3;
						elapsedDragTime = gesture.ElapsedTime;
					}
				}

				if(gesture.DeltaMove != Vector2.zero)
				{
					if(rotating)
					{
						currentVector = Camera.main.ScreenToWorldPoint(new Vector3(gesture.Position.x,gesture.Position.y))
							- _selected.transform.position;
						currentVector.z = initVector.z;
						float angle = Vector3.Angle(initVector,currentVector)*(Vector3.Cross(initVector,currentVector).z > 0 ? 1:-1);
						
						_selected.transform.eulerAngles = new Vector3(0,0
						                                             ,initialRotation.z + angle);

						_selected.rotateHandler = allowRotation;
						_selected.translateHandler = false;
					}
					else
					{
						pos = new Vector3(_selected.transform.position.x + gesture.DeltaMove.x*cu
						                  ,_selected.transform.position.y + gesture.DeltaMove.y*cu
						                  ,_selected.transform.position.z);
						_selected.transform.position = pos;

						_selected.rotateHandler = false;
						_selected.translateHandler = true;
					}
				}
			}
			break;
		case ContinuousGesturePhase.Ended:
			isDragging = false;
			if(dragSound)
			{
				dragSound.Stop();
				dragSound.pitch = 0;
			}

			if(_selected != null)
			{
				_selected.sortingLayer = normalLayerName;
				_selected.sortingOrder = nextSort;
				_selected.rotateHandler = allowRotation;
				_selected.translateHandler = true;
			}
			onDragFinish();
			ignoreNextRotation = false;
			//_selected = null;
			break;
			
		}
	}

	void OnTwist(TwistGesture gesture) 
	{
		if(lastDragFrame == Time.frameCount)
		{
			return;
		}
		
		lastDragFrame = Time.frameCount;


		switch(gesture.Phase)
		{
			case ContinuousGesturePhase.Updated:
				if(_selected != null)
				{
					_selected.transform.eulerAngles = new Vector3(0,0
					                                              ,_selected.transform.eulerAngles.z + gesture.DeltaRotation);
				}
				break;
		}
	}

	float percent(float min, float max, float value)
	{
		if(value >= max)
		{
			return 1;
		}

		if(value <= min)
		{
			return 0;
		}

		return (value-min)/(max-min);
	}

	void OnTap(TapGesture gesture) 
	{
		if(gesture.Raycast.Hits2D != null && gesture.Raycast.Hits2D.Length > 0)
		{
			Array.Sort(gesture.Raycast.Hits2D,delegate(RaycastHit2D hit1, 
			                                           RaycastHit2D hit2) {
				if(hit1.collider)
				{
					if(hit2.collider)
					{
						BaseShape obj1;
						BaseShape obj2;
						
						if(hit1.collider.gameObject.name.Equals("move"))
						{
							obj1 = hit1.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
						}
						else
						{
							obj1 = hit1.collider.gameObject.GetComponent<BaseShape>();
						}
						
						if(hit2.collider.gameObject.name.Equals("move"))
						{
							obj2 = hit2.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
						}
						else
						{
							obj2 = hit2.collider.gameObject.GetComponent<BaseShape>();
						}
						
						return -obj1.spriteRenderer.sortingOrder.CompareTo(obj2.spriteRenderer.sortingOrder);
					}
					else
					{
						return -1;
					}
				}
				
				return 1;
				
			});
			
			BaseShape first = null;
			rotating = true;
			
			foreach(RaycastHit2D hit in gesture.Raycast.Hits2D)
			{
				if(hit.collider)
				{
					if(hit.collider.gameObject.name.Equals("move"))
					{
						if(_selected != null)
						{
							_selected.translateHandler = false;
							_selected.rotateHandler = false;
						}
						
						_selected = hit.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
						_selected.sortingOrder = nextSort;
						_selected.translateHandler = true;
						_selected.rotateHandler = allowRotation;
						return;
					}
				}
			}
		}

		//Cuenta como vacio el presionar en el area de rotar de otro
		if(_selected != null)
		{
			_selected.translateHandler = false;
			_selected.rotateHandler = false;
			_selected = null;
		}
	}

	void OnFingerDown(FingerDownEvent e) 
	{
		fingerCount++;
		if(e.Raycast.Hits2D != null && e.Raycast.Hits2D.Length > 0)
		{
			Array.Sort(e.Raycast.Hits2D,delegate(RaycastHit2D hit1, 
			                                           RaycastHit2D hit2) {
				if(hit1.collider)
				{
					if(hit2.collider)
					{
						BaseShape obj1;
						BaseShape obj2;
						
						if(hit1.collider.gameObject.name.Equals("move"))
						{
							obj1 = hit1.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
						}
						else
						{
							obj1 = hit1.collider.gameObject.GetComponent<BaseShape>();
						}
						
						if(hit2.collider.gameObject.name.Equals("move"))
						{
							obj2 = hit2.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>();
						}
						else
						{
							obj2 = hit2.collider.gameObject.GetComponent<BaseShape>();
						}
						
						return -obj1.spriteRenderer.sortingOrder.CompareTo(obj2.spriteRenderer.sortingOrder);
					}
					else
					{
						return -1;
					}
				}
				
				return 1;
				
			});

			BaseShape first = null;
			bool addeRotator = false;
			foreach(RaycastHit2D hit in e.Raycast.Hits2D)
			{
				if(hit.collider)
				{
					if(hit.collider.gameObject.name.Equals("move"))
					{
						multipleFingersSelection.Add(e.Finger.Index,hit.collider.gameObject.transform.parent.gameObject.GetComponent<BaseShape>());
						break;
					}
					else if(!addeRotator)
					{
						addeRotator = true;
						multipleFingersRotateSelection.Add(e.Finger.Index,hit.collider.gameObject.GetComponent<BaseShape>());
					}
				}
			}


		}
	}

	void OnFingerUp(FingerUpEvent e) 
	{
		fingerCount--;
		multipleFingersRotateSelection.Remove(e.Finger.Index);
		multipleFingersSelection.Remove(e.Finger.Index);
	}

	protected BaseShape getSelectedFromFingers()
	{
		if(_selected != null)
		{
			//El seleccionado tiene prioridad
			foreach(BaseShape s in multipleFingersSelection.Values)
			{
				if(s.GetInstanceID() == _selected.GetInstanceID())
				{
					return _selected;
				}
			}
		}

		//no supe de que otra forma acceder al primero
		foreach(BaseShape s in multipleFingersSelection.Values)
		{
			return s;
		}

		return null;
	}

	protected BaseShape getSelectedFromRotatingFingers()
	{
		//solo se rota el seleccionado
		if(_selected != null)
		{
			//El seleccionado tiene prioridad
			foreach(BaseShape s in multipleFingersRotateSelection.Values)
			{
				if(s.GetInstanceID() == _selected.GetInstanceID())
				{
					return _selected;
				}
			}
		}

		return null;
	}
}
