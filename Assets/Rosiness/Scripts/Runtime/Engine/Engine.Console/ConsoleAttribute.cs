/****************************************************
	文件：ConsoleAttribute.cs
	作者：世界和平
	日期：2020/11/28 10:1:26
	功能：Nothing
*****************************************************/
using System;

namespace Rosiness.Console
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConsoleAttribute: Attribute
    {
		/// <summary>
		/// 标题名称
		/// </summary>
		public string Title;

		/// <summary>
		/// 显示顺序
		/// </summary>
		public int Order;

		public ConsoleAttribute(string title, int order)
		{
			Title = title;
			Order = order;
		}

	}
}