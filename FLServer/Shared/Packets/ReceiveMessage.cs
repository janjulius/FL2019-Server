using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets
{
    public struct ReceiveMessage
    {
        public string MessageText;
        public DateTime TimeStamp;

        public ReceiveMessage(string messageText, DateTime timeStamp)
        {
            MessageText = messageText;
            TimeStamp = timeStamp;
        }
    }
}
