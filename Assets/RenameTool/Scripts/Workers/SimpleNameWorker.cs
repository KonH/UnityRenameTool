namespace UnityRenameTool {
	public class SimpleNameWorker : INameWorker {
		
		string _condition = null;

		public SimpleNameWorker(string condition) {
			_condition = condition;
		}

		public bool IsWantedName(string name) {
			return name.Contains(_condition);
		}
		
		public string Replace(string name, string replacer, int count) {
			if( count < 0 ) {
				return name.Replace(_condition, replacer);
			} else {
				return ReplaceWithCount(name, replacer, count);
			}
		}

		string ReplaceWithCount(string name, string replacer, int count) {
			var pos = name.IndexOf(_condition);
			if ( pos < 0 ) {
				return name;
			}
			var newCount = count - 1;
			var newName = name.Substring(0, pos) + replacer + name.Substring(pos + _condition.Length);
			if( newCount == 0 ) {
				return newName;
			} else {
				return ReplaceWithCount(name, replacer, newCount);
			}
		}
	}
}