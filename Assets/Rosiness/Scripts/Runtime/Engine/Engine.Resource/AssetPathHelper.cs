/****************************************************
	文件：AssetPathHelper.cs
	作者：世界和平
	日期：2020/12/4 9:26:50
	功能：Nothing
*****************************************************/
using System.IO;
using Rosiness.IO;

namespace Rosiness.Resource
{
    internal static class AssetPathHelper
    {
		/// <summary>
		/// 获取规范化的路径
		/// </summary>
		public static string GetRegularPath(string path)
		{
			return path.Replace('\\', '/').Replace("\\", "/"); //替换为Linux路径格式
		}

		/// <summary>
		/// 获取文件所在的目录路径（Linux格式）
		/// </summary>
		public static string GetDirectory(string filePath)
		{
			string directory = Path.GetDirectoryName(filePath);
			return GetRegularPath(directory);
		}

		/// <summary>
		/// 获取基于流文件夹的加载路径
		/// </summary>
		public static string MakeStreamingLoadPath(string path)
		{
			return StringFormat.Format("{0}/{1}", UnityEngine.Application.streamingAssetsPath, path);
		}

		/// <summary>
		/// 获取基于沙盒文件夹的加载路径
		/// </summary>
		public static string MakePersistentLoadPath(string path)
		{
#if UNITY_EDITOR
			// 注意：为了方便调试查看，编辑器下把存储目录放到项目里
			string projectPath = GetDirectory(UnityEngine.Application.dataPath);
			return StringFormat.Format("{0}/Sandbox/{1}", projectPath, path);
#else
		return StringFormat.Format("{0}/Sandbox/{1}", UnityEngine.Application.persistentDataPath, path);
#endif
		}

		/// <summary>
		/// 获取网络资源加载路径
		/// </summary>
		public static string ConvertToWWWPath(string path)
		{
			// 注意：WWW加载方式，必须要在路径前面加file://
#if UNITY_EDITOR
			return StringFormat.Format("file:///{0}", path);
#elif UNITY_IPHONE
			return StringFormat.Format("file://{0}", path);
#elif UNITY_ANDROID
			return path;
#elif UNITY_STANDALONE
			return StringFormat.Format("file:///{0}", path);
#endif
		}

		/// <summary>
		/// 获取AssetDatabase的加载路径
		/// </summary>
		public static string FindDatabaseAssetPath(string location)
		{
#if UNITY_EDITOR
			string path = $"{AssetSystem.LocationRoot}/{location}";
			string fileName = Path.GetFileName(path);
			string directory = GetDirectory(path);
			string assetPath = FindDatabaseAssetPath(directory, fileName);
			if (string.IsNullOrEmpty(assetPath))
				return path;
			return assetPath;
#else
			return string.Empty;
#endif
		}

		/// <summary>
		/// 获取AssetDatabase的加载路径
		/// </summary>
		public static string FindDatabaseAssetPath(string directory, string fileName)
		{
#if UNITY_EDITOR
			// AssetDatabase加载资源需要提供文件后缀格式，然而资源定位地址并没有文件格式信息。
			// 所以我们通过查找该文件所在文件夹内同名的首个文件来确定AssetDatabase的加载路径。
			// 注意：AssetDatabase.FindAssets() 返回文件内包括递归文件夹内所有资源的GUID
			string[] guids = UnityEditor.AssetDatabase.FindAssets(string.Empty, new[] { directory });
			for (int i = 0; i < guids.Length; i++)
			{
				string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);

				if (UnityEditor.AssetDatabase.IsValidFolder(assetPath))
					continue;

				string assetDirectory = GetDirectory(assetPath);
				if (assetDirectory != directory)
					continue;

				string assetName = Path.GetFileNameWithoutExtension(assetPath);
				if (assetName == fileName)
					return assetPath;
			}
#endif

			// 没有找到同名的资源文件
			RosinessLog.Warning($"Not found asset {fileName} in foder : {directory}");
			return string.Empty;
		}
	}
}