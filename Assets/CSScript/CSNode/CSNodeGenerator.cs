using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	public class CSNodeGenerator : CSScriptBaseVisitor<CSNode> {

		void EvaluateExpressions (CSNode node, CSScriptParser.ExpressionContext[] expressions) {
			int len = expressions.Length;
			node._children = new CSNode[len];

			for (int i = 0; i < len; ++i) {
				node._children[i] = Visit (expressions[i]);
			}
		}

		public override CSNode VisitCode (CSScriptParser.CodeContext context) {
			CSNode node = new CSNode (context.Start.Line, context.Start.Column);

			CSScriptParser.LineContext[] lines = context.line ();
			int len = lines.Length;
			node._children = new CSNode[len];

			for (int i = 0; i < len; ++i) {
				node._children[i] = Visit (lines[i]);
			}

			return node;
		}

		public override CSNode VisitLine (CSScriptParser.LineContext context) {
			CSNode node = new CSNode (context.Start.Line, context.Start.Column);
			EvaluateExpressions (node, context.expression ());
			return node;
		}

		public override CSNode VisitIntAtomExp (CSScriptParser.IntAtomExpContext context) {
			CSIntNode node = new CSIntNode (context.Start.Line, context.Start.Column);
			int val = 0;
			if (!int.TryParse (context.INT ().GetText (), out val)) {
				CSLog.E (node, "failed to parse int: " + context.INT ().GetText ());
			}
			node._val = val;
			return node;
		}

		public override CSNode VisitAssignmentExp (CSScriptParser.AssignmentExpContext context) {
			CSAssignNode node = new CSAssignNode (context.Start.Line, context.Start.Column);
			EvaluateExpressions (node, context.expression ());
			return node;
		}

		public override CSNode VisitVariable (CSScriptParser.VariableContext context) {
			CSVariableNode variableNode = new CSVariableNode (context.Start.Line, context.Start.Column);
			variableNode._variableName = context.NAME ().GetText ();
			variableNode._declare = (context.VAR () != null);
			return variableNode;
		}

		public override CSNode VisitVartypes (CSScriptParser.VartypesContext context) {
			return VisitChildren (context);
		}
	}
}