﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSOPAssignNode : CSNode {
		public CSNode Left { get { return _children[0]; } }
		public CSNode Right { get { return _children[1]; } }

		public CSOPAssignNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state, CSObject curObj) {
			if (ChildCount != 2) {
				CSLog.E (this, "assigngment operator has invalid # of children...");
				return null;
			}
			CSObject left = Left.Evaluate (state, curObj);
			CSObject right = Right.Evaluate (state, curObj);

			return left.Assign (right);
		}
	}

}