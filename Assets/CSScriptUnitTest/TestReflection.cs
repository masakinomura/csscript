using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript {
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
			Assert.AreEqual (typeof (int), ReflectionUtil.GetType ("int"));
			Assert.AreEqual (typeof (System.Int32), ReflectionUtil.GetType ("System.Int32", "System"));
			Assert.AreEqual (typeof (System.Int32), ReflectionUtil.GetType ("Int32", "System"));

		}

		[Test]
		public void TestGetTypeClass () {
			Assert.AreEqual (typeof (Screen), ReflectionUtil.GetType ("Screen", "UnityEngine"));
		}

		[Test]
		public void TestGetTypeGeneric () {
			//Debug.Log(typeof(List<>).FullName);
			Assert.AreEqual (typeof (List<>), ReflectionUtil.GetType ("System.Collections.Generic.List`1", "System.Collections.Generic"));
			Assert.AreEqual (typeof (List<>), ReflectionUtil.GetType ("List`1", "System.Collections.Generic"));
			
			
			Assert.AreEqual (typeof (List<>), ReflectionUtil.GetGenericType ("System.Collections.Generic.List", 1, "System.Collections.Generic"));
			Assert.AreEqual (typeof (List<>), ReflectionUtil.GetGenericType ("List", 1, "System.Collections.Generic"));
		}

		[Test]
		public void TestGetTypeGenericSpecific () {
			Debug.Log (typeof (List<int>).FullName);
			Assert.AreEqual (typeof (List<string>), ReflectionUtil.GetType ("System.Collections.Generic.List`1[System.String]", "System.Collections.Generic"));
			Assert.AreEqual (typeof (List<int>), ReflectionUtil.GetType ("System.Collections.Generic.List`1[System.Int32]", "System.Collections.Generic"));

			Assert.AreEqual (typeof (List<string>), ReflectionUtil.GetType ("System.Collections.Generic.List`1[System.String]", "System.Collections.Generic"));
			Assert.AreEqual (typeof (List<int>), ReflectionUtil.GetType ("System.Collections.Generic.List`1[System.Int32]", "System.Collections.Generic"));
			

			Assert.AreEqual (typeof (List<int>), ReflectionUtil.GetGenericType ("System.Collections.Generic.List", "System.Collections.Generic", typeof (int)));
		}

	}
}