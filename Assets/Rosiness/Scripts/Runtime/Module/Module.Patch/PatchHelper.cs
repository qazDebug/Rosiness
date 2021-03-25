/****************************************************
	文件：PatchHelper.cs
	作者：世界和平
	日期：2021/3/19 18:59:14
	功能：Nothing
*****************************************************/
using Rosiness.Resource;
using System.IO;

namespace Rosiness.Patch
{
    internal static class PatchHelper
    {
		private const string StrCacheFileName = "Cache.bytes";

		/// <summary>
		/// 清空沙盒目录
		/// </summary>
		public static void ClearSandbox()
		{
			string directoryPath = AssetPathHelper.MakePersistentLoadPath(string.Empty);
			Directory.Delete(directoryPath, true);
		}

		/// <summary>
		/// 获取沙盒内缓存文件的路径
		/// </summary>
		public static string GetSandboxCacheFilePath()
		{
			return AssetPathHelper.MakePersistentLoadPath(StrCacheFileName);
		}

		/// <summary>
		/// 检测沙盒内缓存文件是否存在
		/// </summary>
		public static bool CheckSandboxCacheFileExist()
		{
			string filePath = GetSandboxCacheFilePath();
			return File.Exists(filePath);
		}

		/// <summary>
		/// 检测沙盒内补丁清单文件是否存在
		/// </summary>
		public static bool CheckSandboxPatchManifestFileExist()
		{
			string filePath = AssetPathHelper.MakePersistentLoadPath(PatchDefine.PatchManifestFileName);
			return File.Exists(filePath);
		}

		/// <summary>
		/// 获取缓存文件的存储路径
		/// </summary>
		public static string MakeSandboxCacheFilePath(string fileName)
		{
			return AssetPathHelper.MakePersistentLoadPath($"Cache/{fileName}");
		}
	}
}