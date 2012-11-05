using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

public static class LevelWriter {
	
	public static void WriteLevel() {

	}

	public static void CreateFile(string filename) {
		string path = "";
		FolderBrowserDialog browser = new FolderBrowserDialog();
		if(broswer.ShowDialog() == DialogResult.OK)
			path = browser.SelectedPath;
		path = Path.Combine(path, filename);


	}

}
