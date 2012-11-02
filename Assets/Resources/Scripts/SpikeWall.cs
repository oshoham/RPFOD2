using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeWall : Wall {

	public List<Vector2> directions;

	void Update() {
		CheckHealth();
		List<string> classList = new List<string> {"Robot", "Player"};
		foreach(Vector2 direction in directions) {
			Vector2 coord = gridCoords + direction;
			List<GameObject> objects = GameManager.floor.GetObjectsOfTypes(coord, classList);
			foreach(GameObject obj in objects) {
				Destroy(obj);
			}
		}
	}

	public static GameObject MakeSpikeWall(int x, int y, int health, bool destructible,
					       List<Vector2> directions, Color color = default(Color)) {
		if(color == default(Color)) {
			color = Color.white;
		}
		GameObject spikeWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		spikeWall.transform.position = new Vector3(x, y, 0.0f);
		SpikeWall script = spikeWall.AddComponent<SpikeWall>();
		spikeWall.renderer.material.mainTexture = Resources.Load("Textures/Spike") as Texture;
		spikeWall.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		spikeWall.renderer.material.color = Color.white;
		script.health = health;
		script.destructible = destructible;
		script.colorPainted = color;
		script.gridCoords = new Vector2(x, y);
		script.directions = directions;
		foreach(Vector2 direction in directions) {
			GameManager.floor.grid[(int)(direction.x + x), (int)(direction.y + y)].plane.renderer.material.color = Color.yellow;
		}
		GameManager.floor.Add(spikeWall, x, y);
		return spikeWall;
	}
	
	/*
	 * Gets called when this is destroyed -- recolor all
	 * the floor tiles to show that the spikes are gone.
	 */
	void OnDisable() {
		foreach(Vector2 direction in directions) {
			GameManager.floor.grid[(int)(direction.x + gridCoords.x), (int)(direction.y + gridCoords.y)].plane.renderer.material.color = Color.black;
		}
		GameManager.floor.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}
}