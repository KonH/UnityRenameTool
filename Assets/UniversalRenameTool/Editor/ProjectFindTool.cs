using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UniversalRenameTool.Editor {
	public class ProjectFindTool {

		System.Func<string, bool> _isWantedName = null;

		public ProjectFindTool(System.Func<string, bool> isWantedName) {
			_isWantedName = isWantedName;
		}

		public UnityEngine.Object[] FilterObjects(string text) {
			var container = new List<UnityEngine.Object>();
			FilterObjectsEasy(container, text);
			return container.ToArray();
		}

		void FilterObjectsEasy(List<UnityEngine.Object> container, string text) {
			var assets = AssetDatabase.FindAssets(text);
			SetProjectFilter(text);
			foreach( var assetGuid in assets ) {
				var path = AssetDatabase.GUIDToAssetPath(assetGuid);
				var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
				container.Add(obj);
			}
		}

		void SetProjectFilter(string text) {
			EditorWindow[] windows = (EditorWindow[])Resources.FindObjectsOfTypeAll (typeof(EditorWindow));
			foreach( var window in windows ) {
				var type = window.GetType();
				if ( type.ToString() == "UnityEditor.ProjectBrowser") {
					var setSearch = type.GetMethod("SetSearch", new[] {typeof(string)} );         
    				object[] parameters = new object[]{text};
					try {
    					setSearch.Invoke(window, parameters);
					}
					catch {}
				}
			}
		}

		void FilterObjectsFull(List<UnityEngine.Object> container, string text) {
			var allAssets = AssetDatabase.FindAssets("");
			foreach( var assetGuid in allAssets ) {
				var path = AssetDatabase.GUIDToAssetPath(assetGuid);
				var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
				if( IsWantedObject(obj, text) ) {
					container.Add(obj);
				}
			}
		}

		bool IsWantedObject(UnityEngine.Object obj, string text) {
			return _isWantedName(obj.name);
		}
	}
}
