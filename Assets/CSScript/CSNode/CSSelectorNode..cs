using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSSelectorNode : CSNode {
		public string[] _selectors;

		public CSSelectorNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state, CSObject curObj) {
			if (curObj == null) {
				CSLog.E (this, "curObj is missing");
				return null;
			}

			int len = _selectors.Length;
			object obj = curObj.Value;

			for (int i = 0; i < len - 1; ++i) {
				if (obj == null) {
					CSLog.E (this, "object is not assgined");
					return null;
				}

				obj = ReflectionUtil.Get (obj, _selectors[i]);
			}

			if (obj == null) {
				CSLog.E (this, "object is not assgined");
				return null;
			}
			string variableName = _selectors[len - 1];
			System.Type type = ReflectionUtil.GetFieldType (obj, variableName);

			return CSObject.InstanceVariableObject (this, type, obj, variableName);
		}
	}

}