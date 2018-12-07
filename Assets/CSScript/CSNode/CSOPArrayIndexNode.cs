using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSOPArrayIndexNode : CSNode {

		public CSNode Left { get { return _children[0]; } }
		public CSArrayIndexNode IndexNode { get { return _children[1] as CSArrayIndexNode; } }

		public CSOPArrayIndexNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state, CSObject curObj) {
			if (ChildCount != 2) {
				CSLog.E (this, "assigngment operator has invalid # of children...");
				return null;
			}
			CSObject left = Left.Evaluate (state, curObj);

			if (left.CanCast<IList> ()) {

				int index = IndexNode.EvaluateIndex (state, curObj);
				System.Type type = ReflectionUtil.GetIListElementType (left.Type);
				return CSObject.ArrayVariableObject (this, type, left.Value, index);

			} else if (left.CanCast<IDictionary> ()) {

				string key = IndexNode.EvaluateKey (state, curObj);
				System.Type type = ReflectionUtil.GetIDictionaryElementType (left.Type);
				return CSObject.DictionaryVariableObject (this, type, left.Value, key);

			} else {
				CSLog.E (this, "you cannot use an index to non IList object");
				return null;
			}

		}
	}

}