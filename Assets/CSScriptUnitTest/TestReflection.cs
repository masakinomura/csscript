﻿using System.Collections;
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

		[Test]
		public void TestCanCastSimple () {
			Assert.True (ReflectionUtil.CanCast (typeof (float), 4));
			Assert.True (ReflectionUtil.CanCast (typeof (float), 3.3f));
			Assert.True (ReflectionUtil.CanCast (typeof (float), 5.5));
			Assert.True (ReflectionUtil.CanCast (typeof (double), 6f));
			Assert.False (ReflectionUtil.CanCast (typeof (string), 3.3f));
			Assert.True (ReflectionUtil.CanCast (typeof (Simple), new Simple ()));
			Assert.True (ReflectionUtil.CanCast (typeof (Simple), 3));
			Assert.False (ReflectionUtil.CanCast (typeof (System.Text.ASCIIEncoding), new Simple ()));

			Assert.True (ReflectionUtil.CanCast (typeof (System.DateTime), System.DateTime.Now));

			Assert.False (ReflectionUtil.CanCast (typeof (int), "3"));
		}

		[Test]
		public void TestCanCastInterface () {
			Assert.True (ReflectionUtil.CanCast (typeof (ITest), new ClassWithInterface ()));
			Assert.True (ReflectionUtil.CanCast (typeof (ITest), new ChildClass ()));
		}

		[Test]
		public void TestCanCastInheritance () {
			Assert.True (ReflectionUtil.CanCast (typeof (ClassWithInterface), new ChildClass ()));
			Assert.True (ReflectionUtil.CanCast (typeof (ClassWithInterface), new GroundChildClass ()));
			Assert.False (ReflectionUtil.CanCast (typeof (ChildClass), new ClassWithInterface ()));
		}

		[Test]
		public void TestCanCastGeneric () {
			Assert.False (ReflectionUtil.CanCast (typeof (GenricOne<>), new GenricOne<int> ()));
			Assert.True (ReflectionUtil.CanCast (typeof (GenricOne<int>), new GenricOne<int> ()));
			Assert.False (ReflectionUtil.CanCast (typeof (GenricOne<int>), new GenricOne<string> ()));
		}

		[Test]
		public void TestCastNumber () {
			Assert.AreEqual (typeof (float), ReflectionUtil.Cast (typeof (float), 3).GetType ());
			Assert.AreEqual (typeof (int), ReflectionUtil.Cast (typeof (int), 3.4).GetType ());
			Assert.AreEqual (3, ReflectionUtil.Cast (typeof (int), 3.4));

		}

		[Test]
		public void TestCastImplecit () {
			//Assert.AreEqual (true, (bool)new Simple ());
			Assert.AreEqual (true, ReflectionUtil.Cast (typeof (bool), new Simple ()));

			Simple s = ReflectionUtil.Cast (typeof (Simple), 4) as Simple;
			Assert.AreEqual (4, s._a);
		}

	}

}