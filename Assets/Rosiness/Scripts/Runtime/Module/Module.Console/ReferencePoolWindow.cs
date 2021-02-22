/****************************************************
	文件：ReferencePoolWindow.cs
	作者：世界和平
	日期：2021/2/21 19:1:27
	功能：Nothing
*****************************************************/
using Rosiness.Reference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Console
{
	[ConsoleAttribute("引用池", 103)]
	internal class ReferencePoolWindow : IConsoleWindow
	{
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		void IConsoleWindow.OnGUI()
		{
			var pools = ReferencePool.GetAllCollectors;
			ConsoleGUI.Lable($"池总数：{pools.Count}");

			float offset = ConsoleGUI.LableStyle.fontSize;
			_scrollPos = ConsoleGUI.BeginScrollView(_scrollPos, offset);
			foreach (var pair in pools)
			{
				ConsoleGUI.Lable($"[{pair.Value.ClassType.FullName}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}");
			}
			ConsoleGUI.EndScrollView();
		}
	}
}