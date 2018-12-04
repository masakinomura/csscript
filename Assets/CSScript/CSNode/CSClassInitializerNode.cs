using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSClassInitializerNode : CSNode {
		public string[] _variableNames;
		public CSClassInitializerNode (int line, int column) : base (line, column) { }

		public int Count { get { return (_variableNames != null?_variableNames.Length : 0); } }

		public override CSObject Evaluate (CSState state) {
			return null;
		}

		public KeyValuePair<string, object> Evaluate (CSState state, int index, object target) {
			System.Type fieldType = ReflectionUtil.GetFieldType (target, _variableNames[index]);
			KeyValuePair<string, object> element = new KeyValuePair<string, object> (
				_variableNames[index],
				_children[index].Evaluate (state).Value);
			return element;
		}
	}
}