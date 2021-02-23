/****************************************************
	文件：DLCManager.cs
	作者：世界和平
	日期：2021/2/23 10:2:15
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rosiness.Editor
{
    public class DLCManager
    {
		private readonly List<DLContent> _contents = new List<DLContent>();

		/// <summary>
		/// 加载所有DLC文件
		/// </summary>
		public void LoadAllDLC()
		{
			string[] files = AssetBundleCollectorSettingData.GetDLCFiles();
			for (int i = 0; i < files.Length; i++)
			{
				DLContent dlc = DLContent.Deserialize(files[i]);
				if (dlc != null)
					_contents.Add(dlc);
			}
		}

		/// <summary>
		/// 获取AssetBundle的所有DLC标签
		/// </summary>
		public string[] GetAssetBundleDLCLabels(string bundleName)
		{
			List<string> labels = new List<string>();
			foreach (var dlc in _contents)
			{
				if (dlc.IsContains(bundleName) == false)
					continue;

				if (string.IsNullOrEmpty(dlc.DefaultLabel) == false)
				{
					if (labels.Contains(dlc.DefaultLabel) == false)
						labels.Add(dlc.DefaultLabel);
				}

				string label = dlc.GetAssetBundleDLCLabel(bundleName);
				if (string.IsNullOrEmpty(label) == false)
				{
					if (labels.Contains(label) == false)
						labels.Add(label);
				}
			}
			return labels.ToArray<string>();
		}
	}
}