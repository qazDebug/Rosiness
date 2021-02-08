/****************************************************
	文件：AssetSystem.cs
	作者：世界和平
	日期：2020/12/4 9:26:58
	功能：Nothing
*****************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Resource
{
    /// <summary>
    /// 基于定位的资源系统
    /// </summary>
    internal static class AssetSystem
	{
		private static readonly List<AssetLoaderBase> _loaders = new List<AssetLoaderBase>(1000);
		private static readonly List<string> _removeKeys = new List<string>(100);
		private static bool _isInitialize = false;


		/// <summary>
		/// 资源定位的根路径
		/// </summary>
		public static string LocationRoot { private set; get; }

		/// <summary>
		/// 在编辑器下模拟运行
		/// </summary>
		public static bool SimulationOnEditor { private set; get; }

		/// <summary>
		/// AssetBundle服务接口
		/// </summary>
		public static IBundleServices BundleServices { private set; get; }

		/// <summary>
		/// 文件解密服务器接口
		/// </summary>
		public static IDecryptServices DecryptServices { private set; get; }

		/// <summary>
		/// 初始化资源系统
		/// 注意：在使用AssetSystem之前需要初始化
		/// </summary>
		public static void Initialize(string locationRoot, bool simulationOnEditor, IBundleServices bundleServices, IDecryptServices decryptServices)
		{
			if (_isInitialize == false)
			{
				_isInitialize = true;
				LocationRoot = AssetPathHelper.GetRegularPath(locationRoot);
				SimulationOnEditor = simulationOnEditor;
				BundleServices = bundleServices;
				DecryptServices = decryptServices;
			}
			else
			{
				throw new Exception($"{nameof(AssetSystem)} is already initialized");
			}
		}

		/// <summary>
		/// 轮询更新
		/// </summary>
		public static void UpdatePoll()
		{
			for (int i = 0; i < _loaders.Count; i++)
			{
				_loaders[i].Update();
			}

			UpdateScene();
		}

		/// <summary>
		/// 获取资源信息
		/// </summary>
		public static AssetBundleInfo GetAssetBundleInfo(string location)
        {
			if (!_isInitialize)
				throw new Exception($"{nameof(AssetSystem)} is not initialize.");
            if (SimulationOnEditor)
            {
#if UNITY_EDITOR
				string assetPath = AssetPathHelper.FindDatabaseAssetPath(location);
				AssetBundleInfo bundleInfo = new AssetBundleInfo(assetPath, assetPath);
				return bundleInfo;
#else
				throw new Exception($"AssetSystem simulation only support unity editor.");
#endif
			}
            else
            {
				if(BundleServices == null)
					throw new Exception($"{nameof(BundleServices)} is null. Use {nameof(AssetSystem.Initialize)}");

				string assetPath = $"{LocationRoot}/{location}".ToLower();
				string bundleName = BundleServices.GetAssetBundleName(assetPath);
				return BundleServices.GetAssetBundleInfo(bundleName);
			}
		}

		/// <summary>
		/// 创建资源文件加载器
		/// </summary>
		public static AssetLoaderBase CreateLoader(string location)
		{
			AssetBundleInfo bundleInfo = GetAssetBundleInfo(location);
			return CreateLoaderInternal(bundleInfo);
		}
		internal static AssetLoaderBase CreateLoaderInternal(AssetBundleInfo bundleInfo)
		{
			// 如果加载器已经存在
			AssetLoaderBase loader = TryGetLoader(bundleInfo.BundleName);
			if (loader != null)
				return loader;

			// 创建加载器
			if (SimulationOnEditor)
				loader = new AssetDatabaseLoader(bundleInfo);
			else
				loader = new AssetBundleLoader(bundleInfo);

			// 新增下载需求
			_loaders.Add(loader);
			return loader;
		}

		/// <summary>
		/// 更新场景
		/// 注意：因为场景比较特殊，需要立刻回收
		/// </summary>
		private static void UpdateScene()
		{
			for (int i = _loaders.Count - 1; i >= 0; i--)
			{
				AssetLoaderBase loader = _loaders[i];
				if (loader.IsSceneLoader && loader.CanDestroy())
				{
					loader.Destroy(true);
					_loaders.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// 资源回收
		/// 卸载引用计数为零的资源
		/// </summary>
		public static void UnloadUnusedAssets()
		{
			for (int i = _loaders.Count - 1; i >= 0; i--)
			{
				AssetLoaderBase loader = _loaders[i];
				if (loader.CanDestroy())
				{
					loader.Destroy(true);
					_loaders.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// 强制回收所有资源
		/// </summary>
		public static void UnloadAllAssets()
		{
			for (int i = 0; i < _loaders.Count; i++)
			{
				AssetLoaderBase loader = _loaders[i];
				loader.Destroy(true);
			}
			_loaders.Clear();

			// 释放所有资源
			Resources.UnloadUnusedAssets();
		}

		/// <summary>
		/// 从列表里获取加载器
		/// </summary>
		private static AssetLoaderBase TryGetLoader(string bundleName)
		{
			AssetLoaderBase loader = null;
			for (int i = 0; i < _loaders.Count; i++)
			{
				AssetLoaderBase temp = _loaders[i];
				if (temp.BundleInfo.BundleName.Equals(bundleName))
				{
					loader = temp;
					break;
				}
			}
			return loader;
		}

		#region 调试专属方法
		internal static List<AssetLoaderBase> GetAllLoaders()
		{
			return _loaders;
		}
		internal static int GetLoaderCount()
		{
			return _loaders.Count;
		}
		internal static int GetLoaderFailedCount()
		{
			int count = 0;
			for (int i = 0; i < _loaders.Count; i++)
			{
				AssetLoaderBase temp = _loaders[i];
				if (temp.States == ELoaderStates.Fail || temp.GetFailedProviderCount() > 0)
					count++;
			}
			return count;
		}
		#endregion

	}
}