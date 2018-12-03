using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public class CSScope {
		public CSScope _parent;

		Dictionary<string, object> _variables = new Dictionary<string, object> ();

		public void AddVarible (string variableName) {
			if (_variables.ContainsKey (variableName)) {
				CSLog.E ("Variable " + variableName + " already exists...");
				return;
			}
			_variables[variableName] = null;
		}

		public bool HasVariable (string variableName) {
			return _variables.ContainsKey (variableName);
		}

		public bool TryGetVariable (string variableName, out object val) {
			return _variables.TryGetValue (variableName, out val);
		}

		public object GetVariable (string variableName) {
			object val = null;
			if (!_variables.TryGetValue (variableName, out val)) {
				CSLog.E ("Variable: " + variableName + " does not exist...");
			}
			return val;
		}

		public object SetVariable (string variableName, object val) {
			if (!_variables.ContainsKey (variableName)) {
				CSLog.E ("Variable " + variableName + " does not exist...");
				return null;
			}
			_variables[variableName] = val;
			return val;
		}
	}
}