using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    public class Command : Attribute
    {
        public string CommandName { get; }
        public string CommandDescription { get; }
        public string CommunityId { get; }

        public Command(string commandName, string commandDescription = null, string communityId = null)
        {
            this.CommandName = commandName;
            this.CommunityId = communityId;
            this.CommandDescription = commandDescription;
        }


    }
}
