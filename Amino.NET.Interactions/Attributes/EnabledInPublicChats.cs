using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    /// <summary>
    /// This Attribute will determine if your command is availabe in Public Chats or not
    /// </summary>
    public class EnabledInPublicChats : Attribute
    {
        /// <summary>
        /// The value that determines if your command is availabe in Public Chats
        /// </summary>
        public bool isEnabledInPCs { get; }

        /// <summary>
        /// The Attribute that allows you to set the availability in Public Chats
        /// </summary>
        /// <param name="isEnabled">The value that determines the availability of your command in Public Chats</param>
        public EnabledInPublicChats(bool isEnabled)
        {
            this.isEnabledInPCs = isEnabled;
        }
    }
}
