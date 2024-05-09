using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Objects
{
    public class LogMessage
    {
        public string Message { get; set; }
        public long Timestamp { get; set; }
        public InteractionsClient.LogLevels LogLevel { get; set; }

    }
}
