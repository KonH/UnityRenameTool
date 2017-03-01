using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityRenameTool.Editor {
	public class ProjectFindTool {

		System.Func<string, bool> _isWantedName = null;

		public ProjectFindTool(System.Func<string, bool> isWantedName) {
			_isWantedName = isWantedName;
		}

		public Object[] FilterObjects(string text) {
			var container = new List<Object>();
			FilterObjects(container, text);
			return container.ToArray();
		}

		void FilterObjects(List<Object> container, string text) {
			var allAssets = AssetDatabase.FindAssets("");
			foreach( var assetGuid in allAssets ) {
				var path = AssetDatabase.GUIDToAssetPath(assetGuid);
				var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
				if( IsWantedObject(obj, text) ) {
					container.Add(obj);
				}
			}
		}

		bool IsWantedObject(Object obj, string text) {
			return _isWantedName(obj.name);
		}
	}
}
