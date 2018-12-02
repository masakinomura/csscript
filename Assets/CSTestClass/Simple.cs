using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript.Test {
	public class Simple {
		public int _a = 1;
		public float _b = 2.0f;

		public Simple () { }

		public Simple (int a) {
			_a = a;
		}

		public Simple (int a, float b) {
			_a = a;
			_b = b;
		}
	}

	public class GenricOne<T> {
		public T pa;
	}
}