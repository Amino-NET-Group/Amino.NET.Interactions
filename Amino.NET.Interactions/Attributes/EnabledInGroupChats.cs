using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    public class EnabledInGroupChats : Attribute
    {

        public bool IsEnabledInGCs { get; }
        
        public EnabledInGroupChats(bool isEnabled)
        {
            this.IsEnabledInGCs = isEnabled;
        }
    }
}
