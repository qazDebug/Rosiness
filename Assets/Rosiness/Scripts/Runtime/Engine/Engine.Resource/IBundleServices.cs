/****************************************************
	文件：IBundleServices.cs
	作者：世界和平
	日期：2020/12/4 9:27:20
	功能：Nothing
*****************************************************/

namespace Rosiness.Resource
{
    public interface IBundleServices
    {
		/// <summary>
		/// 检测下载内容的完整性
		/// </summary>
		bool CheckContentIntegrity(string bundleName);

		/// <summary>
		/// 获取AssetBundle的信息
		/// </summary>
		AssetBundleInfo GetAssetBundleInfo(string bundleName);

		/// <summary>
		/// 获取资源包名称
		/// </summary>
		string GetAssetBundleName(string assetPath);

		/// <summary>
		/// 获取AssetBundle的直接依赖列表
		/// </summary>
		string[] GetDirectDependencies(string bundleName);

		/// <summary>
		/// 获取AssetBundle的所有依赖列表
		/// </summary>
		string[] GetAllDependencies(string bundleName);
	}
}