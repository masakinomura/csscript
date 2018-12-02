﻿using System.Collections;
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
		}

	}
}