using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript.Test {
	public class Simple {
		public int _a = 1;
		public float _b = 2.0f;

		public Simple _i;

		public string _s;

		public Simple () { }

		public Simple (int a) {
			CSLog.D ("int one");
			_a = a;
		}

		public Simple (float b) {
			CSLog.D ("float one");
			_b = b;
		}

		public Simple (int a, float b) {
			_a = a;
			_b = b;
		}

		public Simple (string s) {
			CSLog.D ("sting one");
			_s = s;

		}

		public Simple (Simple i) {
			CSLog.D("instance one");
			_i = i;
		}
	}

	public class GenricOne<T> {
		public T pa;
	}
}