using System;
using UnityEngine;
using UnityEditor;

namespace UnityRenameTool.Editor {
	public static class RenameTool {
		
		public static void Rename(GameObject go, Func<string, string> converter) {
			if( go ) {
				var so = new SerializedObject(go);
				var nameProp = so.FindProperty("m_Name");
				nameProp.stringValue = converter(nameProp.stringValue);
				so.ApplyModifiedProperties();
			}
		}

		public static void Rename(GameObject[] gos, Func<string, string> converter) {
			for( int i = 0; i < gos.Length; i++ ) {
				Rename(gos[i], converter);
			}
		}
	}
}
