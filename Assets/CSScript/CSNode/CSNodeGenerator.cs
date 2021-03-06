﻿using System.Collections;
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
			CSLocalVariableNode variableNode = new CSLocalVariableNode (context.Start.Line, context.Start.Column);
			variableNode._declaration = true;
			variableNode._variableName = context.NAME ().GetText ();

			CSScriptParser.TypeContext vartypes = context.type ();
			if (vartypes != null) {
				CSTypeNode typeNode = Visit (vartypes) as CSTypeNode;
				if (typeNode == null) {
					CSLog.E (variableNode, "failed to get the type");
				}
				variableNode._type = typeNode._type;
			}

			CSObject objForComplier = CSObject.LocalVariableObject (variableNode, variableNode._type, variableNode._variableName, null);
			_state.AddVariable (variableNode._variableName, objForComplier);

			return variableNode;
		}

		public override CSNode VisitNewExp (CSScriptParser.NewExpContext context) {
			CSOPNewNode node = new CSOPNewNode (context.Start.Line, context.Start.Column);
			node._children = new CSNode[6];

			CSScriptParser.ParametersContext parameters = context.parameters ();
			CSScriptParser.Array_indexContext arrayIndex = context.array_index ();

			System.Type currentType;
			string currentTypeString = VisitTypeElements (context.type_elements (), out currentType);
			if (arrayIndex != null) {
				currentTypeString = currentTypeString + "[]";
				currentType = ReflectionUtil.GetType (currentTypeString);
			}

			if (currentType == null) {
				CSLog.E (node, "unknown type: " + currentTypeString);
				return null;
			}

			CSTypeNode typeNode = new CSTypeNode (context.Start.Line, context.Start.Column);
			typeNode._type = currentType;
			typeNode._typeString = currentTypeString;
			typeNode._assemblyName = currentType.Assembly.GetCleanName ();

			node._children[0] = typeNode;

			if (parameters != null) {
				node._children[1] = Visit (parameters);
			}

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

		string GetTypeString (
			CSScriptParser.Type_elementContext[] vartypes,
			int varCount,
			int typeStart) {

			return _GetTypeString (vartypes, varCount, typeStart,
				(vartype) => { return ((CSScriptParser.Type_elementContext) vartype).NAME ().GetText (); },
				(vartype) => { return ((CSScriptParser.Type_elementContext) vartype).generic_parameters (); }
			);
		}

		string GetTypeString (
			CSScriptParser.SelectorContext[] vartypes,
			int varCount,
			int typeStart) {

			return _GetTypeString (vartypes, varCount, typeStart,
				(vartype) => { return ((CSScriptParser.SelectorContext) vartype).NAME ().GetText (); },
				(vartype) => { return ((CSScriptParser.SelectorContext) vartype).generic_parameters (); }
			);
		}

		string _GetTypeString (
			object[] vartypes,
			int varCount,
			int typeStart,
			System.Func<object, string> getName,
			System.Func<object, CSScriptParser.Generic_parametersContext> getGenericParams) {

			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			System.Text.StringBuilder sbTemplate = new System.Text.StringBuilder ();

			sbTemplate.Append ('[');

			bool isThereTemplate = false;

			for (int i = 0; i < varCount; ++i) {
				object next = vartypes[i];
				if (i != 0) {
					if (typeStart >= 0 && typeStart < i) {
						sb.Append ('+');
					} else {
						sb.Append ('.');
					}
				}

				string name = getName (next);

				sb.Append (ReflectionUtil.GetCleanNameIfPrimitive (name));

				CSScriptParser.Generic_parametersContext genericParameters = getGenericParams (next);
				if (genericParameters != null) {
					CSScriptParser.TypeContext[] genericTypes = genericParameters.type ();
					int tempVarCount = genericTypes.Length;
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
							CSTypeNode child = VisitType (genericTypes[j]) as CSTypeNode;
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

		string VisitTypeElements (CSScriptParser.Type_elementsContext context, out System.Type type) {
			CSScriptParser.Type_elementContext[] vartypes = context.type_element ();
			int varLen = vartypes.Length;

			string currentTypeString = null;
			System.Type currentType = null;
			int typeStart = -1;

			for (int i = 0; i < varLen; ++i) {
				CSScriptParser.Type_elementContext next = vartypes[i];
				string name = next.NAME ().GetText ();
				currentTypeString = GetTypeString (vartypes, i + 1, typeStart);
				currentType = ReflectionUtil.GetType (currentTypeString);
				if (typeStart == -1 && currentType != null) {
					typeStart = i;
				}
			}

			type = currentType;
			return currentTypeString;
		}

		public override CSNode VisitType (CSScriptParser.TypeContext context) {
			System.Type currentType;
			string currentTypeString = VisitTypeElements (context.type_elements (), out currentType);
			if (context.arraytype () != null) {
				currentType = ReflectionUtil.GetType (currentTypeString + "[]");
			}

			if (currentType == null) {
				CSLog.E (context.Start.Line, context.Start.Column, "unknown type: " + currentTypeString);
				return null;
			}

			CSTypeNode node = new CSTypeNode (context.Start.Line, context.Start.Column);
			node._type = currentType;
			node._typeString = currentType.AssemblyQualifiedName;
			node._assemblyName = currentType.Assembly.GetCleanName ();
			//CSLog.D ("full name: " + node._typeString + " in the assembly: " + node._assemblyName);

			return node;
		}

		string[] GetSelectorStrings (CSScriptParser.SelectorContext[] selectors, int start) {
			List<string> strings = new List<string> ();
			int len = selectors.Length;
			for (int i = start; i < len; ++i) {
				strings.Add (selectors[i].NAME ().GetText ());
			}
			return strings.ToArray ();
		}

		CSNode VisitSelectors (CSScriptParser.SelectorContext[] selectors, int line, int col) {
			int selectorLen = selectors.Length;

			string firstName = selectors[0].NAME ().GetText ();
			if (_state.HasVariable (firstName)) {
				CSLocalVariableNode node = new CSLocalVariableNode (line, col);
				node._declaration = false;
				node._variableName = firstName;
				if (selectorLen == 1) {
					return node;
				} else {
					CSSelectorNode selectorNode = new CSSelectorNode (line, col);
					selectorNode._selectors = GetSelectorStrings (selectors, 1);
					CSOPDotNode dotNode = new CSOPDotNode (line, col);
					dotNode._children = new CSNode[2];
					dotNode._children[0] = node;
					dotNode._children[1] = selectorNode;
					return dotNode;
				}
			}

			string currentTypeString = null;
			System.Type currentType = null;
			int typeStart = -1;
			int typeEnd = 0;

			for (int i = 0; i < selectorLen; ++i) {
				CSScriptParser.SelectorContext next = selectors[i];
				string name = next.NAME ().GetText ();
				currentTypeString = GetTypeString (selectors, i + 1, typeStart);
				System.Type nextType = ReflectionUtil.GetType (currentTypeString);

				if (typeStart == -1 && nextType != null) {
					typeStart = i;
				}

				if (currentType != null && nextType == null) {
					typeEnd = i;
					break;
				}
				currentType = nextType;
			}

			if (currentType != null) {
				if (typeEnd == 0) {
					CSTypeNode node = new CSTypeNode (line, col);
					node._typeString = currentTypeString;
					node._type = currentType;
					node._assemblyName = currentType.Assembly.GetCleanName ();
					return node;
				} else {
					CSStaticVariableNode node = new CSStaticVariableNode (line, col);
					string varName = selectors[typeEnd].NAME ().GetText ();
					node._variableName = varName;
					node._staticType = currentType;
					node._type = ReflectionUtil.GetFieldType (currentType, varName);

					if (node._type == null) {
						CSLog.E (node, "type: " + currentType.FullName + " doesn't have: " + varName);
					}

					if (selectorLen == typeEnd + 1) {
						return node;
					} else {
						CSSelectorNode selectorNode = new CSSelectorNode (line, col);
						selectorNode._selectors = GetSelectorStrings (selectors, typeEnd + 1);
						CSOPDotNode dotNode = new CSOPDotNode (line, col);
						dotNode._children = new CSNode[2];
						dotNode._children[0] = node;
						dotNode._children[1] = selectorNode;
						return dotNode;
					}
				}
			} else {
				CSSelectorNode node = new CSSelectorNode (line, col);
				node._selectors = GetSelectorStrings (selectors, 0);
				return node;
			}
		}

		public override CSNode VisitSelectorExp (CSScriptParser.SelectorExpContext context) {
			return VisitSelectors (context.selector (), context.Start.Line, context.Start.Column);
		}

		public override CSNode VisitDotExp (CSScriptParser.DotExpContext context) {
			CSOPDotNode node = new CSOPDotNode (context.Start.Line, context.Start.Column);
			CSScriptParser.ExpressionContext[] expressions = context.expression ();
			if (expressions == null || expressions.Length != 2) {
				CSLog.E (context.Start.Line, context.Start.Column, "invalid # of children...");
				return null;
			}
			node._children = new CSNode[2];
			node._children[0] = Visit (expressions[0]);
			node._children[1] = Visit (expressions[1]);

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

		public override CSNode VisitArrayIndexExp (CSScriptParser.ArrayIndexExpContext context) {
			CSOPArrayIndexNode node = new CSOPArrayIndexNode (context.Start.Line, context.Start.Column);

			node._children = new CSNode[2];
			node._children[0] = Visit (context.expression ());
			node._children[1] = Visit (context.array_index ());

			return node;
		}

		CSTypeNode[] GetGenericParameters (CSScriptParser.Generic_parametersContext context) {

			CSScriptParser.TypeContext[] genericTypes = context.type ();
			int tempVarCount = genericTypes.Length;
			CSTypeNode[] types = new CSTypeNode[tempVarCount];

			for (int i = 0; i < tempVarCount; ++i) {
				CSTypeNode child = VisitType (genericTypes[i]) as CSTypeNode;
				types[i] = child;
			}

			return types;
		}

		public override CSNode VisitFuncExp (CSScriptParser.FuncExpContext context) {
			CSFunctionNode node = new CSFunctionNode (context.Start.Line, context.Start.Column);
			node._functionName = context.NAME ().GetText ();
			node._children = new CSNode[2];
			node._children[0] = VisitSelectors (context.selector (), context.Start.Line, context.Start.Column);
			node._children[1] = Visit (context.parameters ());

			CSScriptParser.Generic_parametersContext generic = context.generic_parameters ();
			if (generic != null) {
				node._genericParameters = GetGenericParameters (generic);
			}
			return node;
		}
	}
}