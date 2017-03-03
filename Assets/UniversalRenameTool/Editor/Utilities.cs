using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UniversalRenameTool.Editor {
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

	public abstract class ObservedFieldBase<T> {

		public T Value { get; protected set; }
		protected Action<T> _onChanged = null;

		public ObservedFieldBase(Action<T> onChanged) {
			_onChanged = onChanged;
		}

		public void Read() {
			var tempValue = ReadValue();
			var changed = !Compare(tempValue, Value);
			Value = tempValue;
			if( changed ) {
				_onChanged(Value);
			}
		}

		bool Compare(T x, T y) {
			return EqualityComparer<T>.Default.Equals(x, y);
		}

		public abstract T ReadValue();
	}

	public class ObservedTextField: ObservedFieldBase<string> {
		
		public ObservedTextField(Action<string> onChanged):base(onChanged) {
			Value = "";
		}

		public override string ReadValue() {
			return GUILayout.TextField(Value);
		}
	}

	public class ObservedFindModeField: ObservedFieldBase<FindMode> {

		public ObservedFindModeField(Action<FindMode> onChanged):base(onChanged) {}

		public override FindMode ReadValue() {
			return (FindMode)EditorGUILayout.EnumPopup(Value);
		}
	}

	public class ObservedToggle: ObservedFieldBase<bool> {
		string _text = "";

		public ObservedToggle(Action<bool> onChanged, string text):base(onChanged) {
			_text = text;
		}

		public override bool ReadValue() {
			return GUILayout.Toggle(Value, _text);
		}
	}

	public class ObservedObjectField: ObservedFieldBase<UnityEngine.Object> {
		Type _type = null;

		public ObservedObjectField(Action<UnityEngine.Object> onChanged, Type type):base(onChanged) {
			_type = type;
		}

		public override UnityEngine.Object ReadValue() {
			return EditorGUILayout.ObjectField(Value, _type, true);
		}
	}

	public class ObservedLocationModeField: ObservedFieldBase<LocationMode> {

		public ObservedLocationModeField(Action<LocationMode> onChanged):base(onChanged) {}

		public override LocationMode ReadValue() {
			return (LocationMode)EditorGUILayout.EnumPopup(Value);
		}
	}
}
