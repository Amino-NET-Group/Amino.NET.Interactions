using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    public class EnabledInDms : Attribute
    {
        public bool IsEnabledInDms { get; } = true;

        public EnabledInDms(bool isEnabledInDms)
        {
            IsEnabledInDms = isEnabledInDms;
        }
    }
}
