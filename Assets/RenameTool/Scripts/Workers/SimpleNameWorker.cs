namespace UnityRenameTool {
	public class SimpleNameWorker : INameWorker {
		
		string _condition = null;

		public SimpleNameWorker(string condition) {
			_condition = condition;
		}

		public bool IsWantedName(string name) {
			return name.Contains(_condition);
		}
		
		public string Replace(string name, string replacer) {
			return name.Replace(_condition, replacer);
		}
	}
}