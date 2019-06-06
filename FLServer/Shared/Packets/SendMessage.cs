using Shared.Constants;
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
        public string ReceivingUser;
        public string MessageText;
        public DateTime TimeStamp;

        public SendMessage(string receivingUser, string messageText, DateTime timeStamp)
        {
            ReceivingUser = receivingUser;  
            MessageText = messageText;
            TimeStamp = timeStamp;
        }
    }
}
