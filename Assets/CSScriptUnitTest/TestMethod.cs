using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript.Test {
	public class TestMethod : TestParse {

		[SetUp]
		public void SetUp () {
			ReflectionUtil.Initialize ();
		}

		[TearDown]
		public void TearDown () {
			ReflectionUtil.Destroy ();
		}

		[Test]
		public void TestMethodInt () {
			CSNode root = ParseScript (
				"CSScript.Test.Simple s = new CSScript.Test.Simple(55);\n" +
				"int a = s.GetInt();"
			);

			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual (55, obj.GetAs<int> ());
		}

		[Test]
		public void TestMethodIntWrongCall () {
			CSNode root = ParseScript (
				"int a = CSScript.Test.Simple.GetInt();"
			);

			Assert.Throws<System.MissingMethodException> (() => {
				CSObject obj = root.Evaluate ();
			}, "static method: GetInt cannot be found in CSScript.Test.Simple");
		}

		[Test]
		public void TestStaticMethod () {
			CSNode root = ParseScript (
				"string a = CSScript.Test.Simple.StaticMethod(\"moge\");"
			);
			
			CSObject obj = root.Evaluate ();
			Assert.AreEqual ("a", obj.Name);
			Assert.AreEqual ("moge", obj.GetAs<string> ());
		}
	}
}