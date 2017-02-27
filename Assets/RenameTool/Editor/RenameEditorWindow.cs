using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace UnityRenameTool {
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
			FilterObjects(filteredObjs, null, text);
			Selection.objects = filteredObjs.ToArray();
		}

		void FilterObjects(List<Object> container, GameObject gameObject, string text) {
			if ( gameObject ) {
				if( IsWantedGameObject(gameObject, text) ) {
					container.Add(gameObject);
				}
				FilterChilds(container, gameObject, text);				
			} else {
				InitialFilter(container, text);
			} 
		}

		void FilterChilds(List<Object> container, GameObject gameObject, string text) {
			var trans = gameObject.transform;
			for( int i = 0; i < trans.childCount; i++ ) {
				FilterObjects(container, trans.GetChild(i).gameObject, text);
			}
		}

		void InitialFilter(List<Object> container, string text) {
			var scene = SceneManager.GetActiveScene();
			var gos = scene.GetRootGameObjects();
			for( int i = 0; i < gos.Length; i++ ) {
				FilterObjects(container, gos[i], text);
			}
		}

		bool IsWantedGameObject(GameObject go, string text) {
			return go.name.Contains(text);
		}
    }
}
