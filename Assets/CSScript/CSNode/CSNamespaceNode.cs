using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSNamespaceNode : CSNode {
		public string _namespace;

		public CSNamespaceNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state, CSObject curObj) {
			return null;
		}
	}

}