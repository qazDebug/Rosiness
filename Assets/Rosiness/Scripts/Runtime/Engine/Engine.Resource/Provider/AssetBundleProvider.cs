/****************************************************
	文件：AssetLoaderBase.cs
	作者：世界和平
	日期：2020/12/4 14:34:0
	功能：Nothing
*****************************************************/
using System;
using UnityEngine;

namespace Rosiness.Resource
{
    internal sealed class AssetBundleProvider : AssetProviderBase
    {
        private AssetBundleLoader _loader;
        private AssetBundleRequest _cacheRequest;
        public override float Progress
        {
            get
            {
                if (_cacheRequest == null)
                    return 0;
                return _cacheRequest.progress;
            }
        }

        public AssetBundleProvider(AssetLoaderBase owner, string assetName, System.Type assetType)
              : base(owner, assetName, assetType)
        {
            _loader = owner as AssetBundleLoader;
        }

        public override void Update()
        {
			if (IsDone)
				return;

			if (_loader.CacheBundle == null)
			{
				States = EAssetStates.Fail;
				InvokeCompletion();
			}

			if (States == EAssetStates.None)
			{
				States = EAssetStates.Loading;
			}

			// 1. 加载资源对象
			if (States == EAssetStates.Loading)
			{
				if (AssetType == null)
					_cacheRequest = _loader.CacheBundle.LoadAssetAsync(AssetName);
				else
					_cacheRequest = _loader.CacheBundle.LoadAssetAsync(AssetName, AssetType);
				States = EAssetStates.Checking;
			}

			// 2. 检测加载结果
			if (States == EAssetStates.Checking)
			{
				if (_cacheRequest.isDone == false)
					return;
				AssetObject = _cacheRequest.asset;
				States = AssetObject == null ? EAssetStates.Fail : EAssetStates.Success;
				if (States == EAssetStates.Fail)
					RosinessLog.Warning($"Failed to load asset object : {AssetName} from bundle : {_loader.BundleInfo.BundleName}");
				InvokeCompletion();
			}
		}
		public override void Destory()
		{
			base.Destory();

			var assetObject = AssetObject as UnityEngine.Object;
			if (assetObject != null)
			{
				if (assetObject is GameObject == false)
					Resources.UnloadAsset(assetObject);
			}
		}
	}
}