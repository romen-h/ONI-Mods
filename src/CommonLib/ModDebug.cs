using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RomenH.Common
{
	public static class ModDebug
	{
		[Conditional("DEBUG")]
		public static void LogThisMethod(string msg = null)
		{
			StackTrace stackTrace = new StackTrace();
			// Get calling method name
			var method = stackTrace.GetFrame(1)?.GetMethod();
			if (method != null)
			{
				Debug.Log($"\n[DEBUG_TRACE] {method.DeclaringType?.ToString()}.{method.Name}\n{msg}");
			}
		}
	}
}
