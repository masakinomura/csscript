using System.Collections;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript.Test {
	public class TestLiteral : TestParse {

		[Test]
		public void IntegerInteger () {
			CSNode root = ParseScript ("123;");
			Assert.AreEqual (typeof (CSNode), root.GetType ());
			Assert.AreEqual (1, root.ChildCount);

			CSNode line = root.GetChild (0);
			Assert.AreEqual (typeof (CSNode), line.GetType ());
			Assert.AreEqual (1, line.ChildCount);

			CSNode intNode = line.GetChild (0);
			Assert.AreEqual (typeof (CSIntNode), intNode.GetType ());
			Assert.AreEqual (0, intNode.ChildCount);

			Assert.AreEqual (123, root.Evaluate ().Value);
		}

		[Test]
		public void IntegerString () {
		}		


	}
}