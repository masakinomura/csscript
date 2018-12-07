using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public enum ObjectType {
		VARIABLE,
		IMMEDIATE,
		STATIC,
		TEMP,
		LOCAL,
		ARRAY,
		DICTIONARY,
	}

	public class CSObject {
		CSNode _node;
		object _object;

		string _name = "unknown";

		System.Type _type;

		System.Type _staticType;

		ObjectType _objectType;

		int _arrayIndex;

		public ObjectType ObjectType { get { return _objectType; } }

		public string Name { get { return _name; } }

		public System.Type Type { get { return _type; } }

		private CSObject () { }

		public bool CanCast<T> () {
			return ReflectionUtil.CanCast (typeof (T), Value);
		}

		public bool CanCast (System.Type type) {
			return ReflectionUtil.CanCast (type, Value);
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

		public static CSObject LocalVariableObject (CSNode node, System.Type type, string name, object val) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = val,
					_type = type,
					_objectType = ObjectType.LOCAL,
					_name = name,
					_staticType = null,
					_arrayIndex = -1,
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
					_staticType = null,
					_arrayIndex = -1,
			};
			return obj;
		}

		public static CSObject VariableObject (CSNode node, System.Type type, object parent, string name) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = parent,
					_type = type,
					_objectType = ObjectType.VARIABLE,
					_name = name,
					_staticType = null,
					_arrayIndex = -1,
			};

			return obj;
		}

		public static CSObject TempVariableObject (CSNode node, System.Type type, object val) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = val,
					_type = type,
					_objectType = ObjectType.TEMP,
					_name = "temp",
					_staticType = null,
					_arrayIndex = -1,
			};

			return obj;
		}

		public static CSObject StaticVariableObject (CSNode node, System.Type type, System.Type staticType, string name) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = null,
					_type = type,
					_objectType = ObjectType.STATIC,
					_name = name,
					_staticType = staticType,
					_arrayIndex = -1,
			};

			return obj;
		}

		public static CSObject ArrayVariableObject (CSNode node, System.Type type, object array, int index) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = array,
					_type = type,
					_objectType = ObjectType.ARRAY,
					_name = "array",
					_staticType = null,
					_arrayIndex = index,
			};

			return obj;
		}

		public static CSObject DictionaryVariableObject (CSNode node, System.Type type, object dictionary, string name) {
			CSObject obj = new CSObject () {
				_node = node,
					_object = dictionary,
					_type = type,
					_objectType = ObjectType.DICTIONARY,
					_name = name,
					_staticType = null,
					_arrayIndex = -1,
			};

			return obj;
		}

		public object Value {
			get {
				switch (ObjectType) {
					case ObjectType.VARIABLE:
						return ReflectionUtil.Get (_object, _name);
					case ObjectType.STATIC:
						return ReflectionUtil.Get (_staticType, _name);
					case ObjectType.ARRAY:
						return ((IList) _object) [_arrayIndex];
					case ObjectType.DICTIONARY:
						return ((IDictionary) _object) [_name];
					case ObjectType.TEMP:
					case ObjectType.LOCAL:
					case ObjectType.IMMEDIATE:
						return _object;
					default:
						CSLog.E (_node, "cannot assign to " + _objectType.ToString ());
						return null;
				}
			}
		}

		public CSObject Assign (CSObject obj) {

			switch (ObjectType) {
				case ObjectType.VARIABLE:
					ReflectionUtil.Set (_object, _name, obj.GetAs (_type));
					break;
				case ObjectType.STATIC:
					ReflectionUtil.Set (_staticType, _name, obj.GetAs (_type));
					break;
				case ObjectType.ARRAY:
					IList array = (IList) _object;
					array[_arrayIndex] = obj.GetAs (_type);
					break;
				case ObjectType.DICTIONARY:
					IDictionary dictionary = (IDictionary) _object;
					dictionary[_name] = obj.GetAs (_type);
					break;					
				case ObjectType.TEMP:
				case ObjectType.LOCAL:
					if (_type == null) {
						_object = obj.Value;
						_type = obj.Type;
					} else {
						_object = obj.GetAs (_type);
					}
					break;
				default:
					CSLog.E (_node, "cannot assign to " + _objectType.ToString ());
					break;
			}

			return this;
		}

	}
}