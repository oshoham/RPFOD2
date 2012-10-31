using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour, IColor {

	private Color _colorPainted;
	public Color colorPainted
	{
		get { return _colorPainted; }
		set {
			_colorPainted = value;
			renderer.material.color = value;
		}
	}
	public bool destructible;
	public int health;
	public Vector2 gridCoords;
	
	void Start() {

	}

	void Update() {
		if(health <= 0) {
			GameManager.floor.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
			Destroy(gameObject);
		}
	}

	public static GameObject MakeWall(int x, int y, int health, bool destructible,
					  Color color = default(Color)) {
		if(color == default(Color)) {
			color = Color.gray;
		}
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		wall.transform.position = new Vector3(x, y, 0.0f);
		Wall script = wall.AddComponent<Wall>();
		script.health = health;
		script.destructible = destructible;
		script.colorPainted = color;
		script.gridCoords = new Vector2(x, y);
		GameManager.floor.Add(wall, x, y);
		return wall;
	}
}
