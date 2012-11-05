using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

public static class LevelWriter {
	
	public static void WriteLevel(string filename) {
		string path = "";
		FolderBrowserDialog browser = new FolderBrowserDialog();
		if(broswer.ShowDialog() == DialogResult.OK)
			path = browser.SelectedPath;
		path = Path.Combine(path, filename);

		if(!File.Exists(path)) { //take this line out if we want to overwrite files
			using (StreamWriter writer = File.CreateText(path)) {
				for(int i = 0; i < GameManager.floor.width; i++) {
					for(int j = 0; j < GameManager.floor.height; j++) {
						StringBuilder sb = new StringBuilder();
						Square sq = GameManager.floor.grid[i, j];
						Wall wall = sq.objects.Find(GameObject g => g.GetComponent<Wall>() != null)
						if(wall != null) {
							sb.Append(0 + " " + wall.health + " ");
							if(wall.destructible == false)
								sb.Append(0 + " ");
							else
								sb.Append(1 + " ");

							if(wall.colorPainted == Color.red)
								sb.Append(0 + " ");
							else if(wall.colorPainted == Color.green)
								sb.Append(1 + " ");
							else if(wall.colorPainted == Color.blue)
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
						}

					}
				}
			}
		}
		else
			Debug.Log("Dude, what are you trying to do here? Filename: " + filename + " already exists. Just stop already.");
	}

}
