using System;
using System.IO;

namespace Rosiness.Editor
{
	public class LabelNone : IAssetCollector
	{
		string IAssetCollector.GetAssetBundleLabel(string assetPath)
		{
			// 注意：如果依赖资源来自于忽略文件夹，那么会触发这个异常
			throw new Exception($"{nameof(AssetBundleCollectorSetting)} has depend asset in ignore folder : {assetPath}");
		}
	}

	/// <summary>
	/// 以文件路径作为标签名
	/// </summary>
	public class LabelByFolderPath : IAssetCollector
	{
		string IAssetCollector.GetAssetBundleLabel(string assetPath)
		{
			// 例如："Assets/Config/test.txt" --> "Assets/Config"
			return Path.GetDirectoryName(assetPath);
		}
	}

	/// <summary>
	/// 以文件夹路径作为标签名
	/// 注意：该文件夹下所有资源被打到一个AssetBundle文件里
	/// </summary>
	public class LabelByFilePath : IAssetCollector
	{
		string IAssetCollector.GetAssetBundleLabel(string assetPath)
		{
			// 例如："Assets/Config/test.txt" --> "Assets/Config/test"
			return assetPath.Remove(assetPath.LastIndexOf("."));
		}
	}
}