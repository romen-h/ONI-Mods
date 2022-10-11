using System.IO;
using System.Reflection;

namespace RomenH.CommonLib
{
	public static class Dumper
	{
		private static string GetModDirectory()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		public static void Dump(BlockTileDecorInfo info)
		{
			if (info == null) return;

			Debug.Log("");
			Debug.Log($"Tile Decor Info: {info.name}");

			foreach (var d in info.decor)
			{
				Debug.Log("");
				Debug.Log($"name: {d.name}");
				Debug.Log($"probabilityCutoff: {d.probabilityCutoff}");
				Debug.Log($"requiredConnections: {d.requiredConnections}");
				Debug.Log($"forbiddenConnections: {d.forbiddenConnections}");
				Debug.Log("variants:");
				foreach (var v in d.variants)
				{
					Debug.Log($"    name: {v.name}");
					Debug.Log($"    offset: {v.offset}");
					Debug.Log($"    variant.uvBox: {v.atlasItem.uvBox}");
					for (int i = 0; i < v.atlasItem.uvs.Length; i++)
					{
						Debug.Log($"    variant.uvs[{i}]: {v.atlasItem.uvs[i]}");
					}
				}
			}
		}
	}
}
