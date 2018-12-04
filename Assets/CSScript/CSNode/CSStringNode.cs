using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSStringNode : CSNode {
		public string _val;

		public CSStringNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state) {
			return CSObject.ImmediateObject (this, typeof (string), _val);
		}
	}

}