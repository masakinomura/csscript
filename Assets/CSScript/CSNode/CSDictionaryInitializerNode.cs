using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	[System.Serializable]
	public class CSDictionaryInitializerNode : CSNode {
		public CSNode[] _keys;

		public int Count { get { return (_keys != null ? _keys.Length : 0); } }
		public CSDictionaryInitializerNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state) {
			return null;
		}

		public KeyValuePair<object, object> Evaluate (CSState state, int index, System.Type keyType, System.Type valueType) {
			KeyValuePair<object, object> element = new KeyValuePair<object, object> (
				_keys[index].Evaluate(state).GetAs(keyType),
				_children[index].Evaluate(state).GetAs(valueType));
			return element;
		}
	}
}