using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	[System.Serializable]
	public class CSIntNode : CSNode {
		public int _val;

		public override CSObject Evaluate (CSState state) {
			return new CSObject(_val);
		}
	}

}