using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {

	public class CSNodeGenerator : CSScriptBaseVisitor<CSNode> {
		CSState _state = new CSState ();

		void VisitExpressions (CSNode node, CSScriptParser.ExpressionContext[] expressions) {
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
			VisitExpressions (node, context.expression ());
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
			VisitExpressions (node, context.expression ());
			return node;
		}

		public override CSNode VisitParameters (CSScriptParser.ParametersContext context) {
			CSNode node = new CSNode (context.Start.Line, context.Start.Column);
			VisitExpressions (node, context.expression ());
			return node;
		}

		public override CSNode VisitVarDeclExp (CSScriptParser.VarDeclExpContext context) {
			CSVariableNode variableNode = new CSVariableNode (context.Start.Line, context.Start.Column);
			variableNode._variableName = context.NAME ().GetText ();
			_state.AddVariable (variableNode._variableName);

			CSScriptParser.VartypesContext vartypes = context.vartypes ();
			if (vartypes != null) {
				CSTypeNode typeNode = Visit (vartypes) as CSTypeNode;
				if (typeNode == null) {
					CSLog.E (variableNode, "failed to get the type");
				}
				variableNode._type = typeNode._type;
			}
			return variableNode;
		}

		public override CSNode VisitNewExp (CSScriptParser.NewExpContext context) {
			CSOPNewNode node = new CSOPNewNode (context.Start.Line, context.Start.Column);
			node._children = new CSNode[6];
			node._children[0] = Visit (context.vartypes ());

			CSScriptParser.ParametersContext parameters = context.parameters ();
			if (parameters != null) {
				node._children[1] = Visit (parameters);
			}

			CSScriptParser.Array_indexContext arrayIndex = context.array_index ();
			if (arrayIndex != null) {
				node._children[2] = Visit (arrayIndex);
			}

			CSScriptParser.InitializerContext initializer = context.initializer ();
			if (initializer != null) {
				CSScriptParser.Array_initializerContext arrayInitializer = initializer.array_initializer ();
				if (arrayInitializer != null) {
					node._children[3] = Visit (arrayInitializer);
				}

				CSScriptParser.Dictionary_initializerContext dictionaryInitializer = initializer.dictionary_initializer ();
				if (dictionaryInitializer != null) {
					node._children[4] = Visit (dictionaryInitializer);
				}

				CSScriptParser.Class_initializerContext classInitializer = initializer.class_initializer ();
				if (classInitializer != null) {
					node._children[5] = Visit (classInitializer);
				}
			}
			return node;
		}

		public override CSNode VisitArray_initializer (CSScriptParser.Array_initializerContext context) {
			CSArrayInitializerNode node = new CSArrayInitializerNode (context.Start.Line, context.Start.Column);
			VisitExpressions (node, context.expression ());
			return node;
		}

		public override CSNode VisitDictionary_initializer (CSScriptParser.Dictionary_initializerContext context) {
			CSDictionaryInitializerNode node = new CSDictionaryInitializerNode (context.Start.Line, context.Start.Column);
			CSScriptParser.Dictionary_initializer_elementContext[] elements = context.dictionary_initializer_element ();
			if (elements != null) {
				int len = elements.Length;
				node._keys = new CSNode[len];
				node._children = new CSNode[len];

				for (int i = 0; i < len; ++i) {
					CSScriptParser.ExpressionContext[] expressions = elements[i].expression ();
					node._keys[i] = Visit (expressions[0]);
					node._children[i] = Visit (expressions[1]);
				}
			}
			return node;
		}

		public override CSNode VisitClass_initializer (CSScriptParser.Class_initializerContext context) {
			CSClassInitializerNode node = new CSClassInitializerNode (context.Start.Line, context.Start.Column);
			CSScriptParser.Class_initializer_elementContext[] elements = context.class_initializer_element ();

			if (elements != null) {
				int len = elements.Length;
				node._variableNames = new string[len];
				node._children = new CSNode[len];
				for (int i = 0; i < len; ++i) {
					CSScriptParser.Class_initializer_elementContext element = elements[i];
					node._variableNames[i] = element.NAME ().GetText ();
					node._children[i] = Visit (element.expression ());
				}
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
							if (j != 0) {
								sbTemplate.Append ("], [");
							}
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
			node._typeString = currentType.AssemblyQualifiedName;
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