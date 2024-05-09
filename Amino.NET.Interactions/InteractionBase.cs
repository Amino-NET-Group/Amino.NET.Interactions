using Amino.Interactions.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions
{
    public class InteractionBase
    {


        public Task Respond(Interaction context, string message, bool asReply)
        {
            return Task.CompletedTask;
        }

    }
}
