using UnityEngine;
using System.Collections.Generic;

public class Conveyor : MonoBehaviour {

	public Vector2 startCoords; // the start of the conveyor belt (unneccessary?)
	public Vector2 endCoords; // the end of the conveyor belt
	public Vector2 direction; // the direction in which the conveyor belt moves things
	public float speed; // the speed at which the conveyor belt moves things
	public Vector2[] cells; // the cells in the Grid that the conveyor occupies
	public Vector3 wloc; // the world coordinates of the conveyor
	public List<string> moveableObjects; // the types of objects that the conveyor can move
	
	public Dictionary<GameObject, GameObjectAnimation> currentObjects;

	public bool switchable;
	public float switchRate;
	public float lastSwitched;
	
	public struct GameObjectAnimation {
		public float startedMoving;
		public float endMoving;
		public Vector3 oldPosition;
		public Vector3 newPosition;

		/*
		 * For storing all the relevant info to animate the motion for this object.
		 * The floats are the start and end times, the Vector2s are the positions.
		 */
		public GameObjectAnimation(float start, float end, Vector3 old, Vector3 current) {
			startedMoving = start;
			endMoving = end;
			oldPosition = old;
			newPosition = current;
		}
	}

	void Start () {
		moveableObjects = new List<string>();
		moveableObjects.Add("Player");
		moveableObjects.Add("Robot");
		currentObjects = new Dictionary<GameObject, GameObjectAnimation>();
		lastSwitched = Time.time;
	}
	
	void Update () {
		for(int i = 0; i < cells.Length; i++) {
			List<GameObject> objects = GameManager.floor.GetObjectsOfTypes(cells[i], moveableObjects);
			foreach(GameObject obj in objects) {
				if(currentObjects.ContainsKey(obj)) {
					continue;
				}
				if(GameManager.Move(cells[i], cells[i] + direction, obj)) {
					Vector3 newPosition;
					Vector3 oldPosition;
					if(obj.GetComponent<Player>() != null) {
						Player p = obj.GetComponent<Player>();
						oldPosition = new Vector3(p.gridCoords.x, p.gridCoords.y, 0);
						p.gridCoords += direction;
						newPosition = new Vector3(p.gridCoords.x, p.gridCoords.y, 0);
					}
					else {
						Robot r = obj.GetComponent<Robot>();
						oldPosition = new Vector3(r.gridCoords.x, r.gridCoords.y, 0);
						r.gridCoords += direction;
						newPosition = new Vector3(r.gridCoords.x, r.gridCoords.y, 0);
					}
					currentObjects.Add(obj, new GameObjectAnimation(Time.time, Time.time + speed, oldPosition, newPosition));
				}
			}
			// Slight jank solution to get around not being able to modify stuff while
			// iterating over it goes HERE.
			List<GameObject> toRemove = new List<GameObject>();
			foreach(KeyValuePair<GameObject, GameObjectAnimation> kvp in currentObjects) {
				if(AnimateMotion(kvp.Key, kvp.Value)) {
					toRemove.Add(kvp.Key);
				}
			}
			foreach(GameObject obj in toRemove) {
				currentObjects.Remove(obj);
			}
			// end jank
		}

		if(switchable) {
			if(Time.time - lastSwitched > switchRate)
				direction = direction * -1.0f;
		}
	}

	/*
	 * This returns true if the animation for this GameObjectAnimation is done, false
	 * otherwise. Slightly jank!
	 */
public bool AnimateMotion(GameObject obj, GameObjectAnimation goa) {
	if(Time.time > goa.endMoving) {
		return true;
		}
		float time = (Time.time - goa.startedMoving)/speed + .1f;
		obj.transform.position = Vector3.Lerp(goa.oldPosition, goa.newPosition, time);
		return false;
	}

	public static GameObject MakeConveyor(Vector2 startCoords, Vector2 direction, float length, float speed, bool switchable = false, float switchRate = 0) {
		if(direction.x != 0 && direction.y != 0) // no diagonal conveyors
			return null;
		GameObject conveyor = GameObject.CreatePrimitive(PrimitiveType.Plane);
		conveyor.name = "Conveyor";
		Conveyor script = conveyor.AddComponent<Conveyor>();
		script.startCoords = startCoords;
		script.direction = direction;
		script.speed = speed;
		script.switchable = switchable;
		script.switchRate = switchRate;
		script.cells = new Vector2[(int)length];
		int count = 0;
		while(count < length) {
			script.cells[count] = startCoords + (count * direction);
			GameObject conveyorPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
			//I changed the following line (121) from ... + new Vector3(0f, 0f, -0.1f) to what it is now. It was making the plane go over everything else
			conveyorPlane.transform.position = GameManager.floor.grid[(int)script.cells[count].x, (int)script.cells[count].y].plane.transform.position + new Vector3(0f, 0f, 0f);
			//I kind of fixed the following line by adding new Vector3 to it to make shit slightly bigger... but it's still a little janky looking
			conveyorPlane.transform.localScale = new Vector3(0f, 0f, 0.01f) + GameManager.floor.grid[(int)script.cells[count].x, (int)script.cells[count].y].plane.transform.localScale;
			conveyorPlane.transform.Rotate(-90.0f, 0.0f, 0.0f);
			conveyorPlane.renderer.material.mainTexture = Resources.Load("Textures/Conveyor") as Texture;
	                conveyorPlane.renderer.material.shader = Shader.Find("Transparent/Diffuse");
        	        conveyorPlane.renderer.material.color = Color.white;
			conveyorPlane.name = "conveyor plane";
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
		conveyor.renderer.material.color = Color.white;
		script.wloc = conveyor.transform.position;
		conveyor.transform.Rotate(-90.0f, 0.0f, 0.0f);
		return conveyor;
	}

}
