using System;
using UnityEngine;

namespace UnityRenameTool {
	public class VerticalLayout : IDisposable {
		public VerticalLayout(params GUILayoutOption[] options) {
			GUILayout.BeginVertical(options);
		}

		public void Dispose() {
			GUILayout.EndVertical();
		}
	}

	public class HorizontalLayout : IDisposable {
		public HorizontalLayout(params GUILayoutOption[] options) {
			GUILayout.BeginHorizontal(options);
		}

		public void Dispose() {
			GUILayout.EndHorizontal();
		}
	}

	public class ObservedTextField {
		
		string _text = "";
		Action<string> _onChanged = null;

		public ObservedTextField(Action<string> onChanged) {
			_onChanged = onChanged;
		}

		public void Read() {
			var tempText = GUILayout.TextField(_text);
			var changed = (tempText != _text);
			_text = tempText;
			if( changed ) {
				_onChanged(_text);
			}
		}
	}
}
