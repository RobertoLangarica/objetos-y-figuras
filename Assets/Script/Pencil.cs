using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Pencil : MonoBehaviour {

	public GameObject Switch2EraseBtn;
	public GameObject Switch2PaintBtn;
	public GameObject EreaseAllBtn;
	// Use this for initialization
	void Start () {
		//activateDrawing();
		Switch2EraseBtn.SetActive(false);
		Switch2PaintBtn.SetActive(false);
		EreaseAllBtn.SetActive(false);

	}

	public void activateDrawing()
	{
		if(Switch2EraseBtn.activeSelf)
		{
			moveBtn(true);
		}
		else
		{
			Switch2EraseBtn.SetActive(true);
			Switch2PaintBtn.SetActive(true);
			EreaseAllBtn.SetActive(true);
			moveBtn(false);
		}
	}

	protected void moveBtn(bool hide,float delay = .25f)
	{
		float val = -Screen.height*0.15f;
		if(hide)
		{
			Switch2PaintBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-val ),delay).OnComplete(deactivate);
			Switch2EraseBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-val-val ),delay);
			EreaseAllBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,-val-val-val ),delay);
		}
		else
		{
			Switch2EraseBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay);
			Switch2PaintBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay);
			EreaseAllBtn.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0) ,delay);
		}
	}

	protected void deactivate()
	{
		Switch2EraseBtn.SetActive(false);
		Switch2PaintBtn.SetActive(false);
		EreaseAllBtn.SetActive(false);
	}
}
