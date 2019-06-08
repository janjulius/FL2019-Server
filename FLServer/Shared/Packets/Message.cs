using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets
{
    public struct Message
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string ReceivingUser;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string MessageText;
        public double TimeStamp;

        public Message(string receivingUser, string messageText, double timeStamp)
        {
            ReceivingUser = receivingUser;
            MessageText = messageText;
            TimeStamp = timeStamp;
        }
    }

    public struct Messages
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = PacketConstants.MaxMessages)]
        public Message[] AllMessages;

        public Messages(Message[] allMessages)
        {
            AllMessages = allMessages;
        }
    }
}
