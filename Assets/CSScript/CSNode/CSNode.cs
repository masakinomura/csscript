using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSNode {

		public CSNode[] _children;

		public int ChildCount {
			get {
				if (_children == null) {
					return 0;
				} else {
					return _children.Length;
				}
			}
		}

		public CSNode GetChild(int index) {
			if (_children == null) {
				Debug.LogError("_children is null");
				return null;
			}

			if (index < 0 || index >= ChildCount) {
				Debug.LogError("index out of bound");
				return null;
			}

			return _children[index];
		}

	}

}