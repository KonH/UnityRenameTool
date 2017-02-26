using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace UnityRenameTool {
	public class RenameEditorWindow : EditorWindow {
		
		string _findText = "";

		void OnGUI() {
			GUILayout.BeginVertical(GUILayout.MaxWidth(300));
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("Find:");
					_findText = GUILayout.TextField(_findText);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
			Test();
		}

		void Test() {
			var scene = SceneManager.GetActiveScene();
			var gos = scene.GetRootGameObjects();
			foreach( var go in gos ) {
				Debug.Log(go.name);
			}
		}
	}
}
