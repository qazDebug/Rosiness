/****************************************************
	文件：AssetSystemWindow.cs
	作者：世界和平
	日期：2020/12/1 15:7:37
	功能：Nothing
*****************************************************/
using Rosiness.Reference;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Console
{
    [Console("资源系统", 102)]
    internal class AssetSystemWindow : IConsoleWindow
    {
		private class InfoWrapper : IReference, IComparer<InfoWrapper>, IComparable<InfoWrapper>
		{
			public string Info;
			//public ELoaderStates LoadState;
			public int ProviderFailedCount;

			public void OnRelease()
			{
				Info = string.Empty;
				//LoadState = ELoaderStates.None;
				ProviderFailedCount = 0;
			}
			public int CompareTo(InfoWrapper other)
			{
				return Compare(this, other);
			}
			public int Compare(InfoWrapper a, InfoWrapper b)
			{
				return string.CompareOrdinal(a.Info, b.Info);
			}
		}

		void IConsoleWindow.OnGUI()
        {
            
        }        
    }
}