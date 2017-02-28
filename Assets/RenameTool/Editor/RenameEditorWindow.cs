using UnityEngine;
using UnityEditor;

namespace UnityRenameTool.Editor {
	public class RenameEditorWindow : EditorWindow {
		
		const float MaxWidth  = 300.0f;
		const float TextWidth = 50.0f;

		ObservedTextField _findTextObserver = null;
		ObservedTextField FindTextObserver {
			get {
				if( _findTextObserver == null ) {
					_findTextObserver = new ObservedTextField(OnFindTextChanged);
				}
				return _findTextObserver;
			}
		}

		string _replaceText = "";

		INameWorker _worker = null;

		void OnGUI() {
			using ( new VerticalLayout(GUILayout.MaxWidth(MaxWidth)) ) {
				using ( new HorizontalLayout() ) {
					GUILayout.Label("Rename Tool");
				}
				using ( new HorizontalLayout() ) {
					GUILayout.Label("Find:", GUILayout.Width(TextWidth));
					FindTextObserver.Read();
				}
				using ( new HorizontalLayout() ) {
					GUILayout.Label("Replace:", GUILayout.Width(TextWidth));
					_replaceText = GUILayout.TextField(_replaceText);
				}
				using ( new HorizontalLayout() ) {
					if ( GUILayout.Button("Rename") ) {
						RenameFirstSelected();
					}
					if( GUILayout.Button("Rename All") ) {
						RenameAllSelected();
					}
				}
			}
		}

		void OnFindTextChanged(string text) {
			_worker = new SimpleNameWorker(text);
			var findTool = new FindTool(FindFunc);
			var filterResult = findTool.FilterObjects(text);
			Selection.objects = filterResult;
		}

		void RenameFirstSelected() {
			var selection = Selection.gameObjects;
			var firstSelected = 
				selection.Length > 0 
				? selection[selection.Length - 1] 
				: null;
			RenameTool.Rename(firstSelected, ReplaceFunc);
		}

		void RenameAllSelected() {
			RenameTool.Rename(Selection.gameObjects, ReplaceFunc);
		}

		bool FindFunc(string name) {
			return _worker.IsWantedName(name);
		}

		string ReplaceFunc(string name) {
			return _worker.Replace(name, _replaceText);
		}
    }
}
