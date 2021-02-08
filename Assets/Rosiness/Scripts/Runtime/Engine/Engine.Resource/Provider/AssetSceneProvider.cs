/****************************************************
	文件：AssetLoaderBase.cs
	作者：世界和平
	日期：2020/12/4 14:34:0
	功能：Nothing
*****************************************************/
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rosiness.Resource
{
    internal sealed class AssetSceneProvider : AssetProviderBase
    {
        private SceneInstanceParam _param;
        private AsyncOperation _asyncOp;

        public override float Progress
        {
            get
            {
                if (_asyncOp == null)
                    return 0;
                return _asyncOp.progress;
            }
        }

        public AssetSceneProvider(AssetLoaderBase owner, string assetName, Type assetType, SceneInstanceParam sceneParam)
            : base (owner,assetName,assetType)
        {
            _param = sceneParam;
        }

        public override void Update()
        {
            if (IsDone)
                return;

            if(States == EAssetStates.None)
            {
                States = EAssetStates.Loading;
            }

			// 1. 加载资源对象
			if (States == EAssetStates.Loading)
			{
				var mode = _param.IsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
				_asyncOp = SceneManager.LoadSceneAsync(AssetName, mode);
				if (_asyncOp != null)
				{
					_asyncOp.allowSceneActivation = _param.ActivateOnLoad;
					States = EAssetStates.Checking;
				}
				else
				{
					RosinessLog.Warning($"Failed to load scene : {AssetName}");
					States = EAssetStates.Fail;
					InvokeCompletion();
				}
			}

			// 2. 检测加载结果
			if (States == EAssetStates.Checking)
			{
				if (_asyncOp.isDone || (_param.ActivateOnLoad == false && _asyncOp.progress == 0.9f))
				{
					SceneInstance instance = new SceneInstance(_asyncOp);
					instance.Scene = SceneManager.GetSceneByName(AssetName);
					AssetScene = instance;
					States = EAssetStates.Success;
					InvokeCompletion();
				}
			}
		}
		public override void Destory()
		{
			base.Destory();

			if (_param.IsAdditive)
				SceneManager.UnloadSceneAsync(AssetName);
		}
	}
}