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
		Square[,] see = grid.SCheckRad(3, gridCoords);
		for(int i = 0; i < see.GetLength(0); i++)
			for(int j = 0; j < see.GetLength(1); j++)
				if(see[i, j] != null) {
					see[i,j].colors[Color.green]++;
					see[i,j].SetColor();
				}
	}

	public static GameObject MakeExplosiveCrate(Grid grid, int x, int y, int health, int range) {
		GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
		crate.transform.position = new Vector3(x, y, 0.0f);
		ExplosiveCrate script = crate.AddComponent<ExplosiveCrate>();
		script.colorPainted = Color.red;
		script.gridCoords = new Vector2(x, y);
		script.grid = grid;
		script.health = health;
		script.range = range;
		return crate;
	}
}
