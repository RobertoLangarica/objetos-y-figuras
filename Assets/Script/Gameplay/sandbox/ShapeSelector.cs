using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ShapeSelector : MonoBehaviour {

	public float rotateAmount = 15;

	public GameObject square;
	public GameObject rectangle;
	public GameObject triangle;
	public GameObject rhomboid;
	public GameObject trapezium;
	public GameObject hexagon;
	public GameObject pentagon;
	public GameObject circle;

	public Image img_square;
	public Image img_rectangle;
	public Image img_triangle;
	public Image img_rhomboid;
	public Image img_trapezium;
	public Image img_hexagon;
	public Image img_pentagon; 
	public Image img_circle; 

	public ColorSelector colorSelector;
	public TangramInput input;
	public RectTransform menuArea;

	protected Rect menuRect;
	protected bool waitingForCompleteDragOnCreated = false;
	protected int selectedColor;

	void Start()
	{
		input.onDragFinish+=onDragFinish;

		Vector3[] corners = new Vector3[4];

		//Area para las figuras (aqui aparecen)
		menuArea.GetWorldCorners(corners);
		menuRect = new Rect();
		menuRect.xMin = corners[0].x;
		menuRect.xMax = corners[2].x;
		menuRect.yMin = corners[0].y;
		menuRect.yMax = corners[1].y;
	}

	public void instantiateShape(string name)
	{

		if(waitingForCompleteDragOnCreated)
		{
			return;
		}

		GameObject shape = null;
		Image reference = null;

		waitingForCompleteDragOnCreated = true;

		switch(name)
		{
		case "square":
			shape = (GameObject.Instantiate(square) as GameObject);
			reference = img_square;
			break;
		case "rectangle":
			shape = (GameObject.Instantiate(rectangle) as GameObject);
			reference = img_rectangle;
			break;
		case "triangle":
			shape = (GameObject.Instantiate(triangle) as GameObject);
			reference = img_triangle;
			break;
		case "rhomboid":
			shape = (GameObject.Instantiate(rhomboid) as GameObject);
			reference = img_rhomboid;
			break;
		case "trapezium":
			shape = (GameObject.Instantiate(trapezium) as GameObject);
			reference = img_trapezium;
			break;
		case "hexagon":
			shape = (GameObject.Instantiate(hexagon) as GameObject);
			reference = img_hexagon;
			break;
		case "pentagon":
			shape = (GameObject.Instantiate(pentagon) as GameObject);
			reference = img_pentagon;
			break;
		case "circle":
			shape = (GameObject.Instantiate(circle) as GameObject);
			reference = img_circle;
			break;
		}
		input.ignoreNextRotation = true;

		input.selected = shape.GetComponent<SandboxShape>();
		Sprite sprite = input.selected.spriteRenderer.sprite;

		input.selected.sortingLayer = "SelectedShape";
		input.onDragFinish += shape.GetComponent<SandboxShape>().onDragFinish;
		input.selected.color = (BaseShape.EShapeColor)colorSelector.selectColor(selectedColor);

		/*float u = (Camera.main.orthographicSize*2*sprite.pixelsPerUnit)/Screen.height;
		Vector3 size = sprite.bounds.size * sprite.pixelsPerUnit;
		float s = Mathf.Min((reference.rectTransform.sizeDelta.y*u)/size.y
		                    ,(reference.rectTransform.sizeDelta.x*u)/size.x);
		shape.transform.localScale = new Vector3(s,s,1);*/
		Vector3 pos = Camera.main.ScreenToWorldPoint(reference.transform.position);
		pos.z = 0;
		shape.transform.position = pos;
	}

	public void onDragFinish()
	{
		waitingForCompleteDragOnCreated = false;

		if(input.selected != null)
		{
			Vector3 pos = Camera.main.WorldToScreenPoint(input.selected.transform.position);

			//if(pos.x < Screen.width*.2f)
			if(pos.x < menuRect.xMax)
			{
				input.onDragFinish -= input.selected.gameObject.GetComponent<SandboxShape>().onDragFinish;
				GameObject.Destroy(input.selected.gameObject);
			}
			else
			{
				//Pixel perfect position
				pos.x = Mathf.Round(pos.x);
				pos.y = Mathf.Round(pos.y);
				pos = Camera.main.ScreenToWorldPoint(pos);
				pos.z = input.selected.transform.position.z;
				input.selected.transform.position = pos;
			}
		}
	}

	public void selectColorToSelectedColor(int index)
	{
		selectedColor = index;
	}
}
