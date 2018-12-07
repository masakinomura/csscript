using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript.Test {
	public class TestArray : TestParse {

		[SetUp]
		public void SetUp () {
			ReflectionUtil.Initialize ();
		}

		[TearDown]
		public void TearDown () {
			ReflectionUtil.Destroy ();
		}

		[Test]
		public void TestArrayInt () {
			CSNode root = ParseScript (
				"int[] aa = new int[]{1,2,3,4};" +
				"int a = aa[2];"
			);

			List<int> a = new List<int> () { 1, 2, 3, 4 };

			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (3, obj.GetAs<int> ());
		}

		[Test]
		public void TestArrayIntAssign () {
			CSNode root = ParseScript (
				"int[] aa = new int[]{1,2,3,4};" +
				"aa[2] = 5;" +
				"int a = aa[2];"
			);

			List<int> a = new List<int> () { 1, 2, 3, 4 };

			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (5, obj.GetAs<int> ());
		}

		[Test]
		public void TestListInt () {
			CSNode root = ParseScript (
				"System.Collections.Generic.List<int> aa = new System.Collections.Generic.List<int>(){1,2,3,4};" +
				"int a = aa[2];"
			);

			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (3, obj.GetAs<int> ());
		}

		[Test]
		public void TestListIntAssign () {
			CSNode root = ParseScript (
				"System.Collections.Generic.List<int> aa = new System.Collections.Generic.List<int>(){1,2,3,4};" +
				"aa[2] = 5;" +
				"int a = aa[2];"
			);

			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (5, obj.GetAs<int> ());
		}

		[Test]
		public void TestDictionary () {
			CSNode root = ParseScript (
				"System.Collections.Generic.Dictionary<string, CSScript.Test.Simple> aa = new System.Collections.Generic.Dictionary<string, CSScript.Test.Simple> (){\n" +
				"	{\"hoge\", new CSScript.Test.Simple(33)},\n" +
				"	{\"moge\", new CSScript.Test.Simple(44)},\n" +
				"};\n" +
				"int a = aa[\"moge\"]._a;\n"
			);

			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (44, obj.GetAs<int> ());
		}

		[Test]
		public void TestDictionaryAssign () {
			CSNode root = ParseScript (
				"System.Collections.Generic.Dictionary<string, CSScript.Test.Simple> aa = new System.Collections.Generic.Dictionary<string, CSScript.Test.Simple> ();" +
				"aa[\"moge\"] = new CSScript.Test.Simple(55);" +
				"int a = aa[\"moge\"]._a;\n"
			);

			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (55, obj.GetAs<int> ());
		}
	}
}