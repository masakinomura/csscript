using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSFunctionNode : CSNode {

		public string _functionName;

		public CSTypeNode[] _genericParameters;

		public CSNode Selectors { get { return _children[0]; } }
		public CSNode Parameters { get { return _children[1]; } }

		public CSFunctionNode (int line, int column) : base (line, column) { }

		public override CSObject Evaluate (CSState state, CSObject curObj) {

			CSObject selectedObj = Selectors.Evaluate (state, curObj);

			object[] parameters = null;

			if (Parameters.ChildCount > 0) {
				CSNode[] children = Parameters._children;
				int clen = children.Length;
				parameters = new object[clen];

				for (int i = 0; i < clen; ++i) {
					CSObject next = children[i].Evaluate (state, curObj);
					parameters[i] = next.Value;
				}
			}

			System.Type[] types = null;
			if (_genericParameters != null) {
				int glen = _genericParameters.Length;
				types = new System.Type[glen];
				for (int i = 0; i < glen; ++i) {
					types[i] = _genericParameters[i]._type;
				}
			}

			System.Type retType;

			object retVal = ReflectionUtil.CallMethod (selectedObj.Value, _functionName, types, out retType, parameters);
			return CSObject.TempVariableObject (this, retType, retVal);
		}
	}
}