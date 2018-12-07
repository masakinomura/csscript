using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public class CSScope {
		public CSScope _parent;

		Dictionary<string, CSObject> _variables = new Dictionary<string, CSObject> ();

		string SkipAtMark (string val) {
			if (val == null) {
				return val;
			}
			if (val[0] == '@') {
				val = val.Substring (1);
			}
			return val;
		}

		public void AddVarible (string variableName, CSObject obj) {
			variableName = SkipAtMark (variableName);
			if (_variables.ContainsKey (variableName)) {
				CSLog.E ("Variable " + variableName + " already exists...");
				return;
			}
			_variables[variableName] = obj;
		}

		public bool HasVariable (string variableName) {
			variableName = SkipAtMark (variableName);
			return _variables.ContainsKey (variableName);
		}

		public bool TryGetVariable (string variableName, out CSObject val) {
			variableName = SkipAtMark (variableName);
			return _variables.TryGetValue (variableName, out val);
		}

		public object GetVariable (string variableName) {
			variableName = SkipAtMark (variableName);
			CSObject val = null;
			if (!_variables.TryGetValue (variableName, out val)) {
				CSLog.E ("Variable: " + variableName + " does not exist...");
			}
			return val;
		}

		public object SetVariable (string variableName, CSObject val) {
			variableName = SkipAtMark (variableName);
			if (!_variables.ContainsKey (variableName)) {
				CSLog.E ("Variable " + variableName + " does not exist...");
				return null;
			}
			_variables[variableName] = val;
			return val;
		}
	}
}