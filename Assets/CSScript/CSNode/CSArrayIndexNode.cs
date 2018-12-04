using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSArrayIndexNode : CSNode {
		public bool _isIndexed;
		public bool IsIndexed { get { return _children != null; } }
		public CSNode IndexNode { get { return _children[0]; } }

		public CSArrayIndexNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state) {
			if (IsIndexed) {
				return IndexNode.Evaluate (state);
			} else {
				return CSObject.ImmediateObject (this, typeof (int), -1);
			}
		}

		public int EvaluateIndex () {
			return Evaluate ().GetAs<int> ();
		}
	}
}