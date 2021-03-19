/****************************************************
	文件：UIPanelSetting.cs
	作者：世界和平
	日期：2021/3/19 18:13:57
	功能：Nothing
*****************************************************/

using UnityEngine;

namespace Rosiness.Editor
{
	public class UIPanelSetting : ScriptableObject
	{
		/// <summary>
		/// 精灵文件夹路径
		/// </summary>
		public string UISpriteDirectory = string.Empty;
		
		/// <summary>
		/// 图集文件夹路径
		/// </summary>
		public string UIAtlasDirectory = string.Empty;
	}
}
