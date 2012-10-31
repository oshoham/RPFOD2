using UnityEngine;
using System.Collections;

public class PlayerGui : MonoBehaviour {
	
	Color color;
	Vector3 position;
	Light indicatorLight;

	void Update () {
		transform.position = Camera.main.ScreenToWorldPoint(position);
		if(GameManager.player.colors.Contains(color)) {
			renderer.material.color = color;
		}
		else {
			renderer.material.color = Color.black;
		}
		if(GameManager.player.colorShooting == color) {
			light.range = 10;
		}
		else {
			light.range = 0;
		}
	}
	
	void OnMouseDown() {
		if(GameManager.player.colors.Contains(color)) {
			GameManager.player.colorShooting = color;
		}
	}
	
	/*
	 * Creates a PlayerGui with the given color at position in screen
	 * coordinates.
	 */
	public static GameObject MakePlayerGui(Color color, Vector3 position) {
		GameObject gui = GameObject.CreatePrimitive(PrimitiveType.Plane);
		gui.name = "GUI plane";
		gui.transform.Rotate(-90.0f, 0.0f, 0.0f);
		gui.transform.localScale = new Vector3(.02f, 1.0f, .02f);
		PlayerGui script = gui.AddComponent<PlayerGui>();
		script.color = color;
		script.position = position;
		script.indicatorLight = gui.AddComponent<Light>();
		script.indicatorLight.type = LightType.Spot;
		script.indicatorLight.spotAngle = 10.0f;
		script.indicatorLight.transform.Translate(0.0f, 0.0f, -1.0f);
		// fuck!
		//script.light.transform.localEulerAngles = new Vector3(180.0f, 0.0f, 0.0f);
		return gui;
	}
}
