using System.Collections;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CSScript.Test {
	public class TestParse {

		public CSNode ParseScript (string script) {
			AntlrInputStream inputStream = new AntlrInputStream (script);
			CSScriptLexer lexer = new CSScriptLexer (inputStream);
			CommonTokenStream commonTokenStream = new CommonTokenStream (lexer);
			CSScriptParser parser = new CSScriptParser (commonTokenStream);
			parser.AddErrorListener (new CSDebugLogListener ());
			CSScriptParser.CodeContext code = parser.code ();
			CSNodeGenerator generator = new CSNodeGenerator ();
			return generator.Visit (code);
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