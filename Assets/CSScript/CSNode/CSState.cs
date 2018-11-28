using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public class CSState {
		CSScope _current = null;

		public CSScope Current {
			get {
				if (_current == null) {
					Debug.LogError ("current scope is missing");
				}
				return _current;
			}
		}

		public CSState () {
			_current = new CSScope ();
		}

		public CSScope AddVariable (string variableName) {
			Current.AddVarible (variableName);
			return Current;
		}

		public object GetVariable (string variableName) {
			return Current.GetVariable (variableName);
		}
	}
}