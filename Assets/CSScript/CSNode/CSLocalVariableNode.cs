using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSLocalVariableNode : CSNode {

		public string _variableName;
		public System.Type _type;

		public bool _declaration;

		public CSLocalVariableNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state, CSObject curObj) {
			if (_declaration) {
				CSObject obj = CSObject.LocalVariableObject (this, _type, _variableName, null);
				state.AddVariable (_variableName, obj);
				return obj;
			} else {
				return state.GetVariable (_variableName);
			}

		}
	}

}