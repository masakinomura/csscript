using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public enum ObjectType {
		VARIABLE,
		IMMEDIATE,
	}

	public class CSObject {
		CSNode _node;
		object _object;

		string _name = "unknown";

		System.Type _type;

		ObjectType _objectType;

		public ObjectType ObjectType { get { return _objectType; } }

		public string Name { get { return _name; } }
		public object Value { get { return _object; } }
		public System.Type Type { get { return _type; } }

		private CSObject () { }

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

		public static CSObject LocalVariableObject (CSNode node, System.Type type, string name, object val) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = val,
					_type = type,
					_objectType = ObjectType.VARIABLE,
					_name = name,
			};
			return obj;
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
					_objectType = ObjectType.IMMEDIATE,
					_name = "immedidate",
			};
			return obj;
		}

		public static CSObject VariableObject (CSNode node, System.Type type, object val, params string[] selectors) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = val,
					_type = type,
					_objectType = ObjectType.VARIABLE,
					_name = "temp",
			};

			if (selectors != null && selectors.Length > 0) {
				CSLog.E (node, "implement me!!!");
			}
			return obj;
		}

		public CSObject Assign (CSObject obj) {

			if (ObjectType == ObjectType.VARIABLE) {
				if (_type == null) {
					_object = obj.Value;
					_type = obj.Type;
				} else {
					_object = obj.GetAs (_type);
				}
			} else {
				CSLog.E (_node, "cannot assign to " + _objectType.ToString ());
			}
			return this;
		}

	}
}