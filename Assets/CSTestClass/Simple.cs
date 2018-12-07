using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript.Test {
	public class Simple {

		public class Inner {
			public Inner (int a) {
				_a = a;
			}
			public int _a = 0;
		}

		static string Hoge { get; set; }

		static string HELLO = "HELLO";
		public int _a = 1;
		public float _b = 2.0f;

		public Simple _i;

		public string _s;

		public Simple () {
			_s = HELLO;
		}

		public static explicit operator bool (Simple x) {
			return x._a == 1;
		}

		public static implicit operator Simple (int a) {
			return new Simple (a);
		}

		public Simple (int a) {
			//CSLog.D ("int one");
			_a = a;
		}
		public Simple (float b) {
			//CSLog.D ("float one");
			_b = b;
		}

		public Simple (int a, float b) {
			_a = a;
			_b = b;
		}

		public Simple (string s) {
			//CSLog.D ("sting one");
			_s = s;

		}

		public Simple (Simple i) {
			//CSLog.D ("instance one");
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

		public static string StaticMethod (string arg) {
			return arg;
		}

		public G GetG<G> (G g) {
			return g;
		}

		public int IntProperty { get; set; }

		public string StringGetOnly { get { return _s; } }
		public string StringSetOnly { set { _s = value; } }
	}

	public class GenericOne<T> {

		public static Simple _s;

		public class Inner<A, B> {
			public Inner (A a, B b) {
				_a = a;
				_b = b;
			}
			public A _a;
			public B _b;
		}

		public GenericOne () { }

		public GenericOne (T pa) {
			_pa = pa;
		}

		public T _pa;

		public int GetInt () {
			return 3;
		}

		public T GetT (T t) {
			return t;
		}

		public S GetS<S> (S s) {
			return s;
		}

		public S Get2<S> (S s, T t) {
			_pa = t;
			return s;
		}

		public static string GetStr () {
			return "doge";
		}
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