namespace UnityRenameTool {
	public class SimpleNameWorker : INameWorker {
		
		string _condition  = null;
		bool   _ignoreCase = false;

		public SimpleNameWorker(string condition, bool ignoreCase) {
			_ignoreCase = ignoreCase;
			_condition = 
				_ignoreCase 
				? condition.ToLowerInvariant()
				: condition;
		}

		public bool IsWantedName(string name) {
			if( _ignoreCase ) {
				return name.ToLowerInvariant().Contains(_condition);
			}
			return name.Contains(_condition);
		}
		
		public string Replace(string name, string replacer, int count) {
			if( count < 0 ) {
				if( !_ignoreCase ) {
					return name.Replace(_condition, replacer);
				} else {
					return ReplaceWithCount(name, replacer, int.MaxValue);
				}
			} else {
				return ReplaceWithCount(name, replacer, count);
			}
		}

		string ReplaceWithCount(string name, string replacer, int count) {
			var pos = name.ToLowerInvariant().IndexOf(_condition);
			if ( pos < 0 ) {
				return name;
			}
			var newCount = count - 1;
			var newName = name.Substring(0, pos) + replacer + name.Substring(pos + _condition.Length);
			if( newCount == 0 ) {
				return newName;
			} else {
				return ReplaceWithCount(newName, replacer, newCount);
			}
		}
	}
}