using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CSScript {
	public class CSObject {
		public enum ObjectType {
			VARIABLE,
			IMMEDIATE,
		}

		object _object;

		CSScope _scope;
		string _nameInScope;

		ObjectType _objectType;

		public object Value {
			get {
				switch (_objectType) {
					case ObjectType.VARIABLE:
						return _scope.GetVariable (_nameInScope);
					default:
						return _object;
				}
			}

			set {
				switch (_objectType) {
					case ObjectType.VARIABLE:
						_scope.SetVariable (_nameInScope, value);
						break;
					default:
						_object = value;
						break;
				}
			}
		}

		public CSObject (CSScope scope, string name) {
			_object = null;
			_scope = scope;
			_nameInScope = name;
			_objectType = ObjectType.VARIABLE;
		}

		public CSObject (object val) {
			_object = val;
			_scope = null;
			_nameInScope = null;
			_objectType = ObjectType.IMMEDIATE;
		}

	}
}