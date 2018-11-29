using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSAssignNode : CSNode {
		public CSNode Left { get { return _children[0]; } }
		public CSNode Right { get { return _children[1]; } }

		public CSAssignNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state) {
			if (ChildCount != 2) {
				CSLog.E (this, "assigngment operator has invalid # of children...");
				return null;
			}
			CSObject left = Left.Evaluate (state);
			CSObject right = Right.Evaluate (state);

			left.Value = right.Value;
			return left;
		}
	}

}