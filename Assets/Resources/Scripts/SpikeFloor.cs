using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeFloor : MonoBehaviour {

	public Vector2 gridCoords;
	
	void Update () {
		List<string> classList = new List<string> {"Robot", "Player"};
		List<GameObject> objects = GameManager.floor.GetObjectsOfTypes(gridCoords, classList);
		foreach(GameObject obj in objects) {
			Destroy(obj);
		}
	}
	
	void OnDisable() {
		GameManager.floor.grid[(int)gridCoords.x, (int)gridCoords.y].plane.renderer.material.color = Color.black;
	}
	
	public static GameObject MakeSpikeFloor(int x, int y) {
		GameObject spikeFloor = GameObject.CreatePrimitive(PrimitiveType.Cube);
		spikeFloor.renderer.material.mainTexture = Resources.Load("Textures/Electrocute") as Texture;
		spikeFloor.renderer.material.color = Color.white;
		spikeFloor.transform.localScale = new Vector3(0, 0, 0);
		SpikeFloor script = spikeFloor.AddComponent<SpikeFloor>();
		script.gridCoords = new Vector2(x, y);
		GameManager.floor.grid[x, y].plane.renderer.material.color = Color.yellow;
		return spikeFloor;
	}
}
