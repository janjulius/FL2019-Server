using Shared.Constants;
using Shared.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets
{
    public struct SendMessage
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string SendingUser;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string ReceivingUser;
        public Message Message;

        public SendMessage(string sendingUser, string receivingUser, Message message)
        {
            SendingUser = sendingUser;
            ReceivingUser = receivingUser;
            Message = message;
        }
    }
}
