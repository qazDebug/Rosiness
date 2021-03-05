using Rosiness.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Rosiness.Network
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public sealed class NetworkManager : ModuleSingleton<NetworkManager>, IModule
    {
        /// <summary>
        /// 游戏模块创建参数
        /// </summary>
        public class CreateParameters
        {
            /// <summary>
            /// 网络包编码解码器
            /// </summary>
            public Type PackageCoderType;

            /// <summary>
            /// 网络包最大长度
            /// </summary>
            public int PackageMaxSize = ushort.MaxValue;
        }

        private TcpClient _client;

        // GUI显示数据
        private string _host;
        private int _port;
        private AddressFamily _family = AddressFamily.Unknown;

        /// <summary>
        /// 当前的网络状态
        /// </summary>
        public ENetworkStates States { private set; get; } = ENetworkStates.Disconnect;

        /// <summary>
        /// Mono层网络消息接收回调
        /// </summary>
        public Action<INetworkPackage> MonoPackageCallback;

        /// <summary>
        /// 热更层网络消息接收回调
        /// </summary>
        public Action<INetworkPackage> HotfixPackageCallback;


        void IModule.OnCreate(object param)
        {
            CreateParameters createParam = param as CreateParameters;
            if (createParam == null)
                throw new Exception($"{nameof(NetworkManager)} create param is invalid.");

            _client = new TcpClient(createParam.PackageCoderType, createParam.PackageMaxSize);
        }

        void IModule.OnGUI()
        {
            ConsoleGUI.Lable($"[{nameof(NetworkManager)}] State : {States}");
            ConsoleGUI.Lable($"[{nameof(NetworkManager)}] IP Host : {_host}");
            ConsoleGUI.Lable($"[{nameof(NetworkManager)}] IP Port : {_port}");
            ConsoleGUI.Lable($"[{nameof(NetworkManager)}] IP Type : {_family}");
        }

        void IModule.OnUpdate()
        {
            if (_client == null)
                return;

            // 更新网络客户端
            _client.Update();

            // 拉取网络包
            // 注意：如果服务器意外断开，未拉取的网络包将会丢失
            while (true)
            {
                INetworkPackage package = (INetworkPackage)_client.PickPackage();
                if (package == null)
                    break;
                if (package.IsHotfixPackage)
                    HotfixPackageCallback.Invoke(package);
                else
                    MonoPackageCallback.Invoke(package);
            }

            // 侦测服务器主动断开连接
            if (States == ENetworkStates.Connected)
            {
                if (_client.IsConnected() == false)
                {
                    States = ENetworkStates.Disconnect;
                    NetworkEventDispatcher.SendDisconnectMsg();
                    CloseClient();
                    RosinessLog.Warning("Server disconnect.");
                }
            }
        }

        /// <summary>
		/// 连接服务器
		/// </summary>
		/// <param name="host">地址</param>
		/// <param name="port">端口</param>
		public void ConnectServer(string host, int port)
        {
            if (States == ENetworkStates.Disconnect)
            {
                States = ENetworkStates.Connecting;
                NetworkEventDispatcher.SendBeginConnectMsg();
                IPEndPoint remote = new IPEndPoint(IPAddress.Parse(host), port);
                _client.ConnectAsync(remote, OnConnectServer);

                // 记录数据
                _host = host;
                _port = port;
                _family = remote.AddressFamily;
            }
        }

        private void OnConnectServer(SocketError error)
        {
            RosinessLog.Log($"Server connect result : {error}");
            if (error == SocketError.Success)
            {
                States = ENetworkStates.Connected;
                NetworkEventDispatcher.SendConnectSuccessMsg();
            }
            else
            {
                States = ENetworkStates.Disconnect;
                NetworkEventDispatcher.SendConnectFailMsg(error.ToString());
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisconnectServer()
        {
            if (States == ENetworkStates.Connected)
            {
                States = ENetworkStates.Disconnect;
                NetworkEventDispatcher.SendDisconnectMsg();
                CloseClient();
            }
        }

        /// <summary>
		/// 发送网络消息
		/// </summary>
		public void SendMessage(INetworkPackage package)
        {
            if (States != ENetworkStates.Connected)
            {
                RosinessLog.Warning("Network is not connected.");
                return;
            }

            if (_client != null)
                _client.SendPackage(package);
        }

        /// <summary>
		/// 本地模拟接收网络包
		/// </summary>
		public void Simulate(INetworkPackage package)
        {
            if (package != null)
            {
                if (package.IsHotfixPackage)
                    HotfixPackageCallback.Invoke(package);
                else
                    MonoPackageCallback.Invoke(package);
            }
        }

        private void CloseClient()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
    }
}
