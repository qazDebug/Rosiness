using System;

namespace Rosiness.Network
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NetworkMessageAttribute : Attribute
    {
        public int MsgID;

        public NetworkMessageAttribute(int msgID)
        {
            MsgID = msgID;
        }
    }
}
