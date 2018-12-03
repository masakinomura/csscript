using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CSScript {

	public partial class ReflectionUtil {
		Dictionary<KeyValuePair<string, string>, Type> _typeCache = new Dictionary<KeyValuePair<string, string>, Type> ();

		Type _GetType (string typeName, string namespaceName) {
			Type type = null;

			KeyValuePair<string, string> key = new KeyValuePair<string, string> (namespaceName, typeName);
			if (_inst._typeCache.TryGetValue (key, out type)) {
				return type;
			}

			AsmInfo info;
			if (!string.IsNullOrEmpty (namespaceName)) {
				info = GetAsmInfo (namespaceName);
			} else {
				info = GetAsmInfo (typeName);
			}

			type = _GetTypeWithASM (typeName, info, namespaceName);
			if (type == null) {
				CSLog.D ("type: " + typeName + " cannot be found");
			}

			_typeCache[key] = type;

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