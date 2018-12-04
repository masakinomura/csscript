using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	[System.Serializable]
	public class CSDictionaryInitializerNode : CSNode {
		public CSNode[] _keys;
		public CSDictionaryInitializerNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state) {
			return CSObject.ImmediateObject (this, typeof (int), 0);
		}
	}
}