using System.Collections;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript {
	public class TestVariable : TestParse {

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
			LogAssert.Expect (LogType.Error, "[CSScript line: 1 col: 4] you cannot assign value to immediate");
		}

		[Test]
		public void LineTest () {
			CSNode root = ParseScript ("var d = 3;\n var e = 4; var h = 5;");
			root.Evaluate ();
		}
	}
}