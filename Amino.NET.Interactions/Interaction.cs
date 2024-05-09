using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Objects
{
    public class Interaction
    {
        public string? InteractionChatId { get; set; }
        public string? InteractionName { get; set; }
        public Amino.Client AminoClient { get; set; }
        public Amino.Objects.Message? Message { get; set; }
        public long InteractionTimestamp { get; set; }
        public string? InteractionId { get; set; }
        public List<string> InteractionParameters { get; set; } = new List<string>();
        public InteractionModule InteractionBaseModule { get; set; }

    }
}
