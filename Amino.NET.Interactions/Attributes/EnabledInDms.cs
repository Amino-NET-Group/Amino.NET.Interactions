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
        /// <summary>
        /// Determines if the command is availabe in DM chats
        /// </summary>
        public bool IsEnabledInDms { get; } = true;

        /// <summary>
        /// The Attribute to register your command as a module
        /// </summary>
        /// <param name="isEnabledInDms">Determines if the command will be availabe for DM chats</param>
        public EnabledInDms(bool isEnabledInDms)
        {
            IsEnabledInDms = isEnabledInDms;
        }
    }
}
