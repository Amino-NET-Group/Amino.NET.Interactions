using Amino.Interactions.Attributes;
using Amino.Interactions.Objects;
using ReflectionsTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Amino.Interactions
{
    public partial class InteractionsClient
    {

        public Dictionary<string, Objects.InteractionModule> InteractionModules;
        public Queue<Objects.Interaction> InteractionQueue;

        public enum LogLevels
        {
            None = 0,
            Debug = 1,
            Warning = 2,
            Error = 3
        }

        private Amino.Client AminoClient;
        public int InteractionCooldown = 2000;
        public string InteractionPrefix = "/";
        public bool IgnoreSelf = true;
        public LogLevels LogLevel = LogLevels.None;



        public InteractionsClient(Amino.Client client)
        {
            this.AminoClient = client;
            this.InteractionQueue = new Queue<Objects.Interaction>();
            this.InteractionModules = new Dictionary<string, Objects.InteractionModule>();
            _ = Task.Run(async () => { HandleInteractionQueue(); });

        }


        public Task RegisterModule<T>() where T : InteractionBase
        {
            var moduleType = typeof(T);
            var methods = moduleType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(m => m.GetCustomAttribute<Command>() != null);

            foreach (var method in methods)
            {
                var commandAttribute = method.GetCustomAttribute<Command>();
                var enabledInDmsAttribute = method.GetCustomAttribute<EnabledInDms>();
                var permissionGroup = method.GetCustomAttribute<PermissionGroup>();
                var parameters = new FunctionAnalyzer().GetParameters(method);

                InteractionModule module = new InteractionModule();
                module.ModuleCommandName = commandAttribute.CommandName;
                if (commandAttribute.CommunityId != null) { module.ModuleCommandCommunity = Convert.ToInt32(commandAttribute.CommunityId); }
                if (commandAttribute.CommandDescription != null) { module.ModuleCommandDescription = commandAttribute.CommandDescription; }
                if (enabledInDmsAttribute != null) { module.ModuleCommandEnabledInDms = enabledInDmsAttribute.IsEnabledInDms; }
                if (permissionGroup != null) { module.ModulePermissionGroup = permissionGroup.RequiredPermission; }
                foreach (var parameter in parameters)
                {
                    module.ModuleCommandParameters.Add((parameter.Name, parameter.IsOptional));
                }

                this.InteractionModules.Add(commandAttribute.CommandName, module);
            }
            return Task.CompletedTask;
        }


        public Task RegisterModules(Assembly entrypoint)
        {
            var moduleTypes = entrypoint.GetTypes().Where(t => typeof(InteractionBase).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var moduleType in moduleTypes)
            {
                var methods = moduleType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(m => m.GetCustomAttribute<Command>() != null);

                foreach (var method in methods)
                {
                    var commandAttribute = method.GetCustomAttribute<Command>();
                    var enabledInDmsAttribute = method.GetCustomAttribute<EnabledInDms>();
                    var permissionGroup = method.GetCustomAttribute<PermissionGroup>();
                    var parameters = new FunctionAnalyzer().GetParameters(method);

                    InteractionModule module = new InteractionModule();
                    module.ModuleCommandName = commandAttribute.CommandName;
                    if(commandAttribute.CommunityId != null) { module.ModuleCommandCommunity = Convert.ToInt32(commandAttribute.CommunityId); }
                    if(commandAttribute.CommandDescription != null) { module.ModuleCommandDescription = commandAttribute.CommandDescription; }
                    if(enabledInDmsAttribute != null) { module.ModuleCommandEnabledInDms = enabledInDmsAttribute.IsEnabledInDms; }
                    if(permissionGroup != null) { module.ModulePermissionGroup = permissionGroup.RequiredPermission; }
                    foreach(var parameter in parameters)
                    {
                        module.ModuleCommandParameters.Add((parameter.Name, parameter.IsOptional));
                    }

                    this.InteractionModules.Add(commandAttribute.CommandName,module);
                }
            }
            return Task.CompletedTask;
        }
        
        public bool HandleInteraction(Objects.Interaction interaction)
        {
            return true;
        }


        private async Task HandleInteractionQueue()
        {
            while(true)
            {
                if(InteractionQueue.Count != 0)
                {
                    HandleInteraction(InteractionQueue.Dequeue());
                }
                await Task.Delay(this.InteractionCooldown);
            }
        }


    }
}
