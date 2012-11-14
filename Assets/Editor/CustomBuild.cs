using UnityEditor;
using UnityEngine;

public class CustomBuild : EditorWindow {
	
	static string path = "";
	
	[MenuItem ("File/Custom Build")]
	static void Init() {
		CustomBuild window = (CustomBuild)EditorWindow.GetWindow (typeof (CustomBuild));
	}
	
	public static void BuildGame() {
		if(path == "")
			// set path to be some default thing - maybe the desktop?
		if(Application.platform == RuntimePlatorm.OSXEditor)
			BuildPipeline.BuildPlayer(new string[] {"foo.txt"}, path + "RPFOD.app", BuildTarget.StandaloneOSXIntel, BuildOptions.None);
		else if(Application.platform == RuntimePlatform.WindowsEditor)
			BuildPipeline.BuildPlayer(new string[] {"foo.txt"}, path + "RPFOD.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
		else
			Debug.Log("What, are you using Linux or something? Get outta here.");
	}

	void OnGUI() {
		path = EditorGUILayout.TextField("Path", path);
	}
}
