using Rosiness.Event;

namespace Rosiness.Network
{
    public class NetworkEventMessageDefine
    {
        /// <summary>
        /// ��ʼ����
        /// </summary>
        public class BeginConnect : IEventMessage
        {
        }

        /// <summary>
        /// ���ӳɹ�
        /// </summary>
        public class ConnectSuccess : IEventMessage
        {
        }

        /// <summary>
        /// ����ʧ��
        /// </summary>
        public class ConnectFail : IEventMessage
        {
            public string Error;
        }

        /// <summary>
        /// �Ͽ�����
        /// </summary>
        public class Disconnect : IEventMessage
        {
        }
    }
}
