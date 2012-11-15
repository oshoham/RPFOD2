using UnityEngine;
using System.Collections.Generic;

public class Conveyor : MonoBehaviour {
	public Grid grid;

	public Vector2 startCoords; // the start of the conveyor belt (unneccessary?)
	public Vector2 endCoords; // the end of the conveyor belt
	public Vector2 direction; // the direction in which the conveyor belt moves things
	public float length; // the length of the conveyor belt
	public float speed; // the speed at which the conveyor belt moves things
	public Vector2[] cells; // the cells in the Grid that the conveyor occupies
	public Vector3 wloc; // the world coordinates of the conveyor
	public List<string> moveableObjects; // the types of objects that the conveyor can move
	
	public Dictionary<GameObject, GameObjectAnimation> currentObjects;

	public List<GameObject> planes;

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
			List<GameObject> objects = grid.GetObjectsOfTypes(cells[i], moveableObjects);
			foreach(GameObject obj in objects) {
				if(currentObjects.ContainsKey(obj)) {
					continue;
				}
				if(GameManager.Move(cells[i], cells[i] + direction, obj)) {
					Vector3 newPosition;
					Vector3 oldPosition;
					if(obj.GetComponent<Player>() != null) {
						Player p = obj.GetComponent<Player>();
						oldPosition = new Vector3(p.gridCoords.x, p.gridCoords.y, -1);
						p.gridCoords += direction;
						newPosition = new Vector3(p.gridCoords.x, p.gridCoords.y, -1);
					}
					else {
						Robot r = obj.GetComponent<Robot>();
						oldPosition = new Vector3(r.gridCoords.x, r.gridCoords.y, -1);
						r.gridCoords += direction;
						newPosition = new Vector3(r.gridCoords.x, r.gridCoords.y, -1);
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
			if(Time.time - lastSwitched > switchRate) {
				direction = direction * -1.0f;
				lastSwitched = Time.time;
			}
		}
	}
	
	void OnDisable() {
		for(int i = 0; i < planes.Count; i++) {
			Destroy(planes[i]);
		}
		grid.Remove(gameObject, (int)startCoords.x, (int)startCoords.y);
	}
	
	/*
	 * This returns true if the animation for this GameObjectAnimation is done, false
	 * otherwise. Slightly jank!
	 */
	public bool AnimateMotion(GameObject obj, GameObjectAnimation goa) {
		if(Time.time > goa.endMoving) {
			return true;
		}
		float time = (Time.time - goa.startedMoving)/speed + .15f;
		obj.transform.position = Vector3.Lerp(goa.oldPosition, goa.newPosition, time);
		return false;
	}

	public static GameObject MakeConveyor(Grid grid, Vector2 startCoords, Vector2 direction, float length, float speed, bool switchable = false, float switchRate = 0) {
		if(direction.x != 0 && direction.y != 0) // no diagonal conveyors
			return null;
		GameObject conveyor = GameObject.CreatePrimitive(PrimitiveType.Plane);
		conveyor.name = "Conveyor";
		Conveyor script = conveyor.AddComponent<Conveyor>();
		script.startCoords = startCoords;
		script.direction = direction;
		script.speed = speed;
		if((startCoords + (direction*length)).x >= grid.width)
			length = grid.width - startCoords.x;
		if((startCoords + (direction*length)).x < 0)
			length = startCoords.x + 1;
		if((startCoords + (direction*length)).y >= grid.height)
			length = grid.height - startCoords.y;
		if((startCoords + (direction*length)).y < 0)
			length = startCoords.y + 1;
		script.length = length;
		script.switchable = switchable;
		script.switchRate = switchRate;
		script.cells = new Vector2[(int)length];
		script.planes = new List<GameObject>();
		int count = 0;
		while(count < length) {
			script.cells[count] = startCoords + (count * direction);
			GameObject conveyorPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
			script.planes.Add(conveyorPlane);
			conveyorPlane.transform.position = grid.grid[(int)script.cells[count].x, (int)script.cells[count].y].plane.transform.position + new Vector3(0f, 0f, -0.1f);
			conveyorPlane.transform.localScale = new Vector3(0f, 0f, 0.01f) + grid.grid[(int)script.cells[count].x, (int)script.cells[count].y].plane.transform.localScale;
			conveyorPlane.transform.Rotate(-90.0f, 0.0f, 0.0f);
			conveyorPlane.renderer.material.mainTexture = Resources.Load("Textures/Conveyor") as Texture;
	                conveyorPlane.renderer.material.shader = Shader.Find("Transparent/Diffuse");
        	        conveyorPlane.renderer.material.color = Color.white;
			conveyorPlane.name = "conveyor plane";
			if(direction == new Vector2(1, 0))
				conveyorPlane.transform.Rotate(0, 90f, 0);
			else if(direction == new Vector2(0, 1))
				conveyorPlane.transform.Rotate(0, 180f, 0);
			else if(direction == new Vector2(-1, 0))
				conveyorPlane.transform.Rotate(0, 270f, 0);
			count++;
		}
		script.endCoords = script.cells[(int)length-1];
		if(direction.x != 0)
			conveyor.transform.localScale = new Vector3(0.1f*length, 1.0f, 0.1f);
		else
			conveyor.transform.localScale = new Vector3(0.1f, 1.0f, 0.1f*length);
		conveyor.transform.position = new Vector3(-5000, -5000, -5000);
		conveyor.renderer.material.color = Color.white;
		script.wloc = conveyor.transform.position;
		conveyor.transform.Rotate(-90.0f, 0.0f, 0.0f);
		script.grid = grid;
		return conveyor;
	}

}
