using UnityEngine;
using System.Collections;

public class PlayerGui : MonoBehaviour {
	
	Color color;
	Vector3 position;
	bool forShooting;

	void Update () {
		transform.position = Camera.main.ScreenToWorldPoint(position);
		if(GameManager.player.colors.ContainsKey(color) &&
		   GameManager.player.colors[color] > 0) {
			renderer.material.color = color;
		}
		else {
			renderer.material.color = Color.black;
		}
		if(forShooting) {
			if(GameManager.player.colorShooting == color &&
			   GameManager.player.colors[color] > 0) {
				transform.localScale = new Vector3(0.03f, 1.0f, 0.03f);
			}
			else {
				transform.localScale = new Vector3(0.02f, 1.0f, 0.02f);
			}
		}
		else if (!forShooting && color != GameManager.player.defaultColor) {
			if(GameManager.player.colorPainted == color && GameManager.player.colors[color] > 0) {
				transform.localScale = new Vector3(0.03f, 1.0f, 0.03f);
			}
			else {
				transform.localScale = new Vector3(0.02f, 1.0f, 0.03f);
			}
		}
		else {
			if(GameManager.player.colorPainted == color) {
				transform.localScale = new Vector3(0.03f, 1.0f, 0.03f);
			}
			else {
				transform.localScale = new Vector3(0.02f, 1.0f, 0.03f);
			}
		}
	}
	
	void OnMouseDown() {
		if(color != GameManager.player.defaultColor) {
			if(GameManager.player.colors.ContainsKey(color) &&
			   GameManager.player.colors[color] > 0) {
				if(forShooting) {
					GameManager.player.colorShooting = color;
				}
				else {
					GameManager.player.ReassignColor(color);
				}
			}
		}
		else
			GameManager.player.colorPainted = color;
	}
	
	/*
	 * Creates a PlayerGui with the given color at position in screen
	 * coordinates.
	 */
	public static GameObject MakePlayerGui(Color color, Vector3 position, bool forShooting) {
		GameObject gui = GameObject.CreatePrimitive(PrimitiveType.Plane);
		gui.name = "GUI plane";
		gui.transform.Rotate(-90.0f, 0.0f, 0.0f);
		gui.transform.localScale = new Vector3(.02f, 1.0f, .02f);
		PlayerGui script = gui.AddComponent<PlayerGui>();
		script.color = color;
		script.position = position;
		script.forShooting = forShooting;
		return gui;
	}
}
