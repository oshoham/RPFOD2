using UnityEngine;

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
	
	void Update() {
		Square[,] see = grid.SCheckRad(1, gridCoords);
	}

	public static GameObject MakeExplosiveCrate(Grid grid, int x, int y, int health, int range) {
		GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
		crate.transform.position = new Vector3(x, y, 0.0f);
		ExplosiveCrate script = crate.AddComponent<ExplosiveCrate>();
		crate.renderer.material.mainTexture = Resources.Load("Textures/explosive") as Texture;
		crate.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		script.colorPainted = Color.white;
		script.gridCoords = new Vector2(x, y);
		script.grid = grid;
		script.health = health;
		script.range = range;
		return crate;
	}
}
