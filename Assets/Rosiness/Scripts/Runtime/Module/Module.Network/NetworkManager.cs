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
    /// ���������
    /// </summary>
    public sealed class NetworkManager : ModuleSingleton<NetworkManager>, IModule
    {
        /// <summary>
        /// ��Ϸģ�鴴������
        /// </summary>
        public class CreateParameters
        {
            /// <summary>
            /// ��������������
            /// </summary>
            public Type PackageCoderType;

            /// <summary>
            /// �������󳤶�
            /// </summary>
            public int PackageMaxSize = ushort.MaxValue;
        }

        private TcpClient _client;

        // GUI��ʾ����
        private string _host;
        private int _port;
        private AddressFamily _family = AddressFamily.Unknown;

        /// <summary>
        /// ��ǰ������״̬
        /// </summary>
        public ENetworkStates States { private set; get; } = ENetworkStates.Disconnect;

        /// <summary>
        /// Mono��������Ϣ���ջص�
        /// </summary>
        public Action<INetworkPackage> MonoPackageCallback;

        /// <summary>
        /// �ȸ���������Ϣ���ջص�
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

            // ��������ͻ���
            _client.Update();

            // ��ȡ�����
            // ע�⣺�������������Ͽ���δ��ȡ����������ᶪʧ
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

            // �������������Ͽ�����
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
		/// ���ӷ�����
		/// </summary>
		/// <param name="host">��ַ</param>
		/// <param name="port">�˿�</param>
		public void ConnectServer(string host, int port)
        {
            if (States == ENetworkStates.Disconnect)
            {
                States = ENetworkStates.Connecting;
                NetworkEventDispatcher.SendBeginConnectMsg();
                IPEndPoint remote = new IPEndPoint(IPAddress.Parse(host), port);
                _client.ConnectAsync(remote, OnConnectServer);

                // ��¼����
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
        /// �Ͽ�����
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
		/// ����������Ϣ
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
		/// ����ģ����������
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
