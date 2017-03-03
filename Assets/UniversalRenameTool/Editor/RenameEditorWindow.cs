using UnityEngine;
using UnityEditor;

namespace UniversalRenameTool.Editor {
	public class RenameEditorWindow : EditorWindow {
		
		const float MaxWidth  = 300.0f;
		const float ToolsWidth = 200.0f;
		const float TextWidth = 50.0f;

		const string RootTooltip    = "Find in gameObject and its childs";
		const string FindTooltip    = "Part of object name to find";
		const string ReplaceTooltip = "What replaces 'Find' part";
		const string RefreshTooltip = "Update results manually";
		const string RenameTooltip  = "Rename all selected objects";
		const string CountTooltip   = "How many matches would be replaced ('-1' for all)";

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

		ObservedLocationModeField _locModeObserver = null;
		ObservedLocationModeField LocModeObserver {
			get {
				if( _locModeObserver == null ) {
					_locModeObserver = new ObservedLocationModeField(OnLocationModeChanged);
				}
				return _locModeObserver;
			}
		}

		string      _replaceText = "";
		int        _replaceCount = -1;
		INameWorker _worker      = null;

		GUIStyle _headerStyle = null;

		GUIContent _rootLabelContent     = new GUIContent("Root:",    RootTooltip);
		GUIContent _findLabelContent     = new GUIContent("Find:",    FindTooltip);
		GUIContent _replaceLabelContent  = new GUIContent("Replace:", ReplaceTooltip);
		GUIContent _refreshButtonContent = new GUIContent("Refresh",  RefreshTooltip);
		GUIContent _renameButtonContent  = new GUIContent("Rename",   RenameTooltip);
		GUIContent _countLabelContent    = new GUIContent("Count:",   CountTooltip);

		void OnGUI() {
			if( _headerStyle == null ) {
				_headerStyle =  new GUIStyle(GUI.skin.label);
				_headerStyle.fontSize = 12;
				_headerStyle.fontStyle = FontStyle.Bold;
			}
			
			using ( new VerticalLayout(GUILayout.MaxWidth(MaxWidth)) ) {
				GUILayout.Space(5);
				using ( new HorizontalLayout() ) {
					GUILayout.Label("Rename Tool", _headerStyle);
				}
				using ( new HorizontalLayout(GUILayout.Width(ToolsWidth)) ) {
					GUILayout.Label("Find In:", GUILayout.Width(TextWidth));
					LocModeObserver.Read();
				}
				if( LocModeObserver.Value == LocationMode.Scene ) {
					using ( new HorizontalLayout() ) {
						GUILayout.Label(_rootLabelContent, GUILayout.Width(TextWidth));
						RootObserver.Read();
					}
				}
				using ( new HorizontalLayout() ) {
					GUILayout.Label(_findLabelContent, GUILayout.Width(TextWidth));
					FindTextObserver.Read();
				}
				using ( new HorizontalLayout() ) {
					GUILayout.Label(_replaceLabelContent, GUILayout.Width(TextWidth));
					_replaceText = GUILayout.TextField(_replaceText);
				}
				using ( new HorizontalLayout() ) {
					if( GUILayout.Button(_refreshButtonContent) ) {
						Refresh();
					}
					if( GUILayout.Button(_renameButtonContent) ) {
						RenameAllSelected();
					}
				}
				if( LocModeObserver.Value == LocationMode.Scene ) {
					using ( new HorizontalLayout(GUILayout.Width(ToolsWidth)) ) {
						GUILayout.Label("Mode:", GUILayout.Width(TextWidth));
						FindModeObserver.Read();
						IgnoreCaseToggle.Read();
					}
				}
				using ( new HorizontalLayout(GUILayout.Width(ToolsWidth)) ) {
					GUILayout.Label(_countLabelContent, GUILayout.Width(TextWidth));
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

		void OnLocationModeChanged(LocationMode mode) {
			InitSearch();
		}

		void InitSearch() {
			var text = FindTextObserver.Value;
			var ignoreCase = IgnoreCaseToggle.Value;
			var root = RootObserver.Value as GameObject;
			var locMode = LocModeObserver.Value;
			var findMode = (locMode == LocationMode.Scene) 
				? FindModeObserver.Value
				: FindMode.Simple;
			_worker = CreateWorker(findMode, text, ignoreCase);
			if( _worker != null ) {
				var filterResult = FilterObjects(locMode, root, text);
				Selection.objects = filterResult;
			}
		}

		Object[] FilterObjects(LocationMode locMode, GameObject root, string text) {
			switch( locMode ) {
				case LocationMode.Scene: {
					var sceneFindTool = new SceneFindTool(FindFunc);
					return sceneFindTool.FilterObjects(root, text);
				}

				case LocationMode.Project: {
					var projFindTool = new ProjectFindTool(FindFunc);
					return projFindTool.FilterObjects(text);
				}

				default: return null;
			}
		}

		void Refresh() {
			InitSearch();
		}

		void RenameAllSelected() {
			var locMode = LocModeObserver.Value;
			switch( locMode ) {
				case LocationMode.Scene: {
					SceneRenameTool.Rename(Selection.objects, ReplaceFunc);
				}
				break;

				case LocationMode.Project: {
					ProjectRenameTool.Rename(Selection.objects, ReplaceFunc);
				}
				break;
			}
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
