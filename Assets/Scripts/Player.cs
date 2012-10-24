using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public int health;
	public Vector2 gridCoords;
	public Color colorPainted;
	public Color colorShooting;
	
	void Start() {
		
	}

	void Update() {
		if(health <= 0) {
			Destroy(gameObject);
		}
		GetKeypresses();
	}

	void GetKeypresses() {
		if(Input.GetKeyDown("w")) {
			Move(new Vector2(0, 1));
		}
		if(Input.GetKeyDown("a")) {
			Move(new Vector2(-1, 0));
		}
		if(Input.GetKeyDown("s")) {
			Move(new Vector2(0, -1);
		}
		if(Input.GetKeyDown("d")) {
			Move(new Vector2(1, 0));
		}
	}
	
	/*
	 * Move by x and y in the game grid. The coordinates should probably be in the
	 * range (-1, 1).
	 */
	void Move(Vector2 coords) {
		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject)) {
			gridCoords += coords;
		}
	}

	public static GameObject MakePlayer(int x, int y, int health, Vector3 position) {
		GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
		player.transform.position = position;
		Player script = player.AddComponent<Player>();
		script.gridCoords = new Vector2(x, y);
		script.health = health;
	}
}
