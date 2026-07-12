using System;
using System.IO;

using UnityEngine;

namespace RomenH.Common
{
	public static class AssetLoader
	{
		private static readonly TextureAtlas ReferenceTileAtlas = Assets.GetTextureAtlas("tiles_solid");

		public static Texture2D LoadTexture(string subPath)
		{
			try
			{
				string path = Path.Combine(ModCommon.Folder, "textures", subPath);
				var pngBytes = File.ReadAllBytes(path);
				var texture = new Texture2D(2,2);
				texture.LoadImage(pngBytes);
				return texture;
			}
			catch (Exception ex)
			{
				ModCommon.Log.Error($"Failed to load texture: textures/{subPath}", ex);
				return null;
			}
		}

		public static TextureAtlas GetCustomTileAtlas(string filename)
		{
			TextureAtlas textureAtlas = null;
			try
			{
				string path = Path.Combine(ModCommon.Folder, "textures", filename);
				var data = File.ReadAllBytes(path);
				var tex = new Texture2D(2, 2);
				tex.LoadImage(data);
				textureAtlas = ScriptableObject.CreateInstance<TextureAtlas>();
				textureAtlas.texture = tex;
				textureAtlas.scaleFactor = ReferenceTileAtlas.scaleFactor;
				textureAtlas.items = ReferenceTileAtlas.items;
			}
			catch (Exception ex)
			{
				Debug.LogError($"Could not load atlas image: {filename}");
				Debug.LogException(ex);
			}

			return textureAtlas;
		}
	}
}
