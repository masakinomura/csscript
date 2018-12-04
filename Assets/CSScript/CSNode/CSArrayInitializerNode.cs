using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	[System.Serializable]
	public class CSArrayInitializerNode : CSNode {

		public CSArrayInitializerNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state) {
			return null;
		}

		public object[] EvaluateElements (CSState state, System.Type type) {
			int len = ChildCount;
			object[] elements = new object[len];

			for (int i = 0; i < len; ++i) {
				CSObject obj = _children[i].Evaluate (state);
				elements[i] = obj.GetAs (type);
			}
			return elements;
		}
	}
}