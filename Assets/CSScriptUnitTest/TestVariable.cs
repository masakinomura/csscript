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
			CSState state = new CSState ();
			root.Evaluate (state);
			Assert.AreEqual (null, state.GetVariable ("myVar"));
		}
	}
}