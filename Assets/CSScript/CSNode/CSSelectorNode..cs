using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSSelectorNode : CSNode {
		public string _val;

		public CSSelectorNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state) {
			CSObject obj;
			if (!state.TryGetVariable (_val, out obj)) {
				CSLog.E (this, _val + " has to be a local variable...");
				return null;
			}
			return obj;
		}
	}

}