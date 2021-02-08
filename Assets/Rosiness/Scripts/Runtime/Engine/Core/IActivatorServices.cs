/****************************************************
	文件：IActivatorServices.cs
	作者：世界和平
	日期：2021/1/23 12:26:28
	功能：Nothing
*****************************************************/
using System;

namespace Rosiness
{
    public interface IActivatorServices
    {
        /// <summary>
        /// 创建实例
        /// </summary>
        object CreateInstance(Type classType);

        /// <summary>
        /// 获取特性
        /// </summary>
        Attribute GetAttribute(Type classType);
    }
}