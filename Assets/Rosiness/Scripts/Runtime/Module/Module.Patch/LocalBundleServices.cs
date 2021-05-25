/****************************************************
	文件：LocalBundleServices.cs
	作者：世界和平
	日期：2021/2/22 10:16:16
	功能：Nothing
*****************************************************/
using Rosiness.Network;
using Rosiness.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Patch
{
    public sealed class LocalBundleServices : IBundleServices
    {

        /// <summary>
        /// 适合单机游戏的资源文件服务接口类
        /// </summary>
        public LocalBundleServices()
		{
		}

        /// <summary>
        /// 异步初始化
        /// </summary>
        public IEnumerator InitializeAsync(bool simulationOnEditor)
		{
			if (simulationOnEditor)
				yield break;
        }

		bool IBundleServices.CheckContentIntegrity(string bundleName)
		{
			throw new NotImplementedException();
		}

		public AssetBundleInfo GetAssetBundleInfo(string bundleName)
		{
			throw new NotImplementedException();
		}


		public string GetAssetBundleName(string assetPath)
		{
			throw new NotImplementedException();
		}

		public string[] GetDirectDependencies(string bundleName)
		{
			throw new NotImplementedException();
		}

		public string[] GetAllDependencies(string bundleName)
		{
			throw new NotImplementedException();
		}
    }
}