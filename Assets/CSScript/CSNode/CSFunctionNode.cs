using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSFunctionNode : CSNode {

		public string _functionName;

		public System.Type[] _genericParameters;

		public CSNode Parameters { get { return _children[0]; } }
		public CSNode Right { get { return _children[1]; } }

		public CSFunctionNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state, CSObject curObj) {
			if (ChildCount != 2) {
				CSLog.E (this, "assigngment operator has invalid # of children...");
				return null;
			}

			CSObject left = Parameters.Evaluate (state, curObj);
			CSObject right = Right.Evaluate (state, left);
			return right;
		}
	}

}