using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    /// <summary>
    /// This Attribute defines if a Module can be used in DMs and or DM Groups
    /// </summary>
    /// <remarks>Note: Using this attribute will not have any effect on your program as it is not fully implemented yet. You can still have it and update your project dependencies later for this take effect.</remarks>
    public class EnabledInDms : Attribute
    {
        public bool IsEnabledInDms { get; } = true;

        public EnabledInDms(bool isEnabledInDms)
        {
            IsEnabledInDms = isEnabledInDms;
        }
    }
}
