/****************************************************
	文件：DefaultNetworkPackage.cs
	作者：世界和平
	日期：2021/3/4 17:10:31
	功能：Nothing
*****************************************************/

namespace Rosiness.Network
{
    public class DefaultNetworkPackage: INetworkPackage
    {
        public bool IsHotfixPackage { get; set; }

		/// <summary>
		/// 消息ID
		/// </summary>
		public int MsgID { set; get; }

		/// <summary>
		/// 消息对象
		/// </summary>
		public object MsgObj { set; get; }

		/// <summary>
		/// 包体数据
		/// </summary>
		public byte[] BodyBytes { set; get; }
	}
}