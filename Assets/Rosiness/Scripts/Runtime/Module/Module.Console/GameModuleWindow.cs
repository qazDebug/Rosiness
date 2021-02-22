/****************************************************
	文件：GameModuleWindow.cs
	作者：世界和平
	日期：2021/2/21 19:1:52
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Console
{
    [Console("游戏模块", 100)]
    internal class GameModuleWindow : IConsoleWindow
    {
		// GUI相关
		private Vector2 _scrollPos = Vector2.zero;

		void IConsoleWindow.OnGUI()
		{
			_scrollPos = ConsoleGUI.BeginScrollView(_scrollPos);
			RosinessEngine.DrawModulesGUIContent();
			ConsoleGUI.EndScrollView();
		}
	}
}