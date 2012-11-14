using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeWall : Wall {

	public List<Vector2> directions;

	void Update() {
		List<string> classList = new List<string> {"Robot", "Player"};
		foreach(Vector2 direction in directions) {
			Vector2 coord = gridCoords + direction;
			List<GameObject> objects = grid.GetObjectsOfTypes(coord, classList);
			foreach(GameObject obj in objects) {
				Destroy(obj);
			}
		}
	}

	public static GameObject MakeSpikeWall(Grid grid, int x, int y, List<Vector2> directions, Color color = default(Color)) {
		if(color == default(Color)) {
			color = Color.white;
		}
		GameObject spikeWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		spikeWall.name = "Spike Wall";
		spikeWall.transform.position = new Vector3(x, y, 0.0f);
		SpikeWall script = spikeWall.AddComponent<SpikeWall>();
		spikeWall.renderer.material.mainTexture = Resources.Load("Textures/Electrocute") as Texture;
		spikeWall.renderer.material.color = Color.white;
		script.colorPainted = color;
		script.gridCoords = new Vector2(x, y);
		script.directions = directions;
		script.grid = grid;
		foreach(Vector2 direction in directions) {
			grid.grid[(int)(direction.x + x), (int)(direction.y + y)].plane.renderer.material.color = Color.yellow;
		}
		return spikeWall;
	}
	
	/*
	 * Gets called when this is destroyed -- recolor all
	 * the floor tiles to show that the spikes are gone.
	 */
	void OnDisable() {
		foreach(Vector2 direction in directions) {
			grid.grid[(int)(direction.x + gridCoords.x), (int)(direction.y + gridCoords.y)].plane.renderer.material.color = Color.white;
		}
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}
}