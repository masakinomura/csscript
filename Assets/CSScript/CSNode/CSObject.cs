using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public enum ObjectType {
		VARIABLE,
		IMMEDIATE,
		TYPE,
	}

	public class CSObject {

		CSNode _node;

		object _object;

		CSScope _scope;
		string _nameInScope;

		System.Type _type;

		ObjectType _objectType;

		public ObjectType ObjectType { get { return _objectType; } }

		public string Name {
			get {
				switch (_objectType) {
					case ObjectType.VARIABLE:
						return _nameInScope;
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
					case ObjectType.VARIABLE:
						return _scope.GetVariable (_nameInScope);
					case ObjectType.TYPE:
						return _type;
					default:
						return _object;
				}
			}

			set {
				switch (_objectType) {
					case ObjectType.VARIABLE:
						_scope.SetVariable (_nameInScope, value);
						break;
					case ObjectType.TYPE:
						CSLog.E (_node, "you cannot assign value to type");
						break;
					case ObjectType.IMMEDIATE:
						CSLog.E (_node, "you cannot assign value to immediate");
						break;
					default:
						_object = value;
						break;
				}
			}
		}

		public CSObject (CSNode node, CSScope scope, string name) {
			_node = node;
			_object = null;
			_scope = scope;
			_nameInScope = name;
			_objectType = ObjectType.VARIABLE;
		}

		public CSObject (CSNode node, object val) {
			_node = node;
			_object = val;
			_scope = null;
			_nameInScope = null;
			_objectType = ObjectType.IMMEDIATE;
		}

		public CSObject (CSNode node, System.Type type) {
			_node = node;
			_object = null;
			_scope = null;
			_type = type;
			_nameInScope = null;
			_objectType = ObjectType.TYPE;
		}

	}
}