using Amino.Interactions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Objects
{
    public class UserCache
    {
        public string UserId { get; set; }
        public PermissionGroup.PermissionGroups PermissionLevel { get; set; } = PermissionGroup.PermissionGroups.All;
        public PermissionGroup.PermissionGroups AdditionalPermissionLevel { get; set; } = PermissionGroup.PermissionGroups.All;
    }
}
