using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomenH.Common
{
	/// <summary>
	/// An implementation of IListableOption that uses a string value.
	/// </summary>
	public class StringListOption : IListableOption
	{
		public static implicit operator StringListOption(LocString name) => new StringListOption(name);

		public static bool operator ==(StringListOption one, StringListOption two) => one.Equals(two);

		public static bool operator !=(StringListOption one, StringListOption two) => !one.Equals(two);

		private readonly string name;

		public StringListOption(string optionName)
		{
			name = optionName;
		}

		public string GetProperName()
		{
			return name;
		}

		public override bool Equals(object obj)
		{
			return obj is StringListOption other && other.name == name;
		}

		public override int GetHashCode()
		{
			return name.GetHashCode();
		}

		public override string ToString()
		{
			return name;
		}
	}
}
