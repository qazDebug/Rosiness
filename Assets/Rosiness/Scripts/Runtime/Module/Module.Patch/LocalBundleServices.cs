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
        private VariantCollector _variantCollector;
        private PatchManifest _patchManifest;

        /// <summary>
        /// 适合单机游戏的资源文件服务接口类
        /// </summary>
        public LocalBundleServices()
		{
		}

        /// <summary>
        /// 适合单机游戏的资源文件服务接口类
        /// </summary>
        /// <param name="variantRules">变体规则</param>
        public LocalBundleServices(List<VariantRule> variantRules)
        {
            if (variantRules == null)
                throw new ArgumentNullException();

            _variantCollector = new VariantCollector();
            foreach (var variantRule in variantRules)
            {
                _variantCollector.RegisterVariantRule(variantRule.VariantGroup, variantRule.TargetVariant);
            }
        }

        /// <summary>
        /// 异步初始化
        /// </summary>
        public IEnumerator InitializeAsync(bool simulationOnEditor)
		{
			if (simulationOnEditor)
				yield break;

            // 解析APP里的补丁清单
            string filePath = AssetPathHelper.MakeStreamingLoadPath(PatchDefine.PatchManifestFileName);
            string url = AssetPathHelper.ConvertToWWWPath(filePath);
            WebGetRequest downloader = new WebGetRequest(url);
            downloader.DownLoad();
            yield return downloader;

            if (downloader.HasError())
            {
                downloader.ReportError();
                downloader.Dispose();
                throw new System.Exception($"Fatal error : Failed download file : {url}");
            }

            _patchManifest = PatchManifest.Deserialize(downloader.GetText());
            downloader.Dispose();
        }

		#region IBundleServices接口
		bool IBundleServices.CheckContentIntegrity(string bundleName)
		{
			throw new NotImplementedException();
		}
		AssetBundleInfo IBundleServices.GetAssetBundleInfo(string bundleName)
		{
            if (_variantCollector != null)
                bundleName = _variantCollector.RemapVariantName(_patchManifest, bundleName);

            if (_patchManifest.Elements.TryGetValue(bundleName, out PatchElement element))
            {
                // 直接从沙盒里加载
                string localPath = AssetPathHelper.MakeStreamingLoadPath(element.MD5);
                AssetBundleInfo bundleInfo = new AssetBundleInfo(bundleName, localPath, string.Empty, element.Version, element.IsEncrypted);
                return bundleInfo;
            }
            else
            {
                RosinessLog.Warning($"Not found element in patch manifest : {bundleName}");
                AssetBundleInfo bundleInfo = new AssetBundleInfo(bundleName, string.Empty);
                return bundleInfo;
            }
		}
		string IBundleServices.GetAssetBundleName(string assetPath)
		{
			return _patchManifest.GetAssetBundleName(assetPath);
		}
		string[] IBundleServices.GetDirectDependencies(string bundleName)
		{
            return _patchManifest.GetDirectDependencies(bundleName);
		}
		string[] IBundleServices.GetAllDependencies(string bundleName)
		{
			return _patchManifest.GetAllDependencies(bundleName);
		}
		#endregion
	}
}