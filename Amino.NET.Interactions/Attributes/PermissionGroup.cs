using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Attributes
{
    public class PermissionGroup
    {
        public enum PermissionGroups
        {
            All,
            Chat_Staff,
            Staff,
            Curator,
            Leader,
            Agent
        }

        public PermissionGroups RequiredPermission { get; set; } = PermissionGroups.All;

        public PermissionGroup(PermissionGroups permissionGroup)
        {
            this.RequiredPermission = permissionGroup;
        }
    }
}
