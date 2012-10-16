using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static readonly int WIDTH = 400;
	public static readonly int HEIGHT = 400;

	Grid floor;

	void Start() {
		floor = new Grid(WIDTH, HEIGHT);
	}

	void Update() {

	}
}
