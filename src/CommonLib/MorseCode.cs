using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
	public static class MorseCode
	{
		public static string Encode(string message)
		{
			string code = "";
			foreach (char c in message)
			{
				code += Encode(c);
			}
			return code;
		}

		public static string Encode(char c)
		{
			char upper = char.ToUpper(c);

			switch (upper)
			{
				case 'A': return ".- ";
				case 'B': return "-... ";
				case 'C': return "-.-. ";
				case 'D': return "-.. ";
				case 'E': return ". ";
				case 'F': return "..-. ";
				case 'G': return "--. ";
				case 'H': return ".... ";
				case 'I': return ".. ";
				case 'J': return ".--- ";
				case 'K': return "-.- ";
				case 'L': return ".-.. ";
				case 'M': return "-- ";
				case 'N': return "-. ";
				case 'O': return "--- ";
				case 'P': return ".--. ";
				case 'Q': return "--.- ";
				case 'R': return ".-. ";
				case 'S': return "... ";
				case 'T': return "- ";
				case 'U': return "..- ";
				case 'V': return "...- ";
				case 'W': return ".-- ";
				case 'X': return "-..- ";
				case 'Y': return "-.-- ";
				case 'Z': return "--.. ";

				case '1': return ".---- ";
				case '2': return "..--- ";
				case '3': return "...-- ";
				case '4': return "....- ";
				case '5': return "..... ";
				case '6': return "-.... ";
				case '7': return "--... ";
				case '8': return "---.. ";
				case '9': return "----. ";
				case '0': return "----- ";

				default:
					return " ";
			}
		}

		public static bool[] ExpandToFrameData(string code, int pulseLength = 1)
		{
			List<bool> data = new List<bool>();
			foreach (char c in code)
			{
				if (c=='.')
				{
					for (int i=0; i<pulseLength; i++)
						data.Add(true);
				}
				else if (c=='-')
				{
					for (int i=0; i<pulseLength; i++)
					{
						data.Add(true);
						data.Add(true);
						data.Add(true);
					}
				}
				else if (c==' ')
				{
					for (int i=0; i<pulseLength; i++)
					{
						data.Add(false);
						data.Add(false);
						data.Add(false);
					}
				}

				for (int i=0; i<pulseLength; i++)
				{
					data.Add(false);
				}
			}

			return data.ToArray();
		}
	}
}
