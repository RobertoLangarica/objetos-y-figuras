using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class RemotePlayer
{
	public string playerName;
	public int pos;

	protected string sName = "";
	protected string _photo = "";
	public ShipControler ship;
	
	public string shipName
	{
		set
		{
			if(sName == value)
			{
				return;
			}

			sName = value;

			if(ship)
			{
				GameObject.Destroy(ship.gameObject);
			}

			GameObject tmp = (GameObject)Resources.Load("Space/"+sName+"_space");
			ship = ((GameObject)GameObject.Instantiate(tmp
			                               ,ShipControler.getPosition(pos)
			                               ,Quaternion.identity)).GetComponent<ShipControler>();
			ship.setDepth(pos);

			setPhotoOnShip();
		}
		get{return sName;}
	}

	public void destroy()
	{
		if(ship)
		{
			GameObject.Destroy(ship.gameObject);
		}

		_photo = "";
	}

	public string photo
	{
		set
		{
			_photo = value;

			if(ship)
			{
				setPhotoOnShip();
				ship.setPosition(pos);
			}
		}
		get{return _photo;}
	}

	public void setPhotoOnShip()
	{
		SpriteRenderer sp =  ship.transform.GetChild(0).GetComponent<SpriteRenderer>();
		byte[] bytes = System.Convert.FromBase64String(_photo);

		Texture2D texture = new Texture2D(1,1);
		texture.LoadImage(bytes);

		sp.sprite = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(0.5f,0.5f));
	}
}

