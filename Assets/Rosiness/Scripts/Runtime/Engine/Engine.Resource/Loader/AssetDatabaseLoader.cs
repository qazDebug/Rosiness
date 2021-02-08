/****************************************************
	文件：AssetDatabaseLoader.cs
	作者：世界和平
	日期：2020/12/4 14:34:8
	功能：Nothing
*****************************************************/

namespace Rosiness.Resource
{
    internal sealed class AssetDatabaseLoader: AssetLoaderBase
    {
		public AssetDatabaseLoader(AssetBundleInfo bundleInfo)
			: base(bundleInfo)
		{
		}
		public override void Update()
		{
#if UNITY_EDITOR
			// 如果资源文件加载完毕
			if (States == ELoaderStates.Fail || States == ELoaderStates.Success)
			{
				UpdateAllProvider();
				return;
			}

			// 检测资源文件是否存在
			string guid = UnityEditor.AssetDatabase.AssetPathToGUID(BundleInfo.LocalPath);
			if (string.IsNullOrEmpty(guid))
				States = ELoaderStates.Fail;
			else
				States = ELoaderStates.Success;
#endif
		}
	}
}