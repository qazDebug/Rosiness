/****************************************************
	文件：AssetLoaderBase.cs
	作者：世界和平
	日期：2020/12/4 14:34:0
	功能：Nothing
*****************************************************/
using System;

namespace Rosiness.Resource
{
    internal sealed class AssetDatabaseProvider : AssetProviderBase
    {
        public override float Progress
        {
            get
            {
                if (IsDone)
                    return 100f;
                else
                    return 0;
            }
        }
		
        public AssetDatabaseProvider(AssetLoaderBase owner, string assetName, System.Type assetType)
         : base(owner, assetName, assetType)
        {
        }

        public override void Update()
        {
#if UNITY_EDITOR
			if (IsDone)
				return;

			if (States == EAssetStates.None)
			{
				States = EAssetStates.Loading;
			}

			// 1. 加载资源对象
			if (States == EAssetStates.Loading)
			{
				string assetPath = Owner.BundleInfo.LocalPath;
				AssetObject = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, AssetType);
				States = EAssetStates.Checking;
			}

			// 2. 检测加载结果
			if (States == EAssetStates.Checking)
			{
				States = AssetObject == null ? EAssetStates.Fail : EAssetStates.Success;
				if (States == EAssetStates.Fail)
					RosinessLog.Warning($"Failed to load asset object : {Owner.BundleInfo.LocalPath} : {AssetName}");
				InvokeCompletion();
			}
#endif
		}
	}
}