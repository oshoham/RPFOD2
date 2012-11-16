using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour, IColor {
	public Grid grid;

	private Color _colorPainted;
	public Color colorPainted
	{
		get { return _colorPainted; }
		set {
			_colorPainted = value;
			renderer.material.color = value;
		}
	}
	public Vector2 gridCoords;
	
	void OnDisable() {
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}
	
	public static GameObject MakeWall(Grid grid, int x, int y, Color color = default(Color)) {
		if(color == default(Color)) {
			color = Color.white;
		}
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		wall.transform.position = new Vector3(x, y, -0.5f);
		wall.renderer.material.mainTexture = Resources.Load("Textures/Wall") as Texture;
		Wall script = wall.AddComponent<Wall>();
		script.colorPainted = Color.white;
		script.gridCoords = new Vector2(x, y);
		script.grid = grid;
		return wall;
	}
}
