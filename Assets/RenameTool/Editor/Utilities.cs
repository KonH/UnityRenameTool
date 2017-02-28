using System;
using UnityEngine;
using UnityEditor;

namespace UnityRenameTool.Editor {
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
		
		Action<string> _onChanged = null;

		public string Value { get; private set; }

		public ObservedTextField(Action<string> onChanged) {
			Value = "";
			_onChanged = onChanged;
		}

		public void Read() {
			var tempValue = GUILayout.TextField(Value);
			var changed = (tempValue != Value);
			Value = tempValue;
			if( changed ) {
				_onChanged(Value);
			}
		}
	}

	public class ObservedFindModeField {

		Action<FindMode> _onChanged = null;

		public FindMode Value { get; private set; }

		public ObservedFindModeField(Action<FindMode> onChanged) {
			_onChanged = onChanged;
		}

		public void Read() {
			var tempValue = (FindMode)EditorGUILayout.EnumPopup(Value);
			var changed = (tempValue != Value);
			Value = tempValue;
			if( changed ) {
				_onChanged(Value);
			}
		}
	}
}
