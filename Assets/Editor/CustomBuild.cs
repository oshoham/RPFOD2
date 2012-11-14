using UnityEditor;
using UnityEngine;

public class CustomBuild : EditorWindow {
	
	public static string path = "";
	
	[MenuItem ("File/Custom Build")]
	static void Init() {
		EditorWindow.GetWindow(typeof (CustomBuild));
	}
	
	public static void BuildGame() {
		if(path == "")
			// set path to be some default thing - maybe the desktop?
		if(Application.platform == RuntimePlatform.OSXEditor)
			BuildPipeline.BuildPlayer(new string[] {"StartScreen", "CavalcadeMap", "Editor", "FreePlaySelector", "Game"}, path + "RPFOD.app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
		else if(Application.platform == RuntimePlatform.WindowsEditor)
			BuildPipeline.BuildPlayer(new string[] {"StartScreen", "CavalcadeMap", "Editor", "FreePlaySelector", "Game"}, path + "RPFOD.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
		else
			Debug.Log("Shit, bro, are you using Linux or something? Get outta here.");
	}

	void OnGUI() {
		path = EditorGUILayout.TextField("Path", path);
		if(GUILayout.Button("Save!")) {
			BuildGame();
		}
	}
}
