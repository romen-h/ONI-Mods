#if SPACED_OUT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PeterHan.PLib.Options;

namespace RomenH.GermicideLamp
{
	public class DlcOnlyOptionAttribute : OptionAttribute
	{
		public override bool Visible
		{
			get => DlcManager.IsExpansion1Active();
			set { }
		}

		public DlcOnlyOptionAttribute(string title, string tooltip = null, string category = null) : base(title, tooltip, category)
		{ }
	}
}
#endif
