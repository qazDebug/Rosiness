/****************************************************
	文件：FsmPatchDone.cs
	作者：世界和平
	日期：2021/3/25 14:54:45
	功能：Nothing
*****************************************************/
using Rosiness.AI;

namespace Rosiness.Patch
{
    internal class FsmPatchDone : IFsmNode
    {
		public string Name { private set; get; }

		public FsmPatchDone()
		{
			Name = EPatchStates.PatchDone.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.PatchDone);
		}
		void IFsmNode.OnUpdate()
		{
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}
	}
}