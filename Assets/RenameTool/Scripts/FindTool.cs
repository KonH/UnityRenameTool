﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityRenameTool {
	public static class FindTool {
		public static void FilterObjects(List<Object> container, GameObject gameObject, string text) {
			if ( gameObject ) {
				if( IsWantedGameObject(gameObject, text) ) {
					container.Add(gameObject);
				}
				FilterChilds(container, gameObject, text);				
			} else {
				InitialFilter(container, text);
			} 
		}

		static void FilterChilds(List<Object> container, GameObject gameObject, string text) {
			var trans = gameObject.transform;
			for( int i = 0; i < trans.childCount; i++ ) {
				FilterObjects(container, trans.GetChild(i).gameObject, text);
			}
		}

		static void InitialFilter(List<Object> container, string text) {
			var scene = SceneManager.GetActiveScene();
			var gos = scene.GetRootGameObjects();
			for( int i = 0; i < gos.Length; i++ ) {
				FilterObjects(container, gos[i], text);
			}
		}

		static bool IsWantedGameObject(GameObject go, string text) {
			return go.name.Contains(text);
		}
	}
}
