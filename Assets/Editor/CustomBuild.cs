using UnityEditor;
using UnityEngine;

public class CustomBuild : EditorWindow {
	
	string path = "";
	
	[MenuItem ("File/Custom Build")]
	static void Init() {
		CustomBuild window = (CustomBuild)EditorWindow.GetWindow (typeof (CustomBuild));
	}
	
	public static void BuildGame() {
		BuildPipeline.BuildPlayer(new string[] {"foo.txt"}, "whatever", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
	}

	void OnGUI() {
		path = EditorGUILayout.TextField("Path", path);
	}
}