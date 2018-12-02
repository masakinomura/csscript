using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSOPNewNode : CSNode {
		public CSTypeNode NewType { get { return _children[0] as CSTypeNode; } }
		public CSNode NewParameters { get { return _children[1]; } }

		public CSOPNewNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state) {
			if (ChildCount != 2) {
				CSLog.E (this, "new operator has invalid # of children...");
				return null;
			}

			if (NewType == null || NewType._type == null) {
				CSLog.E (this, "new operator does not have a proper type...");
				return null;
			}

			object[] parameters = null;

			if (NewParameters != null && NewParameters.ChildCount > 0) {
				CSNode[] pchildren = NewParameters._children;
				int pCount = pchildren.Length;
				parameters = new object[pCount];
				for (int i = 0; i < pCount; ++i) {
					parameters[i] = pchildren[i].Evaluate(state).Value;
				}
			}

			object newInstance;
			if (parameters == null) {
				newInstance = System.Activator.CreateInstance (NewType._type);
			} else {
				newInstance = System.Activator.CreateInstance (NewType._type, parameters);
			}

			CSObject obj = new CSObject (this, newInstance);
			return obj;
		}
	}

}