using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSIntNode : CSNode {
		public int _val;

		public CSIntNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state) {
			return CSObject.ImmediateObject (this, typeof (int), _val);
		}
	}

}