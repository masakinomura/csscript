using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSTypeNode : CSNode {
		public string _typeString;
		public string _assemblyName;
		public System.Type _type;

		public CSTypeNode (int line, int column) : base (line, column) { }
		public override CSObject Evaluate (CSState state) {
			return new CSObject (this, _type);
		}
	}

}