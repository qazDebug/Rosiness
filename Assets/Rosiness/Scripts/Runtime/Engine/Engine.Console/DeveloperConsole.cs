/****************************************************
	文件：DeveloperConsole.cs
	作者：世界和平
	日期：2020/11/27 11:3:25
	功能：控制台
*****************************************************/
using Rosiness.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Console
{
    public static class DeveloperConsole
    {
        private class WindowWrapper : IComparer<WindowWrapper>, IComparable<WindowWrapper>
        {
            public Type ClassType;
            public string Title;
            public int Priority;
            public IConsoleWindow Instance;

            public int Compare(WindowWrapper x, WindowWrapper y)
            {
                return x.Priority.CompareTo(y.Priority);
            }

            public int CompareTo(WindowWrapper other)
            {
                return Compare(this, other);
            }
        }

        /// <summary>
        /// 控制台节点列表
        /// </summary>
        private readonly static List<WindowWrapper> _wrappers = new List<WindowWrapper>();

        // GUI相关
        private static bool _visible = false;
        private static int _showIndex = 0;
        private static Texture _bgTexture;
        private static string[] _toolbarTitles;

        /// <summary>
		/// 初始化控制台
		/// </summary>
		/// <param name="assemblyName">扩展的控制台窗口所在的程序集</param>
        public static void Initialize(string assemblyName = AssemblyUtility.UnityDefaultAssemblyName)
        {
            string textureName = "console_background";
            _bgTexture = Resources.Load<Texture>(textureName);

            if(_bgTexture == null) 
                Debug.LogWarning($"Not found {textureName} texture in Resources folder.");

            List<Type> types = AssemblyUtility.GetAssignableAttributeTypes(AssemblyUtility.RosinessAssemblyName,typeof(IConsoleWindow), typeof(ConsoleAttribute));
            List<Type> temps = AssemblyUtility.GetAssignableAttributeTypes(assemblyName, typeof(IConsoleWindow), typeof(ConsoleAttribute));
            types.AddRange(temps);
            for (int i = 0; i < types.Count; i++)
            {
                ConsoleAttribute attribute = (ConsoleAttribute)Attribute.GetCustomAttribute(types[i], typeof(ConsoleAttribute));
                WindowWrapper wrapper = new WindowWrapper()
                {
                    ClassType = types[i],
                    Title = attribute.Title,
                    Priority = attribute.Order,
                };
                _wrappers.Add(wrapper);
            }

            _wrappers.Sort();

            // 创建实例类
            for (int i = 0; i < _wrappers.Count; i++)
            {
                WindowWrapper wrapper = _wrappers[i];
                wrapper.Instance = (IConsoleWindow)Activator.CreateInstance(wrapper.ClassType);
            }

            // 标题列表
            List<string> titles = new List<string>();
            for (int i = 0; i < _wrappers.Count; i++)
            {
                titles.Add(_wrappers[i].Title);
            }
            _toolbarTitles = titles.ToArray();
        }

        /// <summary>
		/// 绘制控制台
		/// 注意：该接口必须在OnGUI函数内调用
		/// </summary>
        public static void Draw()
        {
            ConsoleGUI.InitGlobalStyle();

            float posX = Screen.safeArea.x;
            float posY = Screen.height - Screen.safeArea.height - Screen.safeArea.y;

            if (_visible == false)
            {
                // 显示按钮
                if (GUI.Button(new Rect(posX + 10, posY + 10, ConsoleGUI.XStyle.fixedWidth, ConsoleGUI.XStyle.fixedHeight), "R", ConsoleGUI.XStyle))
                    _visible = true;
            }
            else
            {
                Rect windowRect = new Rect(posX, posY, Screen.safeArea.width, Screen.safeArea.height);
                GUI.Window(0, windowRect, DrawWindow, string.Empty);
            }
        }

        private static void DrawWindow(int windowID)
        {
            // 绘制背景
            if (_visible && _bgTexture != null)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _bgTexture, ScaleMode.StretchToFill, true);

            GUILayout.BeginHorizontal();
            {
                // 隐藏按钮
                if (GUILayout.Button("R", ConsoleGUI.ButtonStyle, GUILayout.Width(ConsoleGUI.ButtonStyle.fixedHeight)))
                    _visible = false;

                // 绘制按钮栏
                _showIndex = GUILayout.Toolbar(_showIndex, _toolbarTitles, ConsoleGUI.ToolbarStyle);
            }
            GUILayout.EndHorizontal();

            // 绘制选中窗口
            for (int i = 0; i < _wrappers.Count; i++)
            {
                if (_showIndex != i)
                    continue;
                WindowWrapper wrapper = _wrappers[i];
                wrapper.Instance.OnGUI();
            }
        }
    }
}