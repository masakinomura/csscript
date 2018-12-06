using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSSelectorNode : CSNode {
		public string[] _selectors;

		public CSSelectorNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state) {
			CSObject obj = null;
			return obj;
		}
	}

}