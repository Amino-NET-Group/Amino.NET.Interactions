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
        public List<PermissionGroup.PermissionGroups> Permissions { get; set; } = new List<PermissionGroup.PermissionGroups>() { PermissionGroup.PermissionGroups.All };
    }
}
