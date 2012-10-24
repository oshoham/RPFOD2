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
}
