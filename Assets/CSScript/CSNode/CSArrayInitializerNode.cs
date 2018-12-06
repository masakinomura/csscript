using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	[System.Serializable]
	public class CSArrayInitializerNode : CSNode {

		public CSArrayInitializerNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state, CSObject curObj) {
			return null;
		}

		public object[] EvaluateElements (CSState state, CSObject curObj, System.Type type) {
			int len = ChildCount;
			object[] elements = new object[len];

			for (int i = 0; i < len; ++i) {
				CSObject obj = _children[i].Evaluate (state, curObj);
				elements[i] = obj.GetAs (type);
			}
			return elements;
		}
	}
}