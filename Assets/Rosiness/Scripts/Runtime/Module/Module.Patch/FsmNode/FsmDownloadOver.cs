/****************************************************
	文件：FsmDownloadOver.cs
	作者：世界和平
	日期：2021/3/19 18:59:34
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rosiness.AI;

namespace Rosiness.Patch
{
    internal class FsmDownloadOver : IFsmNode
    {
		private readonly PatchManagerImpl _patcher;
		public string Name { private set; get; }

		public FsmDownloadOver(PatchManagerImpl patcher)
		{
			_patcher = patcher;
			Name = EPatchStates.DownloadOver.ToString();
		}

        public void OnEnter()
        {
            PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.DownloadOver);
        }

        public void OnUpdate()
        {
        }

        public void OnExit()
        {
        }

        public void OnHandleMessage(object msg)
        {
        }
    }
}