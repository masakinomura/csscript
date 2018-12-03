using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CSScript {

	public class NullWithType {
		public System.Type _type;
		public NullWithType (System.Type type) {
			_type = type;
		}
	}

	public partial class ReflectionUtil {

		object[] CastArgsIfNeeded (ParameterInfo[] parameters, object[] args) {
			int len = args.Length;
			for (int i = 0; i < len; ++i) {
				if (args[i] is NullWithType) {
					args[i] = ReflectionUtil.Cast (parameters[i].ParameterType, null);
				} else if (parameters[i].ParameterType != args[i].GetType ()) {
					args[i] = ReflectionUtil.Cast (parameters[i].ParameterType, args[i]);
				}
			}
			return args;
		}

		object _CallMethod (object target, string methodName, BindingFlags bindingFlags, params object[] args) {
			Type targetType;
			bool isStatic;
			if (target is Type) {
				targetType = target as Type;
				isStatic = true;
				bindingFlags |= BindingFlags.Static;
				bindingFlags &= ~BindingFlags.Instance;
			} else {
				targetType = target.GetType ();
				isStatic = false;
				bindingFlags |= BindingFlags.Instance;
				bindingFlags &= ~BindingFlags.Static;
			}

			MethodInfo method = _GetCallableMethod (targetType, methodName, bindingFlags, args);
			if (method == null) {
				throw new System.MissingMethodException (methodName + " cannot be found..");
			}
			if (isStatic) {
				return method.Invoke (null, CastArgsIfNeeded (method.GetParameters (), args));
			} else {
				return method.Invoke (target, CastArgsIfNeeded (method.GetParameters (), args));
			}
		}

		bool CheckParameterCount (ParameterInfo[] parameters, int count) {
			int min = 0;
			int max = 0;

			if (parameters != null && parameters.Length > 0) {
				max = parameters.Length;

				for (int i = 0; i < max; ++i) {
					if (!parameters[i].HasDefaultValue) {
						min++;
					}
				}
			}

			return (min <= count && count <= max);
		}

		MethodInfo _GetCallableMethod (Type toSearch, string methodName, BindingFlags bindingFlags, params object[] args) {

			MethodInfo[] methods = toSearch.GetMethods (bindingFlags);
			MethodInfo candidate = null;
			int len = methods.Length;

			int inParamCount = 0;
			if (args != null) {
				inParamCount = args.Length;
			}

			bool ambiguous = false;

			for (int i = 0; i < len; ++i) {
				MethodInfo method = methods[i];
				if (method.Name != methodName) {
					// skip function names don't match
					continue;
				}

				ParameterInfo[] parameters = method.GetParameters ();
				if (!CheckParameterCount (parameters, inParamCount)) {
					//parameter counts do not match
					continue;
				}

				bool exactMatch = true;
				for (int j = 0; j < inParamCount; ++j) {
					NullWithType nullType = args[j] as NullWithType;
					if (nullType != null) {
						if (nullType._type != parameters[j].ParameterType) {
							exactMatch = false;
							if (!_CanCast (parameters[j].ParameterType, null)) {
								continue;
							}
						}
					} else if (args[j] == null || args[j].GetType () != parameters[j].ParameterType) {
						exactMatch = false;
						if (!_CanCast (parameters[j].ParameterType, args[j])) {
							continue;
						}
					}
				}

				if (exactMatch) {
					return method;
				}

				if (candidate != null) {
					ambiguous = true;
				}
				candidate = method;
			}

			if (ambiguous) {
				throw new AmbiguousMatchException ("There are multiple methods that matches with " + methodName + " in " + toSearch.ToString ());
			}

			return candidate;
		}
	}
}