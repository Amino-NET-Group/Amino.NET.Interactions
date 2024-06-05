using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    /// <summary>
    /// This Attribute will determine if your Command will be availabe in Group Chats
    /// </summary>
    public class EnabledInGroupChats : Attribute
    {
        /// <summary>
        /// The value that determines if the command works in Group Chats
        /// </summary>
        public bool IsEnabledInGCs { get; }
        
        /// <summary>
        /// Allows you to set the availability of your command in Group Chats
        /// </summary>
        /// <param name="isEnabled">The value that determines if it will be enabled or not</param>
        public EnabledInGroupChats(bool isEnabled)
        {
            this.IsEnabledInGCs = isEnabled;
        }
    }
}
