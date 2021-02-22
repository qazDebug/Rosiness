/****************************************************
	文件：IFsmNode.cs
	作者：世界和平
	日期：2021/2/21 17:29:53
	功能：Nothing
*****************************************************/

namespace Rosiness.AI
{
    public interface IFsmNode
    {
        /// <summary>
        /// 节点名字
        /// </summary>
        string Name { get; }

        void OnEnter();
        void OnUpdate();
        void OnExit();
        void OnHandleMessage(object msg);
    }
}