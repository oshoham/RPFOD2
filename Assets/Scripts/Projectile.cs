using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Projectile : MonoBehaviour {

	//public Vector2 gridCoords;
	public Vector2 dir;
	//public Color colorPainted{ get; set; }
	public GameObject cameFrom;
	//	public static List<string> attack = new List<string>(new string[] {"Robot", "Player"});
	// public Vector3 oldPosition;
	// public Vector3 newPosition;
	// public float startedMoving;
	// public float endMoving;
	public float moveSpeed;
	public float lifeTime;

	void Start() {
		//		startedMoving = Time.time;
	}
	
	void Update() {
		// if(Time.time > startedMoving + lifeTime) {
		// 	Destroy(gameObject);
		// }
		// List<GameObject> canvas = GameManager.floor.GetObjectsOfTypes(gridCoords, attack);
		// if(canvas.Count > 0)
		// {
		// 	foreach(GameObject obj in canvas) {
		// 		print(obj);
		// 		if(obj == cameFrom)
		// 			continue;
		// 		if(obj.name == "Robot")
		// 			obj.GetComponent<Robot>().colorPainted = colorPainted;
		// 		else if(obj.name == "Player")
		// 			obj.GetComponent<Player>().colorPainted = colorPainted;
		// 	}
		// 	Destroy(gameObject);
		// }
		//Move(dir);
		transform.Translate(dir * moveSpeed * Time.deltaTime);
		CheckPosition();
		//AnimateMotion();
	}
	
	/*
	 * Move by x and y in the game grid. 
	 */
	// public void Move(Vector2 coords) {
		//		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject)) {
		//	gridCoords += coords;
		//	startedMoving = Time.time;
		//	endMoving = startedMoving + moveSpeed;
		//	oldPosition = transform.position;
		//	newPosition = new Vector3(gridCoords.x, gridCoords.y, 0);
			//		}
		//}
	
	public abstract void Hit(GameObject obj);
	
	/*
	 * Check if we've hit anything.
	 */
	public void CheckPosition() {
		Collider[] colliders = Physics.OverlapSphere(transform.position, .1f);
		foreach(Collider collider in colliders) {
			GameObject obj = collider.gameObject;
			if(obj == cameFrom)
				continue;
			Hit(obj);
		}
	}
	
	/*
	 * For smooth motion animation
	 */
	// public void AnimateMotion() {
	// 	if(Time.time > endMoving) {
	// 		return;
	// 	}
	// 	float time = (Time.time - startedMoving)/moveSpeed;
	// 	transform.position = Vector3.Lerp(oldPosition, newPosition, time);
	// }

	// public static GameObject MakeProj(Vector2 gridCoords, Vector3 pos, Vector2 dir, Color col, GameObject cameFrom) {
	// 	GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
	// 	proj.transform.localScale = new Vector3(.5f, .5f, .5f);
	// 	proj.name = "Projectile";
	// 	proj.transform.position = pos;
	// 	Projectile script = proj.AddComponent<Projectile>();
	// 	//script.gridCoords = gridCoords;
	// 	script.dir = dir;
	// 	script.colorPainted = col;
	// 	script.moveSpeed = 10.0f;
	// 	script.lifeTime = 2.0f;
	// 	proj.renderer.material.color = col;
	// 	script.cameFrom = cameFrom; 
	// 	//int x = (int) gridCoords.x;
	// 	//int y = (int) gridCoords.y;
	// 	//GameManager.floor.Add(proj, x, y);
	// 	return proj;
	// }

}
