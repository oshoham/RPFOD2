using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	public Grid grid;

	public Vector2 gridCoords;
	
	void OnDisable() {
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}
	
	public static GameObject MakeWall(Grid grid, int x, int y) {
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		wall.transform.position = new Vector3(x, y, -0.5f);
		wall.renderer.material.mainTexture = Resources.Load("Textures/Wall") as Texture;
		Wall script = wall.AddComponent<Wall>();
		script.gridCoords = new Vector2(x, y);
		script.grid = grid;
		return wall;
	}
}
