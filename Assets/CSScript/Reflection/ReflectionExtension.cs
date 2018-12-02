using System;
using System.Collections;
using System.Collections.Generic;
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
		return value.GetType().IsInteger();
	}

	public static bool IsFloat (this object value) {
		return value.GetType().IsFloat();
	}

	public static bool IsNumeric (this object value) {
		return value.GetType().IsNumeric();
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

}