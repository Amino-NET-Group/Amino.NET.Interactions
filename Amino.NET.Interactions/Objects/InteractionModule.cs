using Amino.Interactions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions.Objects
{
    public class InteractionModule
    {
        public string ModuleCommandName { get; set; }
        public string ModuleCommandDescription { get; set; }
        public int? ModuleCommandCommunity { get; set; }
        public InteractionBase ModuleInteractionBase { get; set; }

        public bool ModuleCommandEnabledInDms { get; set; } = true;
        public PermissionGroup.PermissionGroups ModulePermissionGroup { get; set; } = PermissionGroup.PermissionGroups.All;

        public List<(string, bool)> ModuleCommandParameters { get; set; } = new List<(string, bool)>();

        public delegate Task InteractionMethodDelegate(params object[] args);

        public InteractionMethodDelegate ModuleInteractionMethod { get; set; }

    }
}
