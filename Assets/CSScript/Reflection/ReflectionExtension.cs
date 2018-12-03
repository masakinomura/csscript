using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public static class ReflectionExtension {

	public static string GetCleanName (this System.Reflection.Assembly asm) {
		string name = asm.FullName;
		int pos = name.IndexOf (',');
		if (pos >= 0) {
			name = name.Substring (0, pos);
		}
		return name;
	}

	public static bool IsInteger (this object value) {
		return value.GetType ().IsInteger ();
	}

	public static bool IsFloat (this object value) {
		return value.GetType ().IsFloat ();
	}

	public static bool IsNumeric (this object value) {
		return value.GetType ().IsNumeric ();
	}

	public static bool IsInteger (this Type type) {
		switch (Type.GetTypeCode (type)) {
			case TypeCode.Byte:
			case TypeCode.SByte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
				return true;
			default:
				return false;
		}
	}

	public static bool IsFloat (this Type type) {
		switch (Type.GetTypeCode (type)) {
			case TypeCode.Decimal:
			case TypeCode.Double:
			case TypeCode.Single:
				return true;
			default:
				return false;
		}
	}

	public static bool IsNumeric (this Type type) {
		switch (Type.GetTypeCode (type)) {
			case TypeCode.Byte:
			case TypeCode.SByte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Decimal:
			case TypeCode.Double:
			case TypeCode.Single:
				return true;
			default:
				return false;
		}
	}

	public static MethodInfo GetMethodWithReturnTypeAndParemterTypes (this Type toSearch, string methodName, BindingFlags bindingFlags, Type returnType, params Type[] paramTypes) {
		return Array.Find (
			toSearch.GetMethods (bindingFlags),
			delegate (MethodInfo inf) {
				if (inf.Name != methodName || inf.ReturnType != returnType) {
					return false;
				}
				ParameterInfo[] parameters = inf.GetParameters ();
				if (paramTypes == null && parameters == null) {
					return true;
				}

				if (paramTypes != null && parameters == null) {
					return false;
				}

				if (paramTypes == null && parameters != null) {
					return false;
				}

				if (paramTypes.Length != parameters.Length) {
					return false;
				}

				int len = parameters.Length;
				for (int i = 0; i < len; ++i) {
					if (parameters[i].ParameterType != paramTypes[i]) {
						return false;
					}
				}
				return true;
			}
		);
	}

}