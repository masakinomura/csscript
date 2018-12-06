using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSNode {

		public CSNode[] _children;

		public int _line;
		public int _column;

		public CSNode (int line, int column) {
			_line = line;
			_column = column;
		}

		public int ChildCount {
			get {
				if (_children == null) {
					return 0;
				} else {
					return _children.Length;
				}
			}
		}

		public CSNode GetChild (int index) {
			if (_children == null) {
				CSLog.E (this, "_children is null");
				return null;
			}

			if (index < 0 || index >= ChildCount) {
				CSLog.E (this, "index out of bound");
				return null;
			}

			return _children[index];
		}

		public CSObject Evaluate () {
			CSState state = new CSState ();
			return Evaluate (state, null);
		}

		public virtual CSObject Evaluate (CSState state, CSObject curObj) {
			if (_children == null) {
				return null;
			}

			CSObject lastResult = null;

			int len = _children.Length;
			for (int i = 0; i < len; ++i) {
				lastResult = _children[i].Evaluate (state, curObj);
			}

			return lastResult;
		}

	}

}