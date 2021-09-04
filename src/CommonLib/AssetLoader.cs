using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CommonLib
{
	public static class AssetLoader
	{
		private static readonly TextureAtlas ReferenceTileAtlas = Assets.GetTextureAtlas("tiles_solid");
		

		public static TextureAtlas GetCustomTileAtlas(string path)
		{
			TextureAtlas textureAtlas = null;
			try
			{
				var data = File.ReadAllBytes(path);
				var tex = new Texture2D(2, 2);
				tex.LoadImage(data);
				textureAtlas = ScriptableObject.CreateInstance<TextureAtlas>();
				textureAtlas.texture = tex;
				textureAtlas.scaleFactor = ReferenceTileAtlas.scaleFactor;
				textureAtlas.items = ReferenceTileAtlas.items;
			}
			catch
			{
				Debug.LogError($"Could not load atlas image at path {path}");
			}

			return textureAtlas;
		}

		public static BlockTileDecorInfo GetCustomTileDecor(string path, string baseInfo)
		{
			BlockTileDecorInfo decorInfo = null;
			try
			{
				BlockTileDecorInfo ReferenceDecorInfo = Assets.GetBlockTileDecorInfo(baseInfo);

				var data = File.ReadAllBytes(path);
				var tex = new Texture2D(2, 2);
				tex.LoadImage(data);
				var textureAtlas = ScriptableObject.CreateInstance<TextureAtlas>();
				textureAtlas.texture = tex;
				textureAtlas.scaleFactor = ReferenceDecorInfo.atlas.scaleFactor;
				textureAtlas.items = ReferenceDecorInfo.atlas.items;

				decorInfo = ScriptableObject.CreateInstance<BlockTileDecorInfo>();
				decorInfo.atlas = textureAtlas;
				decorInfo.sortOrder = ReferenceDecorInfo.sortOrder;
				decorInfo.decor = ReferenceDecorInfo.decor;
			}
			catch
			{
				Debug.LogError($"Could not load atlas image at path {path}");
			}

			return decorInfo;
		}
	}
}
