using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CSScript {

	public partial class ReflectionUtil {
		Dictionary<KeyValuePair<string, string>, Type> _typeCache = new Dictionary<KeyValuePair<string, string>, Type> ();

		Type _GetType (string typeName, params string[] namespaceNames) {
			Type type = null;

			if (namespaceNames == null || namespaceNames.Length == 0) {
				namespaceNames = new string[] { null };
			}

			int len = namespaceNames.Length;
			for (int i = 0; i < len; ++i) {
				string namespaceName = namespaceNames[i];

				KeyValuePair<string, string> key = new KeyValuePair<string, string> (namespaceName, typeName);
				Type cachedType;
				if (_inst._typeCache.TryGetValue (key, out cachedType)) {
					if (cachedType != null) {
						if (type != null) {
							throw new AmbiguousMatchException ("type: " + typeName + " is ambigours. the type exists in " + type.ToString () + " and " + cachedType.ToString ());
						} else {
							type = cachedType;
						}
					}
				} else {
					AsmInfo info;
					if (!string.IsNullOrEmpty (namespaceName)) {
						info = GetAsmInfo (namespaceName);
					} else {
						info = GetAsmInfo (typeName);
					}

					Type nextType = _GetTypeWithASM (typeName, info, namespaceName);
					_typeCache[key] = nextType;
					if (nextType != null) {
						type = nextType;
					}
				}
			}

			if (type == null) {
				CSLog.D ("type: " + typeName + " cannot be found");
			}

			return type;
		}

		Type _GetTypeWithASM (string typeName, AsmInfo info, string namespaceName) {
			Type type;
			if (_shortTypeNames.TryGetValue (typeName, out type)) {
				return type;
			}

			string fullName = null;
			if (!string.IsNullOrEmpty (namespaceName)) {
				fullName = namespaceName + "." + typeName;
			}

			type = Type.GetType (typeName);
			if (type == null && info != null) {
				type = info._asm.GetType (typeName);
				if (type == null && fullName != null) {
					//CSLog.D ("full name:" + fullName + ":");
					type = info._asm.GetType (fullName);
					if (type == null) {
						type = Type.GetType (fullName);
					}
				}
			}

			if (type == null) {
				//still cannot find it. all search...
				int len = _assemblies.Length;
				for (int i = 0; i < len; ++i) {
					Assembly asm = _assemblies[i];
					type = asm.GetType (typeName);
					if (type != null) {
						break;
					}
					if (fullName != null) {
						type = asm.GetType (fullName);
						if (type != null) {
							break;
						}
					}
				}
			}
			return type;
		}
	}
}