/****************************************************
	文件：AssetBundleLoader.cs
	作者：世界和平
	日期：2020/12/4 14:34:20
	功能：Nothing
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Resource
{
    internal sealed class AssetBundleLoader : AssetLoaderBase
    {
        private readonly List<AssetLoaderBase> _depends = new List<AssetLoaderBase>(10);
        //private WebFileRequest _downloader;
        private AssetBundleCreateRequest _cacheRequest;
        internal AssetBundle CacheBundle { private set; get; }

        public AssetBundleLoader(AssetBundleInfo bundleInfo)
            : base(bundleInfo)
        {
            // 准备依赖列表
            string[] dependencies = AssetSystem.BundleServices.GetDirectDependencies(bundleInfo.BundleName);
            if (dependencies != null && dependencies.Length > 0)
            {
                foreach (string dependBundleName in dependencies)
                {
                    AssetBundleInfo dependBundleInfo = AssetSystem.BundleServices.GetAssetBundleInfo(dependBundleName);
                    AssetLoaderBase dependLoader = AssetSystem.CreateLoaderInternal(dependBundleInfo);
                    _depends.Add(dependLoader);
                }
            }
        }

        public override void Update()
        {
			// 如果资源文件加载完毕
			if (States == ELoaderStates.Success || States == ELoaderStates.Fail)
			{
				UpdateAllProvider();
				return;
			}

			if (States == ELoaderStates.None)
			{
				// 检测加载地址是否为空
				if (string.IsNullOrEmpty(BundleInfo.LocalPath))
				{
					States = ELoaderStates.Fail;
					return;
				}

				if (string.IsNullOrEmpty(BundleInfo.RemoteURL))
					States = ELoaderStates.CheckDepends;
				else
					States = ELoaderStates.Download;
			}

			// 1. 从服务器下载
			if (States == ELoaderStates.Download)
			{
				
			}

			// 2. 检测服务器下载结果
			if (States == ELoaderStates.CheckDownload)
			{ 
			}

			// 3. 检测所有依赖完成状态
			if (States == ELoaderStates.CheckDepends)
			{
				foreach (var dpLoader in _depends)
				{
					if (dpLoader.IsDone() == false)
						return;
				}
				States = ELoaderStates.LoadFile;
			}

			// 4. 加载AssetBundle
			if (States == ELoaderStates.LoadFile)
			{
#if UNITY_EDITOR
				// 注意：Unity2017.4编辑器模式下，如果AssetBundle文件不存在会导致编辑器崩溃，这里做了预判。
				if (System.IO.File.Exists(BundleInfo.LocalPath) == false)
				{
					RosinessLog.Warning($"Not found assetBundle file : {BundleInfo.LocalPath}");
					States = ELoaderStates.Fail;
					return;
				}
#endif

				// Load assetBundle file
				if (BundleInfo.IsEncrypted)
				{
					if (AssetSystem.DecryptServices == null)
						throw new Exception($"{nameof(AssetBundleLoader)} need IDecryptServices : {BundleInfo.BundleName}");

					//EDecryptMethod decryptType = AssetSystem.DecryptServices.DecryptType;
					//if (decryptType == EDecryptMethod.GetDecryptOffset)
					//{
					//	ulong offset = AssetSystem.DecryptServices.GetDecryptOffset(BundleInfo);
					//	_cacheRequest = AssetBundle.LoadFromFileAsync(BundleInfo.LocalPath, 0, offset);
					//}
					//else if (decryptType == EDecryptMethod.GetDecryptBinary)
					//{
					//	byte[] binary = AssetSystem.DecryptServices.GetDecryptBinary(BundleInfo);
					//	_cacheRequest = AssetBundle.LoadFromMemoryAsync(binary);
					//}
					//else
					//{
					//	throw new NotImplementedException($"{decryptType}");
					//}
				}
				else
				{
					_cacheRequest = AssetBundle.LoadFromFileAsync(BundleInfo.LocalPath);
				}
				States = ELoaderStates.CheckFile;
			}

			// 5. 检测AssetBundle加载结果
			if (States == ELoaderStates.CheckFile)
			{
				if (_cacheRequest.isDone == false)
					return;
				CacheBundle = _cacheRequest.assetBundle;

				// Check error
				if (CacheBundle == null)
				{
					RosinessLog.Warning($"Failed to load assetBundle file : {BundleInfo.BundleName}");
					States = ELoaderStates.Fail;
				}
				else
				{
					States = ELoaderStates.Success;
				}
			}
		}

		public override void Reference()
		{
			base.Reference();

			// 同时引用一遍所有依赖资源
			for (int i = 0; i < _depends.Count; i++)
			{
				_depends[i].Reference();
			}
		}
		public override void Release()
		{
			base.Release();

			// 同时释放一遍所有依赖资源
			for (int i = 0; i < _depends.Count; i++)
			{
				_depends[i].Release();
			}
		}
		public override void Destroy(bool force)
		{
			base.Destroy(force);

			// Check fatal
			if (RefCount > 0)
				throw new Exception($"Bundle file loader ref is not zero : {BundleInfo.BundleName}");
			if (IsDone() == false)
				throw new Exception($"Bundle file loader is not done : {BundleInfo.BundleName}");

			//if (_downloader != null)
			//{
			//	_downloader.Dispose();
			//	_downloader = null;
			//}

			if (CacheBundle != null)
			{
				CacheBundle.Unload(force);
				CacheBundle = null;
			}

			_depends.Clear();
		}
		public override bool IsDone()
		{
			if (base.IsDone() == false)
				return false;

			return CheckAllProviderIsDone();
		}
	}
}