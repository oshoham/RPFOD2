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
		CheckHealth();
	}

	public void CheckHealth() {
		if(health <= 0) {
			Destroy(gameObject);
		}
	}
	
	void OnDisable() {
		GameManager.floor.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}
	
	public static GameObject MakeWall(int x, int y, int health, bool destructible,
					  Color color = default(Color)) {
		if(color == default(Color)) {
			color = Color.white;
		}
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		wall.transform.position = new Vector3(x, y, 0.0f);
		wall.renderer.material.mainTexture = Resources.Load("Textures/Wall") as Texture;
		Wall script = wall.AddComponent<Wall>();
		script.health = health;
		script.destructible = destructible;
		script.colorPainted = Color.white;
		script.gridCoords = new Vector2(x, y);
		GameManager.floor.Add(wall, x, y);
		return wall;
	}
}
