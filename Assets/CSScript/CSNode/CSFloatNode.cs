using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSFloatNode : CSNode {
		public float _val;

		public CSFloatNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state) {
			return new CSObject (this, _val);
		}
	}

}