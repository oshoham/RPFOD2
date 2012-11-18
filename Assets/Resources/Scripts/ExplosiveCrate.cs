using UnityEngine;
using System.Collections.Generic;

public class ExplosiveCrate : MonoBehaviour, IColor {

	public Grid grid;
	private Color _colorPainted;
	public Color colorPainted
	{
		get { return _colorPainted; }
		set {
			_colorPainted = value;
			gameObject.renderer.material.color = value;
		}
	}
	public Vector2 gridCoords;
	public int health;
	public int range;
	public int damageDealt;
	public List<Square> see;
	
	void Start() {
		see = grid.SCheckRad(3, gridCoords);
		foreach(Square sq in see) {
			if(sq != null) {
				sq.colors[Color.green]++;
				sq.SetColor();
			}
		}
	}
		
	void Update() {
		if(health <= 0) {
			foreach(Square sq in see) {
				foreach(GameObject obj in grid.GetObjectsOfTypes(sq.loc, new List<string> {"Robot",
								"Player",
								"DestructibleWall"})) {
					Destroy(obj);
				}
			}
			Destroy(gameObject);
		}
	}
	
	void OnDisable() {
		print(see);
		foreach(Square sq in see) {
			print(sq);
			sq.colors[Color.green]--;
			sq.SetColor();
		}
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}

	public static GameObject MakeExplosiveCrate(Grid grid, int x, int y, int health, int range, int damageDealt) {
		GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
		crate.name = "Explosive Crate";
		crate.transform.position = new Vector3(x, y, -0.5f);
		ExplosiveCrate script = crate.AddComponent<ExplosiveCrate>();
		crate.renderer.material.mainTexture = Resources.Load("Textures/explosive") as Texture;
		crate.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		script.colorPainted = Color.white;
		script.gridCoords = new Vector2(x, y);
		script.grid = grid;
		script.health = health;
		script.range = range;
		script.damageDealt = damageDealt;
		return crate;
	}
}
