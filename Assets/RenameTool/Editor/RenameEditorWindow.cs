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

		FindMode    _mode        = FindMode.Simple;
		string      _replaceText = "";
		INameWorker _worker      = null;

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
				using ( new HorizontalLayout() ) {
					GUILayout.Label("Find Mode:");
					_mode = (FindMode)EditorGUILayout.EnumPopup(_mode);
				}
			}
		}

		INameWorker CreateWorker(string text) {
			switch( _mode ) {
				case FindMode.Simple: return new SimpleNameWorker(text);
				case FindMode.RegExp: return new RegExNameWorker (text);
				default: return null;
			}
		}

		void OnFindTextChanged(string text) {
			_worker = CreateWorker(text);
			if( _worker != null ) {
				var findTool = new FindTool(FindFunc);
				var filterResult = findTool.FilterObjects(text);
				Selection.objects = filterResult;
			}
		}

		void RenameFirstSelected() {
			var selection = Selection.gameObjects;
			var firstSelected = 
				selection.Length > 0 
				? selection[0] 
				: null;
			if( firstSelected ) {
				RenameTool.Rename(firstSelected, ReplaceFunc);
				Selection.objects = ReduceSelection(Selection.objects);
			}
		}

		Object[] ReduceSelection(Object[] objects) {
			var newObjects = new Object[objects.Length - 1];
			for( int i = 1; i < objects.Length; i++ ) {
				newObjects[i - 1] = objects[i];
			}
			return newObjects;
		}

		void RenameAllSelected() {
			RenameTool.Rename(Selection.gameObjects, ReplaceFunc);
		}

		bool FindFunc(string name) {
			if( _worker != null ) {
				return _worker.IsWantedName(name);
			}
			return false;
		}

		string ReplaceFunc(string name) {
			if( _worker != null ) {
				return _worker.Replace(name, _replaceText);
			}
			return name;
		}
    }
}
