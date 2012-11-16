using UnityEngine;
using System;

public class Paintball : Projectile, IColor {
	
	private Color _colorPainted;
	public Color colorPainted
	{
		get { return _colorPainted; }
		set {
			renderer.material.color = value;
			_colorPainted = value;
		}
	}
	
	public override void Hit(GameObject obj) {
		if(obj.GetComponent<Robot>() != null) {
			obj.GetComponent<Robot>().colorPainted = colorPainted;
			Destroy(gameObject);
		}
		else if(obj.GetComponent<Player>() != null) {
			obj.GetComponent<Player>().colorPainted = colorPainted;
			Destroy(gameObject);
		}
		else if(obj.GetComponent<Wall>() != null) {
			Destroy(gameObject);
		}
		else if(obj.GetComponent<DestructibleWall>() != null) {
			obj.GetComponent<DestructibleWall>().colorPainted = colorPainted;
			Destroy(gameObject);
		}
	}

	public static GameObject MakePaintball(Vector3 pos, Vector2 dir, Color col, GameObject cameFrom) {
		GameObject paintball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		paintball.transform.localScale = new Vector3(.25f, .25f, .25f);
		paintball.name = "Paintball";
		paintball.transform.position = pos;
		Paintball script = paintball.AddComponent<Paintball>();
		script.dir = dir;
		script.colorPainted = col;
		script.moveSpeed = 10.0f;
		Destroy(paintball, 2.0f); // life time
		script.cameFrom = cameFrom;
		return paintball;
	}
}
