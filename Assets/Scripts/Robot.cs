using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {

	public int health;
	public float speed;
	public Vector2 gridCoords;
	public int damageDealt;
	public Vector2 movementDirection;
	public Vector2 fireDirection;
	public Color colorPainted;
	public Color colorVisible;

	public float lastMoved;
	
	void Start() {
		
	}

	void Update() {
		if(health <= 0) {
			Destroy(gameObject);
		}
		if(Time.time > lastMoved + speed) {
			Move(movementDirection);
		}
	}

	void Move(Vector2 coords) {
		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject)) {
			gridCoords += coords;
		}
	}

	public static GameObject MakeRobot(int x, int y, float speed, int damage, int health,
				    Vector2 movementDirection, Vector2 fireDirection,
				    Color colorVisible) {
		GameObject robot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		robot.transform.position = new Vector3(x, y, 0.0f);
		Robot script = robot.AddComponent<Robot>();
		script.gridCoords = new Vector2(x, y);
		script.speed = speed;
		script.damageDealt = damage;
		script.movementDirection = movementDirection;
		script.fireDirection = fireDirection;
		script.colorVisible = colorVisible;
		GameManager.floor.Add(robot, x, y);
		return robot;
	}
}
