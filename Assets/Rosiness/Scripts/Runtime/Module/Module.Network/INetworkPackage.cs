/****************************************************
	文件：INetworkPackage.cs
	作者：世界和平
	日期：2021/3/4 17:11:58
	功能：Nothing
*****************************************************/

namespace Rosiness.Network
{
    public interface INetworkPackage
    {
		/// <summary>
		/// 是否为热更消息
		/// </summary>
		bool IsHotfixPackage { set; get; }
    }
}