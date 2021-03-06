﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CSScript {

	public partial class ReflectionUtil {
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

		public static string GetCleanNameIfPrimitive (string typeName) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			return _inst._GetCleanNameIfPrimitive (typeName);
		}

		public static bool CanCast (Type type, object o) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return false;
			}
			return _inst._CanCast (type, o);
		}

		public static Type GetType (string typeName, params string[] namespaceNames) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			return _inst._GetType (typeName, namespaceNames);
		}

		public static object Cast (Type type, object o) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return false;
			}
			return _inst._Cast (type, o);
		}

		public static object Get (object target, string name) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			return _inst._Get (target, name, BindingFlags.Public | BindingFlags.NonPublic);
		}

		public static object Get (object target, string name, BindingFlags bindingFlags) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			return _inst._Get (target, name, bindingFlags);
		}

		public static void Set (object target, string name, object value) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
			}
			_inst._Set (target, name, BindingFlags.Public | BindingFlags.NonPublic, value);
		}

		public static void Set (object target, string name, BindingFlags bindingFlags, object value) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
			}
			_inst._Set (target, name, bindingFlags, value);
		}

		public static Type GetFieldType (object target, string name) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			return _inst._GetFieldType (target, name, BindingFlags.Public | BindingFlags.NonPublic);
		}

		public static bool HasField (object target, string name) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return false;
			}
			return _inst._HasField (target, name, BindingFlags.Public | BindingFlags.NonPublic);
		}

		public static bool HasField (object target, string name, BindingFlags bindingFlags, object value) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return false;
			}
			return _inst._HasField (target, name, bindingFlags);
		}

		public static object CallMethod (object target, string methodName, params object[] args) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			Type retType;
			return _inst._CallMethod (target, methodName, BindingFlags.Public | BindingFlags.NonPublic, null, out retType, args);
		}

		public static object CallMethod (object target, string methodName, BindingFlags bindingFlags, params object[] args) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			Type retType;
			return _inst._CallMethod (target, methodName, bindingFlags, null, out retType, args);
		}

		public static object CallMethod (object target, string methodName, Type[] genericTypes, params object[] args) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			Type retType;
			return _inst._CallMethod (target, methodName, BindingFlags.Public | BindingFlags.NonPublic, genericTypes, out retType, args);
		}

		public static object CallMethod (object target, string methodName, BindingFlags bindingFlags, Type[] genericTypes, params object[] args) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				return null;
			}
			Type retType;
			return _inst._CallMethod (target, methodName, bindingFlags, genericTypes, out retType, args);
		}

		public static object CallMethod (object target, string methodName, Type[] genericTypes, out System.Type retType, params object[] args) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				retType = null;
				return null;
			}
			return _inst._CallMethod (target, methodName, BindingFlags.Public | BindingFlags.NonPublic, genericTypes, out retType, args);
		}

		public static object CallMethod (object target, string methodName, BindingFlags bindingFlags, Type[] genericTypes, out System.Type retType, params object[] args) {
			if (_inst == null) {
				CSLog.E ("ReflectionUtil has not been initialized...");
				retType = null;
				return null;
			}
			return _inst._CallMethod (target, methodName, bindingFlags, genericTypes, out retType, args);
		}

		public static Type GetIListElementType (Type type) {
			System.Type elementType = type.GetElementType ();
			if (elementType == null && type.IsGenericType) {
				elementType = type.GenericTypeArguments[0];
			}
			return elementType;
		}

		public static Type GetIDictionaryElementType (Type type) {
			if (type.IsGenericType && type.GenericTypeArguments.Length >= 2) {
				return type.GenericTypeArguments[1];
			}
			return typeof (object);
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
			{ "decimal", typeof (decimal) }, // 
			{ "string", typeof (string) }, //
			{ "object", typeof (object) }, //

			{ "char[]", typeof (char[]) }, //
			{ "byte[]", typeof (byte[]) }, //
			{ "short[]", typeof (short[]) }, //
			{ "ushort[]", typeof (ushort[]) }, //
			{ "int[]", typeof (int[]) }, //
			{ "uint[]", typeof (uint[]) }, // 
			{ "long[]", typeof (long[]) }, // 
			{ "ulong[]", typeof (ulong[]) }, // 
			{ "float[]", typeof (float[]) }, // 
			{ "dobule[]", typeof (double[]) }, // 
			{ "decimal[]", typeof (decimal[]) }, // 
			{ "string[]", typeof (string[]) }, //
			{ "object[]", typeof (object[]) }, //			
		};

		ReflectionUtil () {
			InitializeAssemblyLookup ();
			InitializeCast ();
		}

		void InitializeAssemblyLookup () {
			AppDomain currentDomain = AppDomain.CurrentDomain;
			_assemblies = currentDomain.GetAssemblies ();

			int len = _assemblies.Length;
			for (int i = 0; i < len; ++i) {
				Assembly asm = _assemblies[i];
				string name = asm.GetCleanName ();

				AsmInfo info = new AsmInfo () {
					_name = name,
						_asm = asm,
				};
				//CSLog.D ("adding: " + name);
				_assemblyLookup[name] = info;
			}
		}

		string _GetCleanNameIfPrimitive (string typeName) {
			Type type;
			if (_shortTypeNames.TryGetValue (typeName, out type)) {
				string name = type.AssemblyQualifiedName;
				int pos = name.IndexOf (',');
				if (pos >= 0) {
					name = name.Substring (0, pos);
				}
				return name;
			}
			return typeName;
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

			//CSLog.E ("failed to load assmbly: " + namespaceName);
			return null;
		}
	}
}