using UnityEngine;
using UnityEditor;

namespace UnityRenameTool.Editor {
	public class RenameEditorWindow : EditorWindow {
		
		const float MaxWidth  = 300.0f;
		const float ToolsWidth = 200.0f;
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

		ObservedFindModeField _findModeObserver = null;
		ObservedFindModeField FindModeObserver {
			get {
				if( _findModeObserver == null ) {
					_findModeObserver = new ObservedFindModeField(OnFindModeChanged);
				}
				return _findModeObserver;
			}
		}

		ObservedToggle _ignoreCaseToggle = null;
		ObservedToggle IgnoreCaseToggle {
			get {
				if( _ignoreCaseToggle == null ) {
					_ignoreCaseToggle = new ObservedToggle(OnIgnoreCaseChanged, "Ignore case");
				}
				return _ignoreCaseToggle;
			}
		}

		ObservedObjectField _rootObserver = null;
		ObservedObjectField RootObserver {
			get {
				if( _rootObserver == null ) {
					_rootObserver = new ObservedObjectField(OnRootChanged, typeof(GameObject));
				}
				return _rootObserver;
			}
		}

		string      _replaceText = "";
		int        _replaceCount = -1;
		INameWorker _worker      = null;

		void OnGUI() {
			using ( new VerticalLayout(GUILayout.MaxWidth(MaxWidth)) ) {
				using ( new HorizontalLayout() ) {
					GUILayout.Label("Rename Tool");
				}
				using ( new HorizontalLayout() ) {
					GUILayout.Label("Root:", GUILayout.Width(TextWidth));
					RootObserver.Read();
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
					if( GUILayout.Button("Refresh") ) {
						Refresh();
					}
					if( GUILayout.Button("Rename") ) {
						RenameAllSelected();
					}
				}
				using ( new HorizontalLayout(GUILayout.Width(ToolsWidth)) ) {
					GUILayout.Label("Mode:", GUILayout.Width(TextWidth));
					FindModeObserver.Read();
					IgnoreCaseToggle.Read();
				}
				using ( new HorizontalLayout(GUILayout.Width(ToolsWidth)) ) {
					GUILayout.Label("Count:", GUILayout.Width(TextWidth));
					_replaceCount = EditorGUILayout.IntField(_replaceCount);
				}
			}
		}

		INameWorker CreateWorker(FindMode mode, string text, bool ignoreCase) {
			switch( mode ) {
				case FindMode.Simple: return new SimpleNameWorker(text, ignoreCase);
				case FindMode.RegExp: return new RegExNameWorker (text, ignoreCase);
				default: return null;
			}
		}

		void OnFindTextChanged(string text) {
			InitSearch();
		}

		void OnFindModeChanged(FindMode mode) {
			InitSearch();
		}

		void OnIgnoreCaseChanged(bool value) {
			InitSearch();
		}

		void OnRootChanged(UnityEngine.Object obj) {
			InitSearch();
		}

		void InitSearch() {
			var text = FindTextObserver.Value;
			var mode = FindModeObserver.Value;
			var ignoreCase = IgnoreCaseToggle.Value;
			var root = RootObserver.Value as GameObject;
			_worker = CreateWorker(mode, text, ignoreCase);
			if( _worker != null ) {
				var findTool = new FindTool(FindFunc);
				var filterResult = findTool.FilterObjects(root, text);
				Selection.objects = filterResult;
			}
		}

		void Refresh() {
			InitSearch();
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
				return _worker.Replace(name, _replaceText, _replaceCount);
			}
			return name;
		}
    }
}
