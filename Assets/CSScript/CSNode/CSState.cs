using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public class CSState {
		CSScope _current = null;

		public CSScope Current {
			get {
				if (_current == null) {
					CSLog.E ("current scope is missing");
				}
				return _current;
			}
		}

		public CSState () {
			_current = new CSScope ();
		}

		public bool HasVariable (string variableName) {
			CSScope next = Current;
			while (next != null) {
				if (next.HasVariable (variableName)) {
					return true;
				}

				next = next._parent;
			}
			return false;
		}

		public CSScope AddVariable (string variableName) {
			Current.AddVarible (variableName);
			return Current;
		}

		public object GetVariable (string variableName) {
			CSScope next = Current;
			object val;
			while (next != null) {
				if (next.TryGetVariable (variableName, out val)) {
					return val;
				}
				next = next._parent;
			}
			CSLog.E ("Variable: " + variableName + " does not exist...");
			return null;
		}
	}
}