using UnityEngine;
using System.Collections.Generic;

public class Conveyor : MonoBehaviour {

	public Vector2 startCoords; // the start of the conveyor belt (unneccessary?)
	public Vector2 endCoords; // the end of the conveyor belt
	public Vector2 direction; // the direction in which the conveyor belt moves things
	public float speed; // the speed at which the conveyor belt moves things
	public Vector2[] cells; // the cells in the Grid that the conveyor occupies
	public Vector3 wloc; // the world coordinates of the conveyor
	private List<string> moveableObjects;

	void Start () {
		moveableObjects = new List<string>();
		moveableObjects.Add("Player");
		moveableObjects.Add("Robot");
	}
	
	void Update () {
		for(int i = 0; i < cells.Length; i++) {
			List<GameObject> objects = GameManager.floor.GetObjectsOfTypes(cells[i], moveableObjects);
			foreach(GameObject obj in objects) {
				if(GameManager.Move(cells[i], cells[i] + direction, obj)) {
					float startedMoving = Time.time;
					float endMoving = startedMoving + speed;
					Vector3 oldPosition = obj.transform.position;
					Vector3 newPosition;
					if(obj.GetComponent<Player>() != null) {
						Player p = obj.GetComponent<Player>();
						p.gridCoords += direction;
						newPosition = new Vector3(p.gridCoords.x, p.gridCoords.y, 0);
					}
					else {
						Robot r = obj.GetComponent<Robot>();
						r.gridCoords += direction;
						newPosition = new Vector3(r.gridCoords.x, r.gridCoords.y, 0);
					}
					AnimateMotion(obj, startedMoving, endMoving, oldPosition, newPosition);
				}
			}
		}
	}

	public void AnimateMotion(GameObject obj, float startedMoving, float endMoving, Vector2 oldPosition, Vector2 newPosition) {
		if(Time.time > endMoving) {
			return;
		}
		float time = (Time.time - startedMoving)/speed;
		obj.transform.position = Vector3.Lerp(oldPosition, newPosition, time);
	}

	public static GameObject MakeConveyor(Vector2 startCoords, Vector2 direction, float length, float speed) {
		if(direction.x != 0 && direction.y != 0) // no diagonal conveyors
			return null;
		GameObject conveyor = GameObject.CreatePrimitive(PrimitiveType.Plane);
		conveyor.name = "Conveyor";
		Conveyor script = conveyor.AddComponent<Conveyor>();
		script.startCoords = startCoords;
		script.direction = direction;
		script.speed = speed;
		script.cells = new Vector2[(int)length];
		int count = 0;
		while(count < length) {
			script.cells[count] = startCoords + (count * direction);
			GameManager.floor.grid[(int)script.cells[count].x, (int)script.cells[count].y].plane.renderer.material.color = Color.grey;
			count++;
		}
		script.endCoords = script.cells[(int)length-1];
		//Vector2 middleCoords = (script.startCoords + script.endCoords)/2f;
		if(direction.x != 0)
			conveyor.transform.localScale = new Vector3(0.1f*length, 1.0f, 0.1f);
		else
			conveyor.transform.localScale = new Vector3(0.1f, 1.0f, 0.1f*length);
		//Debug.Log(GameManager.floor.grid[(int)middleCoords.x, (int)middleCoords.y].wloc);
	//	conveyor.transform.position = GameManager.floor.grid[(int)middleCoords.x, (int)middleCoords.y].wloc + new Vector3(0f, 0f, -0.1f);
		conveyor.transform.position = new Vector3(-5000, -5000, -5000);
		script.wloc = conveyor.transform.position;
		conveyor.transform.Rotate(-90.0f, 0.0f, 0.0f);
		conveyor.renderer.material.color = Color.grey; 
		return conveyor;
	}

}
