using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomenH.DumpingSign
{
	public static class SignCategories
	{
		public static readonly Dictionary<Tag, KAnimFile> CategoryIcons = new Dictionary<Tag, KAnimFile>();

		internal static void InitCategories()
		{
			CategoryIcons[GameTags.Agriculture] = Assets.GetAnim("farmtilerotating_kanim");
			CategoryIcons[GameTags.BuildableProcessed] = Assets.GetAnim("rockrefinery_kanim");
			CategoryIcons[GameTags.BuildableRaw] = Assets.GetAnim("floor_basic_kanim");
			CategoryIcons[GameTags.Clothes] = Assets.GetAnim("unlock_clothing_kanim");
			CategoryIcons[GameTags.Compostable] = Assets.GetAnim("compost_kanim");
			CategoryIcons[GameTags.ConsumableOre] = Assets.GetAnim("generatorphos_kanim");
			CategoryIcons[GameTags.Egg] = Assets.GetAnim("incubator_kanim");
			CategoryIcons[GameTags.Farmable] = Assets.GetAnim("farmtilerotating_kanim");
			CategoryIcons[GameTags.Filter] = Assets.GetAnim("waterpurifier_kanim");
			CategoryIcons[GameTags.IndustrialIngredient] = Assets.GetAnim("craftingStation_kanim");
			CategoryIcons[GameTags.IndustrialProduct] = Assets.GetAnim("craftingStation_kanim");
			CategoryIcons[GameTags.Liquifiable] = Assets.GetAnim("liquid_tank_kanim");
			CategoryIcons[GameTags.ManufacturedMaterial] = Assets.GetAnim("craftingStation_kanim");
			CategoryIcons[GameTags.MedicalSupplies] = Assets.GetAnim("medical_cot_kanim");
			CategoryIcons[GameTags.Metal] = Assets.GetAnim("metalrefinery_kanim");
			CategoryIcons[GameTags.Organics] = Assets.GetAnim("farmtilerotating_kanim");
			CategoryIcons[GameTags.RareMaterials] = Assets.GetAnim("supermaterial_refinery_kanim");
			CategoryIcons[GameTags.RefinedMetal] = Assets.GetAnim("metalrefinery_kanim");
			CategoryIcons[GameTags.Seed] = Assets.GetAnim("farmtilerotating_kanim");
			CategoryIcons[GameTags.Sublimating] = Assets.GetAnim("sublimation_station_kanim");
		}
	}
}
