using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CSScript {

	public class ReflectionUtil {
		#region StaticPublicAPI

		public static void Initialize () {
			if (_inst != null) {
				CSLog.E ("ReflectionUtil has already been initialized...");
				return;
			}
			_inst = new ReflectionUtil ();
		}

		public static void Destroy () {
			_inst = null;
		}

		public static Type GetType (string typeName, string assemblyName = null) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			return _inst._GetType (typeName, assemblyName);
		}

		public static Type GetGenericType (string typeName, int argCount, string assemblyName = null) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			typeName = typeName + "`" + argCount;
			return _inst._GetType (typeName, assemblyName);
		}

		public static Type GetGenericType (string typeName, string assemblyName, params Type[] args) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}

			if (args == null || args.Length == 0) {
				return _inst._GetType (typeName, assemblyName);
			}

			int argLen = args.Length;

			StringBuilder sb = new StringBuilder ();
			sb.Append (typeName);
			sb.Append ("`");
			sb.Append (argLen);
			sb.Append ("[");

			for (int i = 0; i < argLen; ++i) {
				if (i != 0) {
					sb.Append(", ");
				}
				sb.Append(args[i].FullName);
			}
			sb.Append ("]");
			
			return _inst._GetType (sb.ToString(), assemblyName);
		}

		#endregion

		public class AsmInfo {
			public Assembly _asm;
			public string _name;
		}

		static ReflectionUtil _inst;
		Assembly[] _assemblies;
		Dictionary<string, AsmInfo> _assemblyLookup = new Dictionary<string, AsmInfo> ();
		Dictionary<string, Type> _shortTypeNames = new Dictionary<string, Type> () { //
			{ "char", typeof (char) }, //
			{ "byte", typeof (byte) }, //
			{ "short", typeof (short) }, //
			{ "ushort", typeof (ushort) }, //
			{ "int", typeof (int) }, //
			{ "uint", typeof (uint) }, // 
			{ "long", typeof (long) }, // 
			{ "ulong", typeof (ulong) }, // 
			{ "float", typeof (float) }, // 
			{ "dobule", typeof (double) }, // 
			{ "string", typeof (string) }, //
		};

		AsmInfo _unityAsm;
		AsmInfo _systemAsm;

		ReflectionUtil () {
			InitializeAssemblyLookup ();
		}

		public void InitializeAssemblyLookup () {
			AppDomain currentDomain = AppDomain.CurrentDomain;
			_assemblies = currentDomain.GetAssemblies ();

			int len = _assemblies.Length;
			for (int i = 0; i < len; ++i) {
				Assembly asm = _assemblies[i];
				string name = asm.FullName;
				if (name.IndexOf (',') >= 0) {
					name = name.Split (',') [0];
				}

				AsmInfo info = new AsmInfo () {
					_name = name,
						_asm = asm,
				};

				if (name == "System") {
					_systemAsm = info;
				} else if (name == "UnityEngine") {
					_unityAsm = info;
				}
				//CSLog.D ("adding: " + name);
				_assemblyLookup[name] = info;
			}
		}

		AsmInfo GetAsmInfo (string namespaceName) {
			if (string.IsNullOrEmpty (namespaceName)) {
				return null;
			}

			AsmInfo info;
			if (_assemblyLookup.TryGetValue (namespaceName, out info)) {
				return info;
			}

			string name = namespaceName;
			int pos = name.LastIndexOf ('.', name.Length - 1);
			while (pos > 0) {
				name = name.Substring (0, pos);

				if (_assemblyLookup.TryGetValue (name, out info)) {
					return info;

				}
				pos = name.LastIndexOf ('.', name.Length - 1);
			}

			CSLog.E ("failed to load assmbly: " + namespaceName);
			return null;
		}

		Type _GetType (string typeName, string namespaceName) {
			Type type = null;
			AsmInfo info = GetAsmInfo (namespaceName);

			type = _GetTypeWithASM (typeName, info, namespaceName);
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

			type = Type.GetType (typeName);

			if (type == null && info != null) {
				type = info._asm.GetType (typeName);

				if (type == null) {
					string fullName = namespaceName + "." + typeName;
					//CSLog.D ("full name:" + fullName + ":");
					type = Type.GetType (fullName);
					if (type == null) {
						type = info._asm.GetType (fullName);
					}
				}
			}
			return type;
		}
	}
}