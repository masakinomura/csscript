using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	[System.Serializable]
	public class CSDictionaryInitializerNode : CSNode {
		public CSNode[] _keys;

		public int Count { get { return (_keys != null ? _keys.Length : 0); } }
		public CSDictionaryInitializerNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state, CSObject curObj) {
			return null;
		}

		public KeyValuePair<object, object> Evaluate (CSState state, CSObject curObj, int index, System.Type keyType, System.Type valueType) {
			KeyValuePair<object, object> element = new KeyValuePair<object, object> (
				_keys[index].Evaluate(state, curObj).GetAs(keyType),
				_children[index].Evaluate(state, curObj).GetAs(valueType));
			return element;
		}
	}	
}