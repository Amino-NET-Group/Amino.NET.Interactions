using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    /// <summary>
    /// With this Attribute you can determine what permission level is required in order for a user to be able to use your module
    /// </summary>
    /// <remarks>Note: Using this attribute will not have any effect on your program as it is not fully implemented yet. You can still have it and update your project dependencies later for this take effect.</remarks>
    public class PermissionGroup : Attribute
    {
        /// <summary>
        /// A list of Permissions that can be set for a command
        /// </summary>
        public enum PermissionGroups
        {
            /// <summary>
            /// The <see cref="All"/> PermissionGroup includes all users no matter the rank or role
            /// </summary>
            All = 0,
            /// <summary>
            /// The <see cref="Chat_Staff"/> PermissionGroup includes all users that are part of a chat staff for example Host and Co-Host
            /// </summary>
            Chat_Staff = 1,
            /// <summary>
            /// The <see cref="Staff"/> PermissionGroup includes all users that are part of a Commununity Staff, for example Curator, Leader and Agent
            /// </summary>
            Staff = 2,
            /// <summary>
            /// The <see cref="Curator"/> PermissionGroup includes all Curators (and higher) of a Community
            /// </summary>
            Curator = 3,
            /// <summary>
            /// The <see cref="Leader"/> PermissionGroup includes all Leaders (and higher) of a Community
            /// </summary>
            Leader = 4,
            /// <summary>
            /// The <see cref="Agent"/> PermissionGroup includes the Agent of a Community
            /// </summary>
            Agent = 5
        }

        public PermissionGroups RequiredPermission { get; set; } = PermissionGroups.All;

        public PermissionGroup(PermissionGroups permissionGroup)
        {
            this.RequiredPermission = permissionGroup;
        }
    }
}
