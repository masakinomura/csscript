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

		public static explicit operator bool (Simple x) {
			return x._a == 1;
		}

		public static implicit operator Simple (int a) {
			return new Simple (a);
		}

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
			CSLog.D ("instance one");
			_i = i;
		}

		public int GetInt () {
			return _a;
		}

		public void SetInt (int a) {
			_a = a;
		}

		public void SetString (string str) {
			_s = str;
		}

		public void SetString (Simple s) {
			_i = s;
		}
	}

	public class GenricOne<T> {
		public T pa;
	}

	public interface ITest {
		void Test ();
	}

	public class ClassWithInterface : ITest {
		void ITest.Test () {
			CSLog.D ("Test is called");
		}
	}

	public class ChildClass : ClassWithInterface {

	}

	public class GroundChildClass : ChildClass {

	}
}