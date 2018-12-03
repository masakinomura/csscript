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
			Assert.False (ReflectionUtil.CanCast (typeof (GenericOne<>), new GenericOne<int> ()));
			Assert.True (ReflectionUtil.CanCast (typeof (GenericOne<int>), new GenericOne<int> ()));
			Assert.False (ReflectionUtil.CanCast (typeof (GenericOne<int>), new GenericOne<string> ()));
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

		[Test]
		public void TestMethodSimple () {
			Simple s = new Simple ();
			s._a = 44;
			Assert.AreEqual (44, ReflectionUtil.CallMethod (s, "GetInt"));
			ReflectionUtil.CallMethod (s, "SetInt", 55);
			Assert.AreEqual (55, s._a);
		}

		[Test]
		public void TestMethodSimpleImplicitCast () {
			Simple s = new Simple ();

			ReflectionUtil.CallMethod (s, "SetInt", 3.4f);
			Assert.AreEqual (3, s._a);
		}

		[Test]
		public void TestMethodSimpleImplicitCast2 () {
			Simple s = new Simple ();
			Simple s2 = new Simple ();

			ReflectionUtil.CallMethod (s, "SetString", "hoge");
			Assert.AreEqual ("hoge", s._s);

			ReflectionUtil.CallMethod (s, "SetString", s2);
			Assert.AreEqual (s2, s._i);

			Assert.Throws<System.Reflection.AmbiguousMatchException> (
				() => {
					ReflectionUtil.CallMethod (s, "SetString", new object[] { null });
				}
			);

			ReflectionUtil.CallMethod (s, "SetString", new NullWithType (typeof (string)));
			Assert.AreEqual (null, s._s);

			ReflectionUtil.CallMethod (s, "SetString", new NullWithType (typeof (Simple)));
			Assert.AreEqual (null, s._s);

		}

		[Test]
		public void TestMethodGenericSimple () {
			Simple s = new Simple ();
			Simple s2 = new Simple ();
			Assert.Throws<System.ArgumentException> (
				() => {
					ReflectionUtil.CallMethod (s, "GetG", 3);
				}
			);
			Assert.AreEqual (3, ReflectionUtil.CallMethod (s, "GetG", new System.Type[] { typeof (int) }, 3));
			Assert.AreEqual (3.2f, ReflectionUtil.CallMethod (s, "GetG", new System.Type[] { typeof (float) }, 3.2f));
			Assert.AreEqual ("hoge", ReflectionUtil.CallMethod (s, "GetG", new System.Type[] { typeof (string) }, "hoge"));
			Assert.AreEqual (s2, ReflectionUtil.CallMethod (s, "GetG", new System.Type[] { typeof (Simple) }, s2));
		}

		[Test]
		public void TestMethodGenericSimple2 () {
			GenericOne<int> gi = new GenericOne<int> ();
			Assert.AreEqual (3, ReflectionUtil.CallMethod (gi, "GetInt"));
			Assert.AreEqual (4, ReflectionUtil.CallMethod (gi, "GetT", 4));
			Assert.AreEqual (4.3f, ReflectionUtil.CallMethod (gi, "GetS", new System.Type[] { typeof (float) }, 4.3f));

			Assert.AreEqual ("hoge", ReflectionUtil.CallMethod (gi, "Get2", new System.Type[] { typeof (string) }, "hoge", 5));
			Assert.AreEqual (5, gi._pa);
		}

	}

}