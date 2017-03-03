using System.Text.RegularExpressions;

namespace UniversalRenameTool {
	public class RegExNameWorker : INameWorker {
		
		Regex _regex = null;

		public RegExNameWorker(string condition, bool ignoreCase) {
			_regex =
				ignoreCase 
				? new Regex(condition, RegexOptions.IgnoreCase)
				: new Regex(condition);
		}

		public bool IsWantedName(string name) {
			return _regex.IsMatch(name);
		}
		
		public string Replace(string name, string replacer, int count) {
			return _regex.Replace(name, replacer, count);
		}
	}
}