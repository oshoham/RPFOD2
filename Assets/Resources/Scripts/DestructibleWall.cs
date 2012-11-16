using UnityEngine;

public class DestructibleWall : MonoBehaviour, IColor {
	public Grid grid;

	private Color _colorPainted;
	public Color colorPainted {
		get { return _colorPainted; }
		set {
			_colorPainted = value;
			renderer.material.color = value;
		}
	}
	public Vector2 gridCoords;
	public int health;

	public static GameObject MakeDestructibleWall(Grid grid, int x, int y, int health, Color color) {
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		wall.transform.position = new Vector3(x, y, -0.5f);
		wall.renderer.material.mainTexture = Resources.Load("Textures/DWall") as Texture;
		DestructibleWall script = wall.AddComponent<DestructibleWall>();
		script.colorPainted = Color.white;
		script.gridCoords = new Vector2(x, y);
		script.grid = grid;
		script.health = health;
		return wall;
	}
}