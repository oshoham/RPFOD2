using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour, IColor {

	public Vector2 gridCoords;
	public bool destroyable;
	private Color _colorPainted;
	public Color colorPainted
	{
		get { return _colorPainted; }
		set {
			renderer.material.color = value;
			_colorPainted = value;
		}
	}

	void Start() {

	}

	void Update() {

	}
}
