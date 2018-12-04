using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using UnityEngine;

namespace CSScript {

	public class CSDebugLogListener : BaseErrorListener {

		public override void SyntaxError (IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
			CSLog.E (line, charPositionInLine, msg);
		}

	}
}