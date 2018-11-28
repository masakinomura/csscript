using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	public class CSNodeGenerator : CSScriptBaseVisitor<CSNode> {

		public override CSNode VisitCode (CSScriptParser.CodeContext context) {
			Debug.Log ("visit code");

			CSNode code = new CSNode ();

			CSScriptParser.LineContext[] lines = context.line ();
			int len = lines.Length;
			code._children = new CSNode[len];

			for (int i = 0; i < len; ++i) {
				code._children[i] = Visit (lines[i]);
			}

			return code;
		}

		public override CSNode VisitLine (CSScriptParser.LineContext context) {
			Debug.Log ("visit line");

			CSNode line = new CSNode ();
			CSScriptParser.ExpressionContext[] expressions = context.expression ();

			int len = expressions.Length;
			line._children = new CSNode[len];

			for (int i = 0; i < len; ++i) {
				line._children[i] = Visit (expressions[i]);
			}

			return line;
		}

		public override CSNode VisitIntAtomExp (CSScriptParser.IntAtomExpContext context) {
			CSIntNode intNode = new CSIntNode();

			int val = 0;
			if(!int.TryParse(context.INT().GetText(), out val)) {
				Debug.LogError("failed to parse int: " + context.INT().GetText());
			}
			Debug.Log ("visit int: " + val);
			intNode._val = val;
			return intNode;
		}

	}
}