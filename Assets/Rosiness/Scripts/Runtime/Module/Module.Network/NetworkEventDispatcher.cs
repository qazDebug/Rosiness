/****************************************************
	文件：NetworkEventDispatcher.cs
	作者：世界和平
	日期：2021/3/5 10:7:16
	功能：Nothing
*****************************************************/

using Rosiness.Event;

namespace Rosiness.Network
{
    internal static class NetworkEventDispatcher
    {
        public static void SendBeginConnectMsg()
        {
            NetworkEventMessageDefine.BeginConnect msg = new NetworkEventMessageDefine.BeginConnect();
            EventManager.Instance.SendMessage(msg);
        }

        public static void SendConnectSuccessMsg()
        {
            NetworkEventMessageDefine.ConnectSuccess msg = new NetworkEventMessageDefine.ConnectSuccess();
            EventManager.Instance.SendMessage(msg);
        }

        public static void SendConnectFailMsg(string error)
        {
            NetworkEventMessageDefine.ConnectFail msg = new NetworkEventMessageDefine.ConnectFail();
            msg.Error = error;
            EventManager.Instance.SendMessage(msg);
        }

        public static void SendDisconnectMsg()
        {
            NetworkEventMessageDefine.Disconnect msg = new NetworkEventMessageDefine.Disconnect();
            EventManager.Instance.SendMessage(msg);
        }
    }
}