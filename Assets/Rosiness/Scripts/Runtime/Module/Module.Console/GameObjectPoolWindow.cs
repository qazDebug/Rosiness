/****************************************************
	文件：GameObjectPoolWindow.cs
	作者：世界和平
	日期：2021/2/21 19:1:36
	功能：Nothing
*****************************************************/
using Rosiness.Pool;
using Rosiness.Resource;
using UnityEngine;

namespace Rosiness.Console
{
    [Console("游戏对象池",105)]
    internal class GameObjectPoolWindow : IConsoleWindow
    {
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		void IConsoleWindow.OnGUI()
		{
			// 如果游戏模块没有创建
			if (RosinessEngine.Contains(typeof(GameObjectPoolManager)) == false)
			{
				ConsoleGUI.YellowLable($"{nameof(GameObjectPoolManager)} is not create.");
				return;
			}

			var pools = GameObjectPoolManager.Instance.GetAllCollectors;
			ConsoleGUI.Lable($"池总数：{pools.Count}");

			float offset = ConsoleGUI.LableStyle.fontSize;
			_scrollPos = ConsoleGUI.BeginScrollView(_scrollPos, offset);
			foreach (var pair in pools)
			{
				string content = $"[{pair.Value.Location}] CacheCount = {pair.Value.Count} SpwanCount = {pair.Value.SpawnCount}";
				if (pair.Value.States == EAssetStates.Fail)
					ConsoleGUI.RedLable(content);
				else
					ConsoleGUI.Lable(content);
			}
			ConsoleGUI.EndScrollView();
		}
	}
}