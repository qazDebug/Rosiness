/****************************************************
	文件：AssetBundleInfo.cs
	作者：世界和平
	日期：2020/12/4 9:26:35
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Resource
{
    public class AssetBundleInfo
    {
		/// <summary>
		/// 资源包名称
		/// </summary>
		public string BundleName { private set; get; }

		/// <summary>
		/// 本地存储的路径
		/// </summary>
		public string LocalPath { private set; get; }

		/// <summary>
		/// 远端下载的路径
		/// </summary>
		public string RemoteURL { private set; get; }

		/// <summary>
		/// 资源版本
		/// </summary>
		public int Version { private set; get; }

		/// <summary>
		/// 是否为加密文件
		/// </summary>
		public bool IsEncrypted { private set; get; }

        public AssetBundleInfo(string bundleName, string localPath, string remoteURL, int version, bool isEncrypted)
        {
            BundleName = bundleName;
            LocalPath = localPath;
            RemoteURL = remoteURL;
            Version = version;
            IsEncrypted = isEncrypted;
        }

        public AssetBundleInfo(string bundleName, string localPath)
        {
            BundleName = bundleName;
            LocalPath = localPath;
			RemoteURL = string.Empty;
			Version = 0;
			IsEncrypted = false;
		}
    }
}