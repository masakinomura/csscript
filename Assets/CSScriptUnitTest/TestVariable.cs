using System.Collections;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript.Test {
	public class TestVariable : TestParse {

		[SetUp]
		public void SetUp () {
			ReflectionUtil.Initialize ();
		}

		[TearDown]
		public void TearDown () {
			ReflectionUtil.Destroy ();
		}

		[Test]
		public void Declaration () {
			CSNode root = ParseScript ("var myVar;");

			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("myVar", obj.Name);
			Assert.AreEqual (null, obj.Value);
		}

		[Test]
		public void AssignInt () {
			CSNode root = ParseScript ("var myVar = 3;");
			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("myVar", obj.Name);
			Assert.AreEqual (3, obj.Value);
		}

		[Test]
		public void AssignImmedidate () {
			CSNode root = ParseScript ("var 4 = 3;");
			root.Evaluate ();
			LogAssert.Expect (LogType.Error, "[CSScript line: 1 col: 4] missing NAME at '4'");
			LogAssert.Expect (LogType.Error, "[CSScript line: 1 col: 4] cannot assign to IMMEDIATE");
		}

		[Test]
		public void LineTest () {
			CSNode root = ParseScript ("var d = 3;\n var e = 4; var h = 5;");

			//position of '3' in 'var d = 3;'
			Assert.AreEqual (1, root._children[0]._children[0]._children[1]._line);
			Assert.AreEqual (8, root._children[0]._children[0]._children[1]._column);

			//position of 'e' in 'var e = 4;'
			Assert.AreEqual (2, root._children[1]._children[0]._children[0]._line);
			Assert.AreEqual (1, root._children[1]._children[0]._children[0]._column);

			//position of 'h' in 'var h = 5;'
			Assert.AreEqual (2, root._children[2]._children[0]._children[0]._line);
			Assert.AreEqual (12, root._children[2]._children[0]._children[0]._column);
		}

		[Test]
		public void TypedVarInt () {
			CSNode root = ParseScript ("int a = 3;");
			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (typeof (int), obj.Type);
			Assert.AreEqual (3, obj.Value);

		}

		[Test]
		public void TypedVarSimple () {
			CSNode root = ParseScript ("CSScript.Test.Simple a = new CSScript.Test.Simple(3);");
			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (typeof (Simple), obj.Type);
			Assert.AreEqual (3, obj.GetAs<Simple> ()._a);

		}

		[Test]
		public void TypedVarDictionary () {
			CSNode root = ParseScript ("System.Collections.Generic.Dictionary<string,int> a \n" +
				"= new System.Collections.Generic.Dictionary<string,int>(){\n" +
				"{\"hoge\", 3}\n" +
				"};"
			);
			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (typeof (System.Collections.Generic.Dictionary<string, int>), obj.Type);
			Assert.AreEqual (3, obj.GetAs<System.Collections.Generic.Dictionary<string, int>> () ["hoge"]);

		}
	}
}