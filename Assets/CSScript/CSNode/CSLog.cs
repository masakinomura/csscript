using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public class CSLog {
		public static void D (string msg) {
			Debug.Log ("[CSScript] " + msg);
		}

		public static void W (string msg) {
			Debug.LogWarning ("[CSScript] " + msg);
		}

		public static void E (string msg) {
			Debug.LogError ("[CSScript] " + msg);
		}

		public static void D (CSNode node, string msg) {
			Debug.Log ("[CSScript line: " + node._line + " col: " + node._column + "] " + msg);
		}

		public static void W (CSNode node, string msg) {
			Debug.LogWarning ("[CSScript line: " + node._line + " col: " + node._column + "] " + msg);
		}

		public static void E (CSNode node, string msg) {
			Debug.LogError ("[CSScript line: " + node._line + " col: " + node._column + "] " + msg);
		}

		public static void E (int line, int column, string msg) {
			Debug.LogError ("[CSScript line: " + line + " col: " + column + "] " + msg);
		}		
	}
}