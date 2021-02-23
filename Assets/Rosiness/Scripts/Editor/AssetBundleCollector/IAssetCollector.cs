namespace Rosiness.Editor
{
	public interface IAssetCollector
	{
		/// <summary>
		/// 获取资源的打包标签
		/// </summary>
		string GetAssetBundleLabel(string assetPath);
	}
}