using System;
using UnityEditor;

namespace UniversalRenameTool.Editor {
	public static class SceneRenameTool {
		
		public static void Rename(UnityEngine.Object go, Func<string, string> converter) {
			if( go ) {
				var so = new SerializedObject(go);
				var nameProp = so.FindProperty("m_Name");
				nameProp.stringValue = converter(nameProp.stringValue);
				so.ApplyModifiedProperties();
			}
		}

		public static void Rename(UnityEngine.Object[] gos, Func<string, string> converter) {
			for( int i = 0; i < gos.Length; i++ ) {
				Rename(gos[i], converter);
			}
		}
	}

	public static class ProjectRenameTool {
		
		public static void Rename(UnityEngine.Object obj, Func<string, string> converter) {
			if( obj ) {
				var pathName = AssetDatabase.GetAssetPath(obj);
				AssetDatabase.RenameAsset(pathName, converter(obj.name));
				AssetDatabase.Refresh(ImportAssetOptions.Default);
			}
		}

		public static void Rename(UnityEngine.Object[] gos, Func<string, string> converter) {
			for( int i = 0; i < gos.Length; i++ ) {
				Rename(gos[i], converter);
			}
		}
	}
}
