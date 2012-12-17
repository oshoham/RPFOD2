using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikeFloor : MonoBehaviour {
	public Grid grid;

	public Vector2 gridCoords;
	public GameObject sparks = Resources.Load("Standard Assets/Particles/Misc/Sparks") as GameObject;
	
	void Update () {
		List<string> classList = new List<string> {"Robot", "Player"};
		List<GameObject> objects = grid.GetObjectsOfTypes(gridCoords, classList);
		foreach(GameObject obj in objects) {
			Player p = obj.GetComponent<Player>();
			if(p != null) {
				Instantiate(sparks, transform.position + new Vector3(0,0,-5F), Quaternion.identity);
				p.health = 0;
			}
			Robot r =  obj.GetComponent<Robot>();
			if(r != null) {
				Instantiate(sparks, transform.position + new Vector3(0,0,-5F), Quaternion.identity);
				r.health = 0;
			}
		}
	}
	
	void OnDisable() {
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
		grid.grid[(int)gridCoords.x, (int)gridCoords.y].plane.renderer.material.color = Color.white;
	}
	
	public static GameObject MakeSpikeFloor(Grid grid, int x, int y) {
		GameObject spikeFloor = GameObject.CreatePrimitive(PrimitiveType.Cube);
		spikeFloor.renderer.material.mainTexture = Resources.Load("Textures/ElectrocuteIcon") as Texture;
		spikeFloor.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		spikeFloor.renderer.material.color = Color.white;
		spikeFloor.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		spikeFloor.transform.position = new Vector3(x, y, 0.0f);
		SpikeFloor script = spikeFloor.AddComponent<SpikeFloor>();
		script.gridCoords = new Vector2(x, y);
		script.grid = grid;
		return spikeFloor;
	}
}
