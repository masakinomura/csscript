using System.Collections;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript {
	public class TestLiteral {

		public CSScriptParser CreateParser (string code) {
			AntlrInputStream inputStream = new AntlrInputStream (code);
			CSScriptLexer lexer = new CSScriptLexer (inputStream);
			CommonTokenStream commonTokenStream = new CommonTokenStream (lexer);
			return new CSScriptParser (commonTokenStream);
		}

		[Test]
		public void IntegerLiteral () {

			CSScriptParser parser = CreateParser ("123;");

			CSScriptParser.CodeContext code = parser.code ();

			CSNodeGenerator generator = new CSNodeGenerator ();

			CSNode root = generator.Visit (code);

			Assert.AreEqual (typeof (CSNode), root.GetType ());
			Assert.AreEqual (1, root.ChildCount);

			CSNode line = root.GetChild (0);

			Assert.AreEqual (typeof (CSNode), line.GetType ());
			Assert.AreEqual (1, line.ChildCount);

			//Antlr4.Runtime.ParserInterpreter

			//code.li

		}

		// A UnityTest behaves like a coroutine in PlayMode
		// and allows you to yield null to skip a frame in EditMode
		// [UnityTest]
		// public IEnumerator NewTestScriptWithEnumeratorPasses () {
		// 	// Use the Assert class to test conditions.
		// 	// yield to skip a frame
		// 	yield return null;
		// }
	}
}