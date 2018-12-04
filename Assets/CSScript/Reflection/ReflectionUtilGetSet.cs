using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CSScript {

	public partial class ReflectionUtil {

		public bool _HasField (object target, string name, BindingFlags bindingFlags) {

			System.Type type;
			bool isStatic;
			if (target is System.Type) {
				type = target as System.Type;
				isStatic = true;
				bindingFlags |= BindingFlags.Static;
				bindingFlags &= ~BindingFlags.Instance;
			} else {
				type = target.GetType ();
				isStatic = false;
				bindingFlags |= BindingFlags.Instance;
				bindingFlags &= ~BindingFlags.Static;
			}

			FieldInfo[] fields = type.GetFields (bindingFlags);
			int len = fields.Length;
			for (int i = 0; i < len; ++i) {
				FieldInfo field = fields[i];
				if (field.Name == name) {
					return true;
				}
			}

			PropertyInfo[] properties = type.GetProperties (bindingFlags);
			len = properties.Length;
			for (int i = 0; i < len; ++i) {
				PropertyInfo property = properties[i];

				if (property.Name == name && property.CanRead) {
					return true;
				}
			}

			return false;
		}

		public Type _GetFieldType (object target, string name, BindingFlags bindingFlags) {
			System.Type type;
			bool isStatic;
			if (target is System.Type) {
				type = target as System.Type;
				isStatic = true;
				bindingFlags |= BindingFlags.Static;
				bindingFlags &= ~BindingFlags.Instance;
			} else {
				type = target.GetType ();
				isStatic = false;
				bindingFlags |= BindingFlags.Instance;
				bindingFlags &= ~BindingFlags.Static;
			}

			FieldInfo[] fields = type.GetFields (bindingFlags);
			int len = fields.Length;
			for (int i = 0; i < len; ++i) {
				FieldInfo field = fields[i];

				if (field.Name == name) {
					return field.FieldType;
				}
			}

			PropertyInfo[] properties = type.GetProperties (bindingFlags);
			len = properties.Length;
			for (int i = 0; i < len; ++i) {
				PropertyInfo property = properties[i];

				if (property.Name == name) {
					return property.PropertyType;
				}
			}
			throw new System.ArgumentException ("Getter Member: " + name + " cannot be found in " + type.ToString ());
		}

		public object _Get (object target, string name, BindingFlags bindingFlags) {
			System.Type type;
			bool isStatic;
			if (target is System.Type) {
				type = target as System.Type;
				isStatic = true;
				bindingFlags |= BindingFlags.Static;
				bindingFlags &= ~BindingFlags.Instance;
			} else {
				type = target.GetType ();
				isStatic = false;
				bindingFlags |= BindingFlags.Instance;
				bindingFlags &= ~BindingFlags.Static;
			}

			FieldInfo[] fields = type.GetFields (bindingFlags);
			int len = fields.Length;
			for (int i = 0; i < len; ++i) {
				FieldInfo field = fields[i];

				if (field.Name == name) {
					if (isStatic) {
						return field.GetValue (null);
					} else {
						return field.GetValue (target);
					}
				}
			}

			PropertyInfo[] properties = type.GetProperties (bindingFlags);
			len = properties.Length;
			for (int i = 0; i < len; ++i) {
				PropertyInfo property = properties[i];

				if (property.Name == name) {
					if (!property.CanRead) {
						throw new System.InvalidOperationException ("cannot read from a write-only property: " + name);
					}
					if (isStatic) {
						return property.GetValue (null);
					} else {
						return property.GetValue (target);
					}
				}
			}
			throw new System.ArgumentException ("Getter Member: " + name + " cannot be found in " + type.ToString ());
		}

		public void _Set (object target, string name, BindingFlags bindingFlags, object val) {
			System.Type type;
			bool isStatic;
			if (target is System.Type) {
				type = target as System.Type;
				isStatic = true;
				bindingFlags |= BindingFlags.Static;
				bindingFlags &= ~BindingFlags.Instance;
			} else {
				type = target.GetType ();
				isStatic = false;
				bindingFlags |= BindingFlags.Instance;
				bindingFlags &= ~BindingFlags.Static;
			}

			FieldInfo[] fields = type.GetFields (bindingFlags);
			int len = fields.Length;
			for (int i = 0; i < len; ++i) {
				FieldInfo field = fields[i];

				if (field.Name == name) {
					object castedVal = val;
					if (castedVal is NullWithType) {
						castedVal = _Cast (field.FieldType, null);
					} else if (castedVal == null || field.FieldType != castedVal.GetType ()) {
						castedVal = _Cast (field.FieldType, castedVal);
					}
					if (isStatic) {
						field.SetValue (null, castedVal);
					} else {
						field.SetValue (target, castedVal);
					}
					return;
				}
			}

			PropertyInfo[] properties = type.GetProperties (bindingFlags);
			len = properties.Length;
			for (int i = 0; i < len; ++i) {
				PropertyInfo property = properties[i];

				if (property.Name == name) {
					if (!property.CanWrite) {
						throw new System.InvalidOperationException ("cannot write into a read-only property: " + name);
					}
					object castedVal = val;
					if (castedVal is NullWithType) {
						castedVal = _Cast (property.PropertyType, null);
					} else if (castedVal == null || property.PropertyType != castedVal.GetType ()) {
						castedVal = _Cast (property.PropertyType, castedVal);
					}
					if (isStatic) {
						property.SetValue (null, castedVal);
					} else {
						property.SetValue (target, castedVal);
					}
					return;
				}
			}
			throw new System.ArgumentException ("Setter Member: " + name + " cannot be found in " + type.ToString ());
		}

	}
}