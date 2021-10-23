using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomenH.Common
{
	public static class ModifierUtil
	{
		public static ModifierSet.ModifierInfo CreateModifierInfo(string id, string type, string attribute, float value, float duration = 0, Units units = Units.Flat, bool multiplier = false, bool showInUI = true, bool isBad = false, string customIcon = "", string emoteAnim = "", float emoteCooldown = 0)
		{
			return new ModifierSet.ModifierInfo()
			{
				Id = id,
				Name = id,
				IdHash = new HashedString(id),
				Type = type,
				Attribute = attribute,
				Value = value,
				Units = units,
				Multiplier = multiplier,
				Duration = duration,
				ShowInUI = showInUI,
				Tier = 0,
				Notes = "",
				StompGroup = "",
				IsBad = isBad,
				CustomIcon = customIcon,
				TriggerFloatingText = true,
				EmoteAnim = emoteAnim,
				EmoteCooldown = emoteCooldown
			};
		}
	}
}
