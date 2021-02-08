/****************************************************
	文件：WindowAttribute.cs
	作者：世界和平
	日期：2021/1/23 12:20:18
	功能：Nothing
*****************************************************/
using System;

namespace Rosiness.Window
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WindowAttribute : Attribute
    {
        /// <summary>
        /// 窗口层级
        /// </summary>
        public int WindowLayer;

        /// <summary>
        /// 全屏窗口标记
        /// </summary>
        public bool FullScreen;

        public WindowAttribute(int windowLayer, bool fullScreen)
        {
            WindowLayer = windowLayer;
            FullScreen = fullScreen;
        }
    }
}