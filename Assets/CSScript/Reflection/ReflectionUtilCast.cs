using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CSScript {

	public partial class ReflectionUtil {
		Dictionary<Type, MethodInfo> _castMethods = new Dictionary<Type, MethodInfo> ();
		Dictionary<KeyValuePair<Type, Type>, MethodInfo> _dynamicCasts = new Dictionary<KeyValuePair<Type, Type>, MethodInfo> ();
		MethodInfo _castMethod;

		void InitializeCast () {
			_castMethod = this.GetType ().GetMethod ("DynamicCast", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		T DynamicCast<T> (object o) {
			try {
				return (T) o;
			} catch (Exception) { }

			Type st = o.GetType ();
			Type dt = typeof (T);

			KeyValuePair<Type, Type> key = new KeyValuePair<Type, Type> (st, dt);

			MethodInfo meth;

			if (!_dynamicCasts.TryGetValue (key, out meth)) {
				meth = st.GetMethodWithReturnTypeAndParemterTypes ("op_Implicit", BindingFlags.Static | BindingFlags.Public, dt, st);
				if (meth == null) {
					meth = st.GetMethodWithReturnTypeAndParemterTypes ("op_Explicit", BindingFlags.Static | BindingFlags.Public, dt, st);
				}
				if (meth == null) {
					meth = dt.GetMethodWithReturnTypeAndParemterTypes ("op_Implicit", BindingFlags.Static | BindingFlags.Public, dt, st);
				}
				if (meth == null) {
					meth = dt.GetMethodWithReturnTypeAndParemterTypes ("op_Explicit", BindingFlags.Static | BindingFlags.Public, dt, st);
				}
				_dynamicCasts[key] = meth;
			}

			if (meth == null) throw new InvalidCastException ("Invalid Cast.");

			return (T) meth.Invoke (null, new object[] { o });
		}

		bool _CanCast (Type type, object o) {
			bool canCast = false;
			try {
				_Cast (type, o);
				canCast = true;
			} catch (Exception) {
				//CSLog.D (e.Message);
				canCast = false;
			}
			return canCast;
		}

		object _Cast (Type type, object o) {
			if (type.IsNumeric () && o.IsNumeric ()) {
				switch (Type.GetTypeCode (type)) {
					case TypeCode.Byte:
						return Convert.ToByte (o);
					case TypeCode.SByte:
						return Convert.ToSByte (o);
					case TypeCode.UInt16:
						return Convert.ToUInt16 (o);
					case TypeCode.UInt32:
						return Convert.ToUInt32 (o);
					case TypeCode.UInt64:
						return Convert.ToUInt64 (o);
					case TypeCode.Int16:
						return Convert.ToInt16 (o);
					case TypeCode.Int32:
						return Convert.ToInt32 (o);
					case TypeCode.Int64:
						return Convert.ToInt64 (o);
					case TypeCode.Decimal:
						return Convert.ToDecimal (o);
					case TypeCode.Double:
						return Convert.ToDouble (o);
					case TypeCode.Single:
						return Convert.ToSingle (o);
				}
			}

			MethodInfo method;
			if (!_castMethods.TryGetValue (type, out method)) {
				method = _castMethod.MakeGenericMethod (type);
				_castMethods[type] = method;
			}

			object ret = null;
			try {
				ret = method.Invoke (this, new object[] { o });
			} catch (System.Reflection.TargetInvocationException e) {
				throw e.InnerException;
			}
			return ret;
		}
	}
}