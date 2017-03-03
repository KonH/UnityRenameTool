namespace UniversalRenameTool {
	public interface INameWorker {
		bool IsWantedName(string name);
		string Replace(string name, string replacer, int count);
	}
}
