using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    public class EnabledInPublicChats : Attribute
    {
        public bool isEnabledInPCs { get; }

        public EnabledInPublicChats(bool isEnabled)
        {
            this.isEnabledInPCs = isEnabled;
        }
    }
}
