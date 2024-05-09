using Amino.Interactions.Attributes;
using Amino.Interactions.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

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
        public bool AutoHandleInteractions { get; } = false;


        public event Action<Interaction> InteractionCreated;
        public event Action<LogMessage> Log;


        public InteractionsClient(Amino.Client client)
        {
            this.AminoClient = client;
            
            this.InteractionModules = new Dictionary<string, Objects.InteractionModule>();

            if(AutoHandleInteractions)
            {
                this.InteractionQueue = new Queue<Objects.Interaction>();
                _ = Task.Run(async () => { HandleInteractionQueue(); });
            }
            
            AminoClient.onMessage += HandleMessageSocket;

        }


        private void HandleMessageSocket(Amino.Objects.Message message)
        {
            Console.WriteLine(message.content);
            if (message.content.StartsWith(InteractionPrefix))
            {
                if (InteractionModules.ContainsKey(message.content.Substring(InteractionPrefix.Length).Split(" ")[0]))
                {
                    InteractionModule module = InteractionModules[message.content.Substring(InteractionPrefix.Length).Split(" ")[0]];
                    Interaction context = new Interaction();
                    context.Message = message;
                    context.InteractionParameters.AddRange(message.content.Split(" ")[1..]);
                    context.InteractionChatId = message.chatId;
                    context.AminoClient = this.AminoClient;
                    context.InteractionId = Guid.NewGuid().ToString();
                    context.InteractionName = module.ModuleCommandName;
                    context.InteractionTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    context.InteractionBaseModule = module;

                    if(this.InteractionCreated != null) { this.InteractionCreated.Invoke(context); }

                }
            }
        }


        public Task RegisterModule<T>() where T : InteractionBase
        {
            Type moduleType = typeof(T);

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

                module.ModuleInteractionMethod = async (args) =>
                {
                    var finalArgs = new object[method.GetParameters().Length];

                    for (int i = 0; i < args.Length; i++)
                    {
                        finalArgs[i] = args[i];
                    }

                    for (int i = args.Length; i < finalArgs.Length; i++)
                    {
                        var parameter = method.GetParameters()[i];
                        finalArgs[i] = parameter.DefaultValue;
                    }

                    await (Task)method.Invoke(Activator.CreateInstance(moduleType), finalArgs);
                };

                InteractionBase moduleInstance = Activator.CreateInstance<T>();
                module.ModuleInteractionBase = moduleInstance;

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

                    module.ModuleInteractionMethod = async (args) =>
                    {
                        var finalArgs = new object[method.GetParameters().Length];

                        for (int i = 0; i < args.Length; i++)
                        {
                            finalArgs[i] = args[i];
                        }

                        for (int i = args.Length; i < finalArgs.Length; i++)
                        {
                            var parameter = method.GetParameters()[i];
                            finalArgs[i] = parameter.DefaultValue;
                        }

                        await (Task)method.Invoke(Activator.CreateInstance(moduleType), finalArgs);
                    };

                    
                    InteractionBase moduleInstance = (InteractionBase)Activator.CreateInstance(moduleType);
                    if (typeof(InteractionBase).IsAssignableFrom(moduleType))
                    {
                        module.ModuleInteractionBase = moduleInstance;
                    } 

                    

                    this.InteractionModules.Add(commandAttribute.CommandName,module);
                }
            }
            return Task.CompletedTask;
        }
        
        public void HandleInteraction(Objects.Interaction interactionContext)
        {
            List<object> args = new List<object>() { interactionContext };

            for(int i = 1; i < interactionContext.InteractionBaseModule.ModuleCommandParameters.Count; i++)
            {
                if (i > interactionContext.InteractionParameters.Count) { break; }
                var interactionParameter = interactionContext.InteractionParameters[i - 1];
                var moduleParameter = interactionContext.InteractionBaseModule.ModuleCommandParameters[i];

                switch (moduleParameter.Item1.ToLower())
                {
                    case "string":
                        args.Add(interactionParameter);
                        break;
                    case "string[]":
                        string[] textParams = new string[] { string.Join(" ", interactionContext.InteractionParameters.Skip(i -1)) };
                        args.Add(textParams);
                        break;
                }
            }
            interactionContext.InteractionBaseModule.ModuleInteractionMethod.Invoke(args.ToArray());
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
