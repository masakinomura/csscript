using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSOPNewNode : CSNode {
		public CSTypeNode NewType { get { return _children[0] as CSTypeNode; } }
		public CSNode NewParameters { get { return _children[1]; } }
		public CSArrayIndexNode ArrayIndex { get { return _children[2] as CSArrayIndexNode; } }
		public bool IsArray { get { return _children[2] != null; } }

		public CSArrayInitializerNode ArrayInitializer { get { return _children[3] as CSArrayInitializerNode; } }

		public CSNode[] _arrayInitializer;
		public KeyValuePair<CSNode, CSNode>[] _dictionaryInitializer;
		public KeyValuePair<string, CSNode>[] _objectInitializer;

		public CSOPNewNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state) {
			if (ChildCount != 6) {
				CSLog.E (this, "new operator has invalid # of children...");
				return null;
			}

			if (NewType == null || NewType._type == null) {
				CSLog.E (this, "new operator does not have a proper type...");
				return null;
			}

			object newInstance = null;
			System.Type type = null;
			if (IsArray) {
				type = NewType._arrayType;
				int count = ArrayIndex.EvaluateIndex ();
				if (ArrayInitializer != null) {
					object[] elements = ArrayInitializer.EvaluateElements (state, NewType._type);
					int len = elements.Length;
					int allocCount = (len > count? len : count);
					newInstance = System.Activator.CreateInstance (type, allocCount);
					System.Array array = newInstance as System.Array;
					for (int i = 0; i < len; ++i) {
						array.SetValue(elements[i], i);
					}
				} else {
					if (count < 0) {
						throw new System.ArgumentException ("array needs an initializer or count");
					} else {
						newInstance = System.Activator.CreateInstance (type, count);
					}
				}
			} else {

				object[] parameters = null;

				if (NewParameters != null && NewParameters.ChildCount > 0) {
					CSNode[] pchildren = NewParameters._children;
					int pCount = pchildren.Length;
					parameters = new object[pCount];
					for (int i = 0; i < pCount; ++i) {
						parameters[i] = pchildren[i].Evaluate (state).Value;
					}
				}

				type = NewType._type;
				if (parameters == null) {
					newInstance = System.Activator.CreateInstance (type);
				} else {
					newInstance = System.Activator.CreateInstance (type, parameters);
				}
			}

			CSObject obj = CSObject.TempVariableObject (this, type, newInstance);
			return obj;
		}
	}

}