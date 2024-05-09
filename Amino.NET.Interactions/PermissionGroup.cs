using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
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
