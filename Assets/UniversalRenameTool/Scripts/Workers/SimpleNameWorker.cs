using System.Collections.Generic;

namespace UniversalRenameTool {
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
			var readyName = 
				_ignoreCase 
				? name.ToLowerInvariant()
				: name;

			var indexes = new List<int>();
			for ( int index = 0; index < readyName.Length; index += _condition.Length ) {
        		index = readyName.IndexOf(_condition, index);
        		if ( ( index < 0 ) || (indexes.Count >= count) ) {
            		break;
				}
        		indexes.Add(index);
    		}

			var result = name;
			for( int i = indexes.Count - 1; i >= 0; i-- ) {
				var index = indexes[i];
				result = result.Substring(0, index) + replacer + result.Substring(index + _condition.Length);
			}
			return result;
		}
	}
}