using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Pencil : MonoBehaviour {

	public GameObject Switch2EraseBtn;
	public GameObject Switch2PaintBtn;
	public GameObject EreaseAllBtn;
	
	protected GameObject QuestionBtn;
	protected bool showing;

	public bool startOpen = false;
	// Use this for initialization
	void Start () {
		//activateDrawing();
		QuestionBtn = GameObject.Find("ClueBtn");
		Switch2EraseBtn.SetActive(false);
		Switch2PaintBtn.SetActive(false);
		EreaseAllBtn.SetActive(false);
		moveBtn(true);

		if(startOpen)
		{
			StartCoroutine("startOpening");
		}
	}

	public void activateDrawing()
	{
		Switch2EraseBtn.SetActive(true);
		Switch2PaintBtn.SetActive(true);
		EreaseAllBtn.SetActive(true);
		if(showing)
		{
			moveBtn(true);
			showing=false;
			if(QuestionBtn&&!startOpen)
				QuestionBtn.GetComponent<Button>().interactable = true;
		}
		else
		{
			showing=true;
			moveBtn(false);
			if(QuestionBtn&&!startOpen)
				QuestionBtn.GetComponent<Button>().interactable = false;
		}
	}

	protected void moveBtn(bool hide,float delay = 0.25f)
	{
		float val = -Screen.height*0.1255f;
		if(hide)
		{
			Switch2PaintBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-val ),delay);
			Switch2EraseBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-val-val ),delay);
			EreaseAllBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-val-val-val ),delay);
		}
		else
		{
			Switch2EraseBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay).SetEase(Ease.OutBack);
			Switch2PaintBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay).SetEase(Ease.OutBack);
			EreaseAllBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay).SetEase(Ease.OutBack);
		}
	}

	public void avtivateQuestion()
	{
		QuestionBtn.GetComponent<Button>().interactable = QuestionBtn.GetComponent<Button>().interactable == true ? false: true;
	}

	IEnumerator startOpening()
	{
		yield return new WaitForSeconds(.2f);
		activateDrawing();
		DrawingInput a = FindObjectOfType<DrawingInput>();
		a.GetComponent<DrawingInput>().change2Draw();
		a.GetComponent<DrawingInput>().drawingTrue();
	}
}
