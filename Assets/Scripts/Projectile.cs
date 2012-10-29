using UnityEngine;
using System.Collections.Generic;
using System;

public class Projectile : MonoBehaviour, IColor {

	public Vector2 gridCoords;
	public Vector2 dir;
	public Color colorPainted{ get; set; }
	public GameObject cameFrom;
	public static List<string> attack = new List<string>(new string[] {"Robot", "Player"});
	public int life = (GameManager.WIDTH > GameManager.HEIGHT) ? GameManager.WIDTH : GameManager.HEIGHT;
	public Grid grid;
	void Start() {
	}
	
	void Update() {
		List<GameObject> canvas = grid.GetObjectsOfTypes(gridCoords, attack);
	
		if(canvas != null)
		{
			foreach(GameObject obj in canvas) {
				if(obj.name == "Robot")
					obj.GetComponent<Robot>().colorPainted = colorPainted;
				else if(obj.name == "Player")
					obj.GetComponent<Player>().colorPainted = colorPainted;
			}
			Destroy(this);
		}
		life--;
		if(life == 0)
			Destroy(this);
	}
	

	public void Move(Vector2 coords) {
		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject))
		{
			gridCoords += coords;
			transform.Translate(new Vector3(coords.x, coords.y, 0));
		}
	}

	public static GameObject MakeProj(int x, int y, Vector3 pos, Vector2 dir, Color col, GameObject cameFrom, Grid grid) {
		GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		proj.transform.localScale = new Vector3(.0625f, .0625f, .0625f);
		proj.name = "Projectile";
		proj.transform.position = pos;
		Projectile script = proj.AddComponent<Projectile>();
		script.gridCoords = new Vector2(x, y);
		script.dir = dir;
		script.colorPainted = col;
		script.cameFrom = cameFrom; 
		script.grid = grid;
		return proj;
	}

}
