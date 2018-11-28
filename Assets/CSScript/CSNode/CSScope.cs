﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public class CSScope {
		public CSScope _parent;

		Dictionary<string, object> _variables = new Dictionary<string, object> ();

		public void AddVarible (string variableName) {
			if (_variables.ContainsKey (variableName)) {
				Debug.LogError ("Variable " + variableName + " already exists...");
				return;
			}
			_variables[variableName] = null;
		}

		public object GetVariable (string variableName) {
			object val = null;
			if (!_variables.TryGetValue (variableName, out val)) {
				Debug.LogError ("Variable: " + variableName + " does not exist...");
			}
			return val;
		}

		public object SetVariable (string variableName, object val) {
			if (!_variables.ContainsKey (variableName)) {
				Debug.LogError ("Variable " + variableName + " does not exist...");
				return null;
			}
			_variables[variableName] = val;
			return val;
		}
	}
}