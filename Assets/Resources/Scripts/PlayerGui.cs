using UnityEngine;
using System.Collections;

public class PlayerGui : MonoBehaviour {
	
	Color color;
	Vector3 position;
	bool forShooting;
	public static Shader transspec = Shader.Find("Transparent/Parallax Diffuse");
	public static Shader normal = Shader.Find("Diffuse");
	public static Shader transdiff = Shader.Find("Transparent/Diffuse");
	public Quaternion oldRotation;
	
	void Start () {
		oldRotation = transform.rotation;
	}

	void Update () {
		transform.position = Camera.main.ScreenToWorldPoint(position);
		if(GameManager.player == null)
			return;
		if((GameManager.player.colors.ContainsKey(color) &&
		    GameManager.player.colors[color] > 0) || color == GameManager.player.defaultColor) {
			renderer.material.color = color;
		}
		else {
			renderer.material.color = Color.gray;
			renderer.material.shader = transdiff;
		}
		if(forShooting) {
			if(GameManager.player.colorShooting == color &&
			   GameManager.player.colors[color] > 0) {
				transform.localScale = new Vector3(0.07f, 1.0f, 0.07f);
				renderer.material.shader = normal;
				transform.Rotate(Vector3.up * Time.deltaTime * 70);
			}
			else {
				transform.localScale = new Vector3(0.03f, 1.0f, 0.03f);
				renderer.material.shader = transspec;
				transform.rotation = oldRotation;
			}
		}
		else if (!forShooting && color != GameManager.player.defaultColor) {
			if(GameManager.player.colorPainted == color) {
				transform.localScale = new Vector3(0.07f, 1.0f, 0.07f);
				renderer.material.color = color;
				renderer.material.shader = normal;
			}
			else {
				transform.localScale = new Vector3(0.03f, 1.0f, 0.03f);
				renderer.material.shader = transspec;
			}
		}
		else {
			renderer.material.color = color;
			if(GameManager.player.colorPainted == color) {
				transform.localScale = new Vector3(0.07f, 1.0f, 0.07f);
				renderer.material.shader = normal;
			}
			else {
				transform.localScale = new Vector3(0.03f, 1.0f, 0.03f);
				renderer.material.shader = transspec;
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
	
	//weirdly not working, maybe because planes don't have colliders/aren't gui objects
	void OnMouseOver() {
		transform.localScale = new Vector3(0.05f, 1.0f, 0.05f);
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
