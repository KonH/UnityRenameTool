using UnityEditor;

namespace UnityRenameTool {
	public static class RenameMenuItems {
		
		[MenuItem("Window/Rename Tool")]
		public static void OpenWindow() {
			EditorWindow.GetWindow(typeof(RenameEditorWindow), false, "Rename Tool");
		}
	}
}
