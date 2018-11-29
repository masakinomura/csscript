using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSVariableNode : CSNode {

		public bool _declare;
		public string _variableName;

		public CSVariableNode (int line, int column) : base (line, column) {}

		public override CSObject Evaluate (CSState state) {
			if (_declare) {
				state.AddVariable (_variableName);
			}
			return new CSObject (this, state.Current, _variableName);
		}
	}

}