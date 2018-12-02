using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript.Test {
	public class A<O, P> {
		public class B<R> {
			public class C { }

		}
	}
	public class TestReflection {

		[SetUp]
		public void SetUp () {
			ReflectionUtil.Initialize ();
		}

		[TearDown]
		public void TearDown () {
			ReflectionUtil.Destroy ();
		}

		[Test]
		public void TestGetTypeInt () {
			Assert.AreEqual (typeof (int), ReflectionUtil.GetType ("", "int"));
			Assert.AreEqual (typeof (System.Int32), ReflectionUtil.GetType ("System", "System.Int32"));
			Assert.AreEqual (typeof (System.Int32), ReflectionUtil.GetType ("System", "Int32"));

		}

		[Test]
		public void TestGetTypeClass () {
			Assert.AreEqual (typeof (Screen), ReflectionUtil.GetType ("UnityEngine", "Screen"));
		}

		[Test]
		public void TestGetTypeGeneric () {
			//Debug.Log(typeof(List<>).FullName);
			Assert.AreEqual (typeof (List<>), ReflectionUtil.GetType ("System.Collections.Generic", "System.Collections.Generic.List`1"));
			Assert.AreEqual (typeof (List<>), ReflectionUtil.GetType ("System.Collections.Generic", "List`1"));

			//Assert.AreEqual (typeof (List<>), ReflectionUtil.GetType ("System.Collections.Generic", "System.Collections.Generic.List", 1));
			//Assert.AreEqual (typeof (List<>), ReflectionUtil.GetType ("System.Collections.Generic", "List", 1));
		}

		[Test]
		public void TestGetTypeGenericSpecific () {
			Debug.Log (typeof (List<int>).FullName);
			Debug.Log (typeof (A<int, List<int>>.B<string>.C).FullName);
			Assert.AreEqual (typeof (List<string>), ReflectionUtil.GetType ("System.Collections.Generic", "System.Collections.Generic.List`1[[System.String]]"));
			Assert.AreEqual (typeof (List<int>), ReflectionUtil.GetType ("System.Collections.Generic", "System.Collections.Generic.List`1[[System.Int32]]"));

			Assert.AreEqual (typeof (List<string>), ReflectionUtil.GetType ("System.Collections.Generic", "System.Collections.Generic.List`1[[System.String]]"));
			Assert.AreEqual (typeof (List<int>), ReflectionUtil.GetType ("System.Collections.Generic", "System.Collections.Generic.List`1[[System.Int32]]"));

			//Assert.AreEqual (typeof (List<int>), ReflectionUtil.GetType ("System.Collections.Generic", "System.Collections.Generic.List", typeof (int)));

			Assert.AreEqual (typeof (List<int>.Enumerator), ReflectionUtil.GetType ("System.Collections.Generic", "System.Collections.Generic.List`1+Enumerator[System.Int32]"));
		}

		[Test]
		public void TestGetTypeInAssembly () {
			Assert.AreEqual (typeof (CSScript.CSNode), ReflectionUtil.GetType ("CSScript", "CSNode"));
			Assert.AreEqual (typeof (System.Text.ASCIIEncoding), ReflectionUtil.GetType ("System", "Text.ASCIIEncoding"));
			Assert.AreEqual (typeof (System.Text.ASCIIEncoding), ReflectionUtil.GetType ("System.Text", "ASCIIEncoding"));

		}

	}

}