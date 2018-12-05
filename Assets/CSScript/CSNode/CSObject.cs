using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public enum ObjectType {
		LOCAL_VARIABLE,
		TEMP_VARIABLE,
		IMMEDIATE,
	}

	public class CSObject {
		CSNode _node;
		object _object;

		CSScope _scope;
		string _selector;

		System.Type _type;

		ObjectType _objectType;

		public ObjectType ObjectType { get { return _objectType; } }

		public string Name {
			get {
				switch (_objectType) {
					case ObjectType.LOCAL_VARIABLE:
						return _selector;
					case ObjectType.IMMEDIATE:
						return "Immedidate";
					default:
						return "unknown";
				}
			}
		}

		public object Value {
			get {
				switch (_objectType) {
					case ObjectType.LOCAL_VARIABLE:
						return _scope.GetVariable (_selector);
					case ObjectType.IMMEDIATE:
						return _object;
					case ObjectType.TEMP_VARIABLE:
						return _object;
					default:
						return _object;
				}
			}

			// set {
			// 	switch (_objectType) {
			// 		case ObjectType.VARIABLE:
			// 			_scope.SetVariable (_nameInScope, value);
			// 			break;
			// 		case ObjectType.TYPE:
			// 			CSLog.E (_node, "you cannot assign value to type");
			// 			break;
			// 		case ObjectType.IMMEDIATE:
			// 			CSLog.E (_node, "you cannot assign value to immediate");
			// 			break;
			// 		default:
			// 			_object = value;
			// 			break;
			// 	}
			// }
		}

		public System.Type Type { get { return _type; } }

		private CSObject () { }

		public static CSObject LocalVariableObject (CSNode node, System.Type type, CSScope scope, string name) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = null,
					_scope = scope,
					_selector = name,
					_objectType = ObjectType.LOCAL_VARIABLE,
			};
			return obj;
		}

		public T GetAs<T> () {
			if (typeof (T) == typeof (object) || typeof (T) == Type) {
				return (T) Value;
			} else {
				return (T) ReflectionUtil.Cast (typeof (T), Value);
			}
		}

		public object GetAs (System.Type type) {
			if (type == typeof (object) || type == Type) {
				return Value;
			} else {
				return ReflectionUtil.Cast (type, Value);
			}
		}

		public static CSObject ImmediateObject (CSNode node, System.Type type, object val) {
			if (val == null) {
				CSLog.E (node, "val cannot be null");
				return null;
			}
			CSObject obj = new CSObject () {
				_node = node,
					_object = val,
					_type = val.GetType (),
					_scope = null,
					_selector = null,
					_objectType = ObjectType.IMMEDIATE,
			};
			return obj;
		}

		public static CSObject TempVariableObject (CSNode node, System.Type type, object val, params string[] selectors) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = val,
					_scope = null,
					_type = type,
					_selector = null,
					_objectType = ObjectType.TEMP_VARIABLE,
			};

			if (selectors != null && selectors.Length > 0) {
				CSLog.E (node, "implement me!!!");
			}
			return obj;
		}

		public static CSObject TempVariableObject (CSNode node, CSObject inObj, params string[] selectors) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = inObj.Value,
					_scope = null,
					_type = inObj.Type,
					_selector = null,
					_objectType = ObjectType.TEMP_VARIABLE,
			};

			if (selectors != null && selectors.Length > 0) {
				CSLog.E (node, "implement me!!!");
			}
			return obj;
		}

		public CSObject Assign (CSObject obj) {

			if (ObjectType == ObjectType.LOCAL_VARIABLE) {

				if (_type == null) {
					_scope.SetVariable (_selector, obj.Value);
					_type = obj.Type;
				} else {
					_scope.SetVariable (_selector, obj.GetAs (_type));
				}

			} else if (ObjectType == ObjectType.TEMP_VARIABLE) {
				if (_type == null) {
					_object = obj.Value;
					_type = obj.Type;
				} else {
					_object = obj.GetAs(_type);	
				}								
			} else {
				CSLog.E (_node, "cannot assign to " + _objectType.ToString ());
			}
			return this;
		}

	}
}