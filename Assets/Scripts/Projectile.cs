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
	public Vector3 oldPosition;
	public Vector3 newPosition;
	public float startedMoving;
	public float endMoving;
	public float moveSpeed;

	void Start() {
		startedMoving = Time.time;
		moveSpeed = 0.4f;

	}
	
	void Update() {
		List<GameObject> canvas = GameManager.floor.GetObjectsOfTypes(gridCoords, attack);
	
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
		Move(dir);
		AnimateMotion();
	}
	
	/*
	 * Move by x and y in the game grid. 
	 */
	public void Move(Vector2 coords) {
	//	if(GameManager.Move(gridCoords, gridCoords + coords, gameObject))
	//	{
			gridCoords += coords;
			startedMoving = Time.time;
			endMoving = startedMoving + moveSpeed;
			oldPosition = transform.position;
			newPosition = new Vector3(gridCoords.x, gridCoords.y, 0);
	//	}
	}

	/*
	 * For smooth motion animation
	 */
	public void AnimateMotion() {
		if(Time.time > endMoving) {
			return;
		}
		float time = (Time.time - startedMoving)/moveSpeed;
		transform.position = Vector3.Lerp(oldPosition, newPosition, time);
	}

	public static GameObject MakeProj(Vector2 gridCoords, Vector3 pos, Vector2 dir, Color col, GameObject cameFrom) {
		GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		proj.transform.localScale = new Vector3(.5f, .5f, .5f);
		proj.name = "Projectile";
		proj.transform.position = pos;
		Projectile script = proj.AddComponent<Projectile>();
		script.gridCoords = gridCoords;
		script.dir = dir;
		script.colorPainted = col;
		proj.renderer.material.color = col;
		script.cameFrom = cameFrom; 
		int x = (int) gridCoords.x;
		int y = (int) gridCoords.y;
		GameManager.floor.Add(proj, x, y);
		return proj;
	}

}
