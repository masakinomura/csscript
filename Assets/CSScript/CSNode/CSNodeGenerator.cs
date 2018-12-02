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

		public override CSNode VisitFloatAtomExp (CSScriptParser.FloatAtomExpContext context) {
			CSFloatNode node = new CSFloatNode (context.Start.Line, context.Start.Column);
			float val = 0;
			if (!float.TryParse (context.FLOAT ().GetText ().Replace("f", ""), out val)) {
				CSLog.E (node, "failed to parse float: #" + context.FLOAT ().GetText () + "#");
			}
			node._val = val;
			return node;
		}

		public override CSNode VisitAssignmentExp (CSScriptParser.AssignmentExpContext context) {
			CSOPAssignNode node = new CSOPAssignNode (context.Start.Line, context.Start.Column);
			EvaluateExpressions (node, context.expression ());
			return node;
		}

		public override CSNode VisitParameters (CSScriptParser.ParametersContext context) {
			CSNode node = new CSNode (context.Start.Line, context.Start.Column);
			EvaluateExpressions (node, context.expression ());
			return node;
		}

		public override CSNode VisitVariable (CSScriptParser.VariableContext context) {
			CSVariableNode variableNode = new CSVariableNode (context.Start.Line, context.Start.Column);
			variableNode._variableName = context.NAME ().GetText ();
			variableNode._declare = (context.VAR () != null);
			return variableNode;
		}

		public override CSNode VisitNewExp (CSScriptParser.NewExpContext context) {
			CSOPNewNode node = new CSOPNewNode (context.Start.Line, context.Start.Column);
			node._children = new CSNode[2];
			node._children[0] = Visit (context.vartypes ());
			node._children[1] = Visit (context.parameters ());
			return node;
		}

		public override CSNode VisitVartypes (CSScriptParser.VartypesContext context) {
			CSTypeNode node = new CSTypeNode (context.Start.Line, context.Start.Column);

			CSScriptParser.VartypeContext[] vartypes = context.vartype ();
			int varLen = vartypes.Length;

			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			System.Text.StringBuilder sbTemplate = new System.Text.StringBuilder ();
			sbTemplate.Append ('[');

			bool isThereTemplate = false;

			for (int i = 0; i < varLen; ++i) {
				CSScriptParser.VartypeContext next = vartypes[i];
				if (i != 0) {
					sb.Append ('.');
				}

				sb.Append (ReflectionUtil.GetCleanNameIfPrimitive (next.NAME ().GetText ()));

				CSScriptParser.Template_typeContext template = next.template_type ();
				if (template != null) {
					CSScriptParser.VartypesContext[] templatetypes = template.vartypes ();
					int tempVarCount = templatetypes.Length;
					if (tempVarCount > 0) {
						if (isThereTemplate) {
							sbTemplate.Append (',');
						} else {
							isThereTemplate = true;
						}

						sb.Append ('`');
						sb.Append (tempVarCount.ToString ());
						sbTemplate.Append ('[');
						node._children = new CSNode[tempVarCount];
						for (int j = 0; j < tempVarCount; ++j) {
							CSTypeNode child = VisitVartypes (templatetypes[j]) as CSTypeNode;
							sbTemplate.Append (child._typeString);
						}
						sbTemplate.Append (']');
					}
				}
			}
			sbTemplate.Append (']');

			if (isThereTemplate) {
				sb.Append (sbTemplate.ToString ());
			}

			string typeString = sb.ToString ();
			System.Type type = ReflectionUtil.GetType (null, typeString);
			node._type = type;
			node._typeString = typeString;
			node._assemblyName = type.Assembly.GetCleanName ();
			CSLog.D ("full name: " + node._typeString + " in the assembly: " + node._assemblyName);

			return node;
		}

	}
}