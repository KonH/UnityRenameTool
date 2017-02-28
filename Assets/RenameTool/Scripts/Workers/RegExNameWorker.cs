using System.Text.RegularExpressions;

namespace UnityRenameTool {
	public class RegExNameWorker : INameWorker {
		
		Regex _regex = null;

		public RegExNameWorker(string condition) {
			_regex = new Regex(condition);
		}

		public bool IsWantedName(string name) {
			return _regex.IsMatch(name);
		}
		
		public string Replace(string name, string replacer, int count) {
			return _regex.Replace(name, replacer, count);
		}
	}
}