using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniversalRenameTool {
	public class SceneFindTool {

		System.Func<string, bool> _isWantedName = null;

		public SceneFindTool(System.Func<string, bool> isWantedName) {
			_isWantedName = isWantedName;
		}

		public Object[] FilterObjects(GameObject root, string text) {
			var container = new List<Object>();
			FilterObjects(container, root, text);
			return container.ToArray();
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
			return _isWantedName(go.name);
		}
	}
}
