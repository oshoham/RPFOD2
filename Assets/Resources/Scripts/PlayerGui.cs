using UnityEngine;
using System.Collections;

public class PlayerGui : MonoBehaviour {
	
	Color color;
	Vector3 position;
	bool forShooting;

	void Update () {
		transform.position = Camera.main.ScreenToWorldPoint(position);
		if(GameManager.player == null)
			return;
		if((GameManager.player.colors.ContainsKey(color) &&
		    GameManager.player.colors[color] > 0) || color == GameManager.player.defaultColor) {
			renderer.material.color = color;
		}
		else {
			renderer.material.color = Color.black;
		}
		if(forShooting) {
			if(GameManager.player.colorShooting == color &&
			   GameManager.player.colors[color] > 0) {
				transform.localScale = new Vector3(0.07f, 1.0f, 0.07f);
			}
			else {
				transform.localScale = new Vector3(0.04f, 1.0f, 0.04f);
			}
		}
		else if (!forShooting && color != GameManager.player.defaultColor) {
			if(GameManager.player.colorPainted == color) {
				transform.localScale = new Vector3(0.06f, 1.0f, 0.06f);
				renderer.material.color = color;
			}
			else {
				transform.localScale = new Vector3(0.04f, 1.0f, 0.04f);
			}
		}
		else {
			renderer.material.color = color;
			if(GameManager.player.colorPainted == color) {
				transform.localScale = new Vector3(0.06f, 1.0f, 0.06f);
			}
			else {
				transform.localScale = new Vector3(0.04f, 1.0f, 0.04f);
			}
		}
	}
	
	void OnMouseDown() {
		if(GameManager.player == null)
			return;
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
		else {
			GameManager.player.colorPainted = color;
		}
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
		gui.renderer.material.mainTexture = Resources.Load("Textures/Tile2") as Texture;
		PlayerGui script = gui.AddComponent<PlayerGui>();
		script.color = color;
		script.position = position;
		script.forShooting = forShooting;
		return gui;
	}
}
