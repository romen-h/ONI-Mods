using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomenH.Common
{
	public class Logger
	{
		private readonly string modName;

		public bool DebugLoggingEnabled
		{ get; set; } = false;

		internal Logger(string modName)
		{
			this.modName = modName;
		}

		public void Info(string message)
		{
			global::Debug.Log($"[{modName}] {message}");
		}

		public void Warn(string message)
		{
			global::Debug.LogWarning($"[{modName}] {message}");
		}

		public void Error(string message, Exception ex = null)
		{
			UnityEngine.Debug.LogWarning($"[{modName}] {message}");
			if (DebugLoggingEnabled)
			{
				global::Debug.LogException(ex);
			}
		}

		public void Debug(string message)
		{
			if (DebugLoggingEnabled)
			{
				global::Debug.Log($"[{modName}] {message}");
			}
		}
	}
}
