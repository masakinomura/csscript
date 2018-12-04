using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	public class CSNodeGenerator : CSScriptBaseVisitor<CSNode> {
		CSState _state = new CSState ();

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
			if (!float.TryParse (context.FLOAT ().GetText ().Replace ("f", ""), out val)) {
				CSLog.E (node, "failed to parse float: #" + context.FLOAT ().GetText () + "#");
			}
			node._val = val;
			return node;
		}

		public override CSNode VisitStringAtomExp (CSScriptParser.StringAtomExpContext context) {
			CSStringNode node = new CSStringNode (context.Start.Line, context.Start.Column);
			string str = context.STRING ().GetText ();
			node._val = str.Substring (1, str.Length - 2);
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

		public override CSNode VisitLocal_variable (CSScriptParser.Local_variableContext context) {
			CSVariableNode variableNode = new CSVariableNode (context.Start.Line, context.Start.Column);
			variableNode._variableName = context.NAME ().GetText ();
			if (context.VAR () != null) {
				variableNode._declare = true;
				_state.AddVariable (variableNode._variableName);
			} else {
				variableNode._declare = false;
			}
			return variableNode;
		}

		public override CSNode VisitNewExp (CSScriptParser.NewExpContext context) {
			CSOPNewNode node = new CSOPNewNode (context.Start.Line, context.Start.Column);
			node._children = new CSNode[3];
			node._children[0] = Visit (context.vartypes ());
			if (context.parameters () != null) {
				node._children[1] = Visit (context.parameters ());
			} else {
				node._children[1] = null;
			}

			if (context.array_index () != null) {
				node._children[2] = Visit (context.array_index ());
			} else {
				node._children[2] = null;
			}
			return node;
		}

		string GetTypeString (CSScriptParser.VartypeContext[] vartypes, int varCount) {
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			System.Text.StringBuilder sbTemplate = new System.Text.StringBuilder ();
			sbTemplate.Append ('[');

			bool isThereTemplate = false;

			for (int i = 0; i < varCount; ++i) {
				CSScriptParser.VartypeContext next = vartypes[i];
				if (i != 0) {
					sb.Append ('.');
				}

				sb.Append (ReflectionUtil.GetCleanNameIfPrimitive (next.NAME ().GetText ()));

				CSScriptParser.Generic_parametersContext genericParameters = next.generic_parameters ();
				if (genericParameters != null) {
					CSScriptParser.VartypesContext[] templatetypes = genericParameters.vartypes ();
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
			return typeString;
		}

		public override CSNode VisitVartypes (CSScriptParser.VartypesContext context) {
			CSScriptParser.VartypeContext[] vartypes = context.vartype ();
			int varLen = vartypes.Length;

			bool isVariable = false;
			List<string> selector = new List<string> ();
			string currentTypeString = null;
			System.Type currentType = null;
			int typeCount = varLen;

			for (int i = 0; i < varLen; ++i) {
				CSScriptParser.VartypeContext next = vartypes[i];
				string name = next.NAME ().GetText ();
				if (i == 0) {
					if (_state.HasVariable (name)) {
						isVariable = true;
						typeCount = i;
					}
				}

				if (currentType != null) {
					if (ReflectionUtil.HasField (currentType, name)) {
						isVariable = true;
						typeCount = i;
					}
				}

				if (isVariable) {
					selector.Add (name);
				} else {
					currentTypeString = GetTypeString (vartypes, varLen);
					currentType = ReflectionUtil.GetType (currentTypeString);
				}
			}

			//string typeString = GetTypeString (vartypes, varLen);
			//System.Type type = ReflectionUtil.GetType (typeString);
			CSTypeNode node = new CSTypeNode (context.Start.Line, context.Start.Column);
			node._type = currentType;
			node._arrayType = ReflectionUtil.GetType (currentTypeString + "[]");
			node._typeString = currentTypeString;
			node._assemblyName = currentType.Assembly.GetCleanName ();
			//CSLog.D ("full name: " + node._typeString + " in the assembly: " + node._assemblyName);

			return node;
		}
		public override CSNode VisitArray_index (CSScriptParser.Array_indexContext context) {
			CSArrayIndexNode node = new CSArrayIndexNode (context.Start.Line, context.Start.Column);

			if (context.expression () != null) {
				node._children = new CSNode[1];
				node._children[0] = Visit (context.expression ());
			} else {
				node._children = null;
			}

			return node;
		}

	}
}