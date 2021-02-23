/****************************************************
	文件：AssetInfo.cs
	作者：世界和平
	日期：2021/2/23 10:2:8
	功能：资源信息类
*****************************************************/
using UnityEditor;

namespace Rosiness.Editor
{
	/// <summary>
	/// 资源信息类
	/// </summary>
	public class AssetInfo
	{
		public string AssetPath { private set; get; }
		public bool IsCollectAsset { private set; get; }
		public bool IsSceneAsset { private set; get; }
		public bool IsVideoAsset { private set; get; }

		/// <summary>
		/// 被依赖次数
		/// </summary>
		public int DependCount = 0;

		/// <summary>
		/// AssetBundle标签
		/// </summary>
		public string AssetBundleLabel = null;

		/// <summary>
		/// AssetBundle变体
		/// </summary>
		public string AssetBundleVariant = null;

		public AssetInfo(string assetPath)
		{
			AssetPath = assetPath;
			IsCollectAsset = AssetBundleCollectorSettingData.IsCollectAsset(assetPath);
			IsSceneAsset = AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(SceneAsset);
			IsVideoAsset = AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(UnityEngine.Video.VideoClip);
		}
	}
}