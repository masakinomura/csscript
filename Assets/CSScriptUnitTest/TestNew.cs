using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript.Test {

	public class TestNew : TestParse {

		[SetUp]
		public void SetUp () {
			ReflectionUtil.Initialize ();
		}

		[TearDown]
		public void TearDown () {
			ReflectionUtil.Destroy ();
		}

		[Test]
		public void NewSimpleClass () {
			CSNode root = ParseScript ("var a = new CSScript.Test.Simple();");

			CSObject obj = root.Evaluate ();

			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (typeof (CSScript.Test.Simple), obj.Value.GetType ());
		}

		[Test]
		public void NewSimpleClassArgs () {
			CSNode root = ParseScript ("var a = new CSScript.Test.Simple(3);");

			CSObject obj = root.Evaluate ();

			Assert.AreEqual ("a", obj.Name);

			Assert.AreEqual (typeof (CSScript.Test.Simple), obj.Value.GetType ());
			CSScript.Test.Simple s = obj.Value as CSScript.Test.Simple;
			Assert.AreEqual (3, s._a);
		}

		[Test]
		public void NewSimpleClassArgsB () {
			CSNode root = ParseScript ("var a = new CSScript.Test.Simple(2.0f);");

			CSObject obj = root.Evaluate ();

			Assert.AreEqual ("a", obj.Name);

			Assert.AreEqual (typeof (CSScript.Test.Simple), obj.Value.GetType ());
			CSScript.Test.Simple s = obj.Value as CSScript.Test.Simple;
			Assert.AreEqual (2.0f, s._b);
		}

		[Test]
		public void NewSimpleClassArgsString () {
			CSNode root = ParseScript ("var a = new CSScript.Test.Simple(\"hello\");");

			CSObject obj = root.Evaluate ();

			Assert.AreEqual ("a", obj.Name);

			Assert.AreEqual (typeof (CSScript.Test.Simple), obj.Value.GetType ());
			CSScript.Test.Simple s = obj.Value as CSScript.Test.Simple;
			Assert.AreEqual ("hello", s._s);
		}

		[Test]
		public void NewSimpleClassArgsRef () {
			CSNode root = ParseScript ("var a = new CSScript.Test.Simple(new CSScript.Test.Simple());");

			CSObject obj = root.Evaluate ();

			Assert.AreEqual ("a", obj.Name);

			Assert.AreEqual (typeof (CSScript.Test.Simple), obj.Value.GetType ());
			CSScript.Test.Simple s = obj.Value as CSScript.Test.Simple;
			Assert.AreNotEqual (null, s._i);
		}

		[Test]
		public void NewSimpleClassArgs2 () {
			CSNode root = ParseScript ("var a = new CSScript.Test.Simple(3, 5.2f);");

			CSObject obj = root.Evaluate ();

			Assert.AreEqual ("a", obj.Name);

			Assert.AreEqual (typeof (CSScript.Test.Simple), obj.Value.GetType ());
			CSScript.Test.Simple s = obj.Value as CSScript.Test.Simple;
			Assert.AreEqual (3, s._a);
			Assert.AreEqual (5.2f, s._b);
			Assert.AreEqual (typeof (float), s._b.GetType ());
		}

		[Test]
		public void NewList () {
			CSNode root = ParseScript ("var list = new System.Collections.Generic.List<int>();");

			CSObject obj = root.Evaluate ();

			Assert.AreEqual ("list", obj.Name);
			Assert.AreEqual (typeof (List<int>), obj.Value.GetType ());

			List<int> list = obj.Value as List<int>;
			list.Add (34);
			Assert.AreEqual (1, list.Count);
			Assert.AreEqual (34, list[0]);
		}

		[Test]
		public void NewIntArray () {
			CSNode root = ParseScript ("var a = new int[32];");
			CSObject obj = root.Evaluate ();
			Assert.AreEqual (typeof (int[]), obj.Type);
			int[] arr = obj.GetAs<int[]> ();
			Assert.AreEqual (32, arr.Length);
		}

		[Test]
		public void NewListArray () {
			CSNode root = ParseScript ("var a = new System.Collections.Generic.List<int>[2];");
			CSObject obj = root.Evaluate ();
			Assert.AreEqual (typeof (List<int>[]), obj.Type);
			List<int>[] arr = obj.GetAs<List<int>[]> ();
			Assert.AreEqual (2, arr.Length);
		}

		[Test]
		public void NewIntArrayWithInitializer () {
			CSNode root = ParseScript ("var a = new int[]{1,2,3};");
			CSObject obj = root.Evaluate ();
			Assert.AreEqual (typeof (int[]), obj.Type);
			int[] arr = obj.GetAs<int[]> ();
			Assert.AreEqual (3, arr.Length);

			Assert.AreEqual (1, arr[0]);
			Assert.AreEqual (2, arr[1]);
			Assert.AreEqual (3, arr[2]);
		}

		[Test]
		public void NewClassArrayWithInitializer () {
			CSNode root = ParseScript ("var a = new CSScript.Test.Simple[]{new CSScript.Test.Simple(1), new CSScript.Test.Simple(2)};");
			CSObject obj = root.Evaluate ();
			Assert.AreEqual (typeof (Simple[]), obj.Type);
			Simple[] arr = obj.GetAs<Simple[]> ();
			Assert.AreEqual (2, arr.Length);

			Assert.AreEqual (1, arr[0]._a);
			Assert.AreEqual (2, arr[1]._a);
		}

		[Test]
		public void NewDictionaryWithInitializer () {
			Debug.Log (typeof (Dictionary<string, Simple>));
			CSNode root = ParseScript ("var a = new  System.Collections.Generic.Dictionary<string, CSScript.Test.Simple>(){" +
				"	{\"hoge\", new CSScript.Test.Simple (1)}," +
				"	{\"moge\", new CSScript.Test.Simple (2)}," +
				"};");

			CSObject obj = root.Evaluate ();
			Assert.AreEqual (typeof (Dictionary<string, Simple>), obj.Type);
			Dictionary<string, Simple> dict = obj.GetAs<Dictionary<string, Simple>> ();
			Assert.AreEqual (2, dict.Count);

			Assert.AreEqual (1, dict["hoge"]._a);
			Assert.AreEqual (2, dict["moge"]._a);
		}

		[Test]
		public void TestParserError () {
			Debug.Log (typeof (Dictionary<string, Simple>));
			CSNode root = ParseScript ("var a = new  System.Collections.Generic.Dictionary<string, CSScript.Test.Simple>;");
			LogAssert.Expect (LogType.Error, "[CSScript line: 1 col: 80] extraneous input ';' expecting {'(', '[', '.'}");
		}

	}
}