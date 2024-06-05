using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Objects
{
    public class ChatCache
    {
        public enum ChatTypes
        {
            PublicChat,
            PrivateChat,
            GroupChat
        }

        public string ChatId { get; set; }
        public ChatTypes ChatType { get; set; }
    }
}
