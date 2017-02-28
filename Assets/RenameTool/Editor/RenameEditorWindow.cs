using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityRenameTool.Editor {
	public class RenameEditorWindow : EditorWindow {
		
		ObservedTextField _findTextObserver = null;
		ObservedTextField FindTextObserver {
			get {
				if( _findTextObserver == null ) {
					_findTextObserver = new ObservedTextField(OnFindTextChanged);
				}
				return _findTextObserver;
			}
		}

		void OnGUI() {
			using ( new VerticalLayout(GUILayout.MaxWidth(300)) ) {
				using ( new HorizontalLayout() ) {
					GUILayout.Label("Rename Tool");
				}
				using ( new HorizontalLayout() ) {
					GUILayout.Label("Find:");
					FindTextObserver.Read();
				}
			}
		}

		void OnFindTextChanged(string text) {
			var filteredObjs = new List<Object>();
			FindTool.FilterObjects(filteredObjs, null, text);
			Selection.objects = filteredObjs.ToArray();
		}
    }
}
