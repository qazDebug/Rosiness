/****************************************************
	文件：AssetImporterSetting.cs
	作者：世界和平
	日期：2021/2/25 15:33:47
	功能：Nothing
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Editor
{
    [CreateAssetMenu]
    public class AssetImporterSetting : ScriptableObject
	{
		[Serializable]
		public class Wrapper
		{
			public string ProcessDirectory = string.Empty;
			public string ProcessorName = string.Empty;
		}

		/// <summary>
		/// 开关
		/// </summary>
		public bool Toggle = true;

		/// <summary>
		/// 路径列表
		/// </summary>
		public List<Wrapper> Elements = new List<Wrapper>();
	}
}
