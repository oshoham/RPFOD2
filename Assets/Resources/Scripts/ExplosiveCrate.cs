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
	
	public static GameObject MakeExplosiveCrate(Grid grid, int x, int y) {
		return null;
	}
}