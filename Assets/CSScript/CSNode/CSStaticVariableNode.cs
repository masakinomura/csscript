using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSStaticVariableNode : CSNode {
		public string _typeString;
		public string _variableName;
		public System.Type _staticType;
		public System.Type _type;

		public CSStaticVariableNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state, CSObject curObj) {
			return CSObject.StaticVariableObject (this, _type, _staticType, _variableName);
		}
	}

}