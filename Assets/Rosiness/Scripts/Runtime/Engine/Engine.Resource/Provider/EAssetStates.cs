/****************************************************
	文件：EAssetStates.cs
	作者：世界和平
	日期：2020/12/4 17:55:6
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Resource
{
    /// <summary>
    /// 资源加载状态
    /// </summary>
    public enum EAssetStates
    {
        None = 0,
        Loading,
        Checking,
        Success,
        Fail,
    }
}