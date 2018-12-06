using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSClassInitializerNode : CSNode {
		public string[] _variableNames;
		public CSClassInitializerNode (int line, int column) : base (line, column) { }

		public int Count { get { return (_variableNames != null?_variableNames.Length : 0); } }

		public override CSObject Evaluate (CSState state, CSObject curObj) {
			return null;
		}

		public KeyValuePair<string, object> Evaluate (CSState state, CSObject curObj, int index, object target) {
			KeyValuePair<string, object> element = new KeyValuePair<string, object> (
				_variableNames[index],
				_children[index].Evaluate (state, curObj).Value);
			return element;
		}
	}
}