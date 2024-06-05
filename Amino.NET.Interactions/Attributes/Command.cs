using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    /// <summary>
    /// This Attribute defines a Module to be a command
    /// </summary>
    public class Command : Attribute
    {
        /// <summary>
        /// The name of your command, please see <see href="https://github.com/Amino-NET-Group/Amino.NET.Interactions">the official Github page</see>  for rules and restictions
        /// </summary>
        public string CommandName { get; }
        /// <summary>
        /// A description you can give to your command, you can use this to make a help command for example
        /// </summary>
        public string CommandDescription { get; }
        /// <summary>
        /// If set, you will lock your command to work on the specified Community only
        /// </summary>
        public string CommunityId { get; }

        /// <summary>
        /// The Attribute that registers a Command as a module
        /// </summary>
        /// <param name="commandName">The name of the command</param>
        /// <param name="commandDescription">The description of your command</param>
        /// <param name="communityId">The community the command is locked to</param>
        public Command(string commandName, string commandDescription = null, string communityId = null)
        {
            this.CommandName = commandName;
            this.CommunityId = communityId;
            this.CommandDescription = commandDescription;
        }


    }
}
