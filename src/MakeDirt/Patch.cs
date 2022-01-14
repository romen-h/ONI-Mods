using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace RomenH.MakeDirt
{
	[HarmonyPatch(typeof(RockCrusherConfig))]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static class RockCrusherConfig_ConfigureBuildingTemplate_Patch
	{
		public static void Postfix(GameObject __0)
		{
			var clay = ElementLoader.elements.Find((Element e) => e.HasTag(SimHashes.Clay.CreateTag()));
			var sand = ElementLoader.elements.Find((Element e) => e.HasTag(SimHashes.Sand.CreateTag()));
			var phosphorite = ElementLoader.elements.Find((Element e) => e.HasTag(GameTags.Phosphorite));
			var dirt = ElementLoader.elements.Find((Element e) => e.HasTag(GameTags.Dirt));
			var fertilizer = ElementLoader.elements.Find((Element e) => e.HasTag(GameTags.Fertilizer));

			if (clay != null && sand != null && phosphorite != null && dirt != null)
			{
				ComplexRecipe.RecipeElement[] inputs = new ComplexRecipe.RecipeElement[3]
				{
					new ComplexRecipe.RecipeElement(sand.tag, 80f),
					new ComplexRecipe.RecipeElement(clay.tag, 10f),
					new ComplexRecipe.RecipeElement(phosphorite.tag, 10f)
				};

				ComplexRecipe.RecipeElement[] outputs = new ComplexRecipe.RecipeElement[1]
				{
					new ComplexRecipe.RecipeElement(dirt.tag, 100f)
				};

				string text = ComplexRecipeManager.MakeRecipeID(RockCrusherConfig.ID, inputs, outputs);
				new ComplexRecipe(text, inputs, outputs)
				{
					time = 40f,
					description = string.Format(ModStrings.STRINGS.BUILDINGS.ROCKCRUSHER.MAKEDIRT_RECIPE_DESCRIPTION, sand.name, clay.name, phosphorite.name, dirt.name),

					nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
					fabricators = new List<Tag>
					{
						TagManager.Create(RockCrusherConfig.ID)
					}
				};
			}

			if (clay != null && sand != null && fertilizer != null && dirt != null)
			{
				ComplexRecipe.RecipeElement[] inputs = new ComplexRecipe.RecipeElement[3]
				{
					new ComplexRecipe.RecipeElement(sand.tag, 56f),
					new ComplexRecipe.RecipeElement(clay.tag, 8f),
					new ComplexRecipe.RecipeElement(fertilizer.tag, 36f)
				};

				ComplexRecipe.RecipeElement[] outputs = new ComplexRecipe.RecipeElement[1]
				{
					new ComplexRecipe.RecipeElement(dirt.tag, 100f)
				};

				string text = ComplexRecipeManager.MakeRecipeID(RockCrusherConfig.ID, inputs, outputs);
				new ComplexRecipe(text, inputs, outputs)
				{
					time = 40f,
					description = string.Format(ModStrings.STRINGS.BUILDINGS.ROCKCRUSHER.MAKEDIRT_RECIPE_DESCRIPTION, sand.name, clay.name, fertilizer.name, dirt.name),

					nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
					fabricators = new List<Tag>
					{
						TagManager.Create(RockCrusherConfig.ID)
					}
				};
			}
		}
	}
}
