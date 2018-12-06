using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSLocalVariableNode : CSNode {

		public string _variableName;
		public System.Type _type;

		public CSLocalVariableNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state) {
			state.AddVariable (_variableName);
			return CSObject.LocalVariableObject (this, _type, _variableName, null);
		}
	}

}