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
        public enum PermissionGroups
        {
            All = 0,
            Chat_Staff = 1,
            Staff = 2,
            Curator = 3,
            Leader = 4,
            Agent = 5
        }

        public PermissionGroups RequiredPermission { get; set; } = PermissionGroups.All;

        public PermissionGroup(PermissionGroups permissionGroup)
        {
            this.RequiredPermission = permissionGroup;
        }
    }
}
