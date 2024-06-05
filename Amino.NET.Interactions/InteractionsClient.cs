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
using static System.Net.WebRequestMethods;

namespace Amino.Interactions
{
    public partial class InteractionsClient
    {

        /// <summary>
        /// A Dictionary of all currently registered Modules, stored as: commandName:InteactionModule
        /// </summary>
        public Dictionary<string, Objects.InteractionModule> InteractionModules;
        /// <summary>
        /// A Queue for all currently Queued up interactions, this is only useful if <see cref="AutoHandleInteractions"/> is checked
        /// </summary>
        public Queue<Objects.Interaction> InteractionQueue;

        /// <summary>
        /// The cache Dictionary for Chats, you can use this to get and modify the current cache, it has the following format: <b>ChatId:<see cref="ChatCache"/></b>
        /// </summary>
        public Dictionary<string, ChatCache> InteractionChatCache = new Dictionary<string, ChatCache>();
        /// <summary>
        /// The cache Dictionary for Users and permissions, you can use this to get and modify the current cache, it has the following format: <b>UserId:<see cref="UserCache"/></b>
        /// </summary>
        public Dictionary<string, UserCache> InteractionUserCache = new Dictionary<string, UserCache>();

        /// <summary>
        /// An enum type of all LogLevels you can choose for you InteractionsClient
        /// </summary>
        public enum LogLevels
        {
            /// <summary>
            /// Indicates that no Logging is being done
            /// </summary>
            None = 0,
            /// <summary>
            /// Indicates that you want to receive all log events
            /// </summary>
            Debug = 1,
            /// <summary>
            /// Indicates that you want to receive only Warnings
            /// </summary>
            Warning = 2,
            /// <summary>
            /// Indicates that you want to receive only Errors
            /// </summary>
            Error = 3
        }

        private Amino.Client AminoClient;

        /// <summary>
        /// The cooldown for your <see cref="InteractionQueue"/>, only useful if <see cref="AutoHandleInteractions"/> is checked
        /// </summary>
        /// <remarks>Note: You can currently not edit the property as the Automatic interaction queue is not implemented</remarks>
        public int InteractionCooldown { get; } = 2000;
        /// <summary>
        /// This sets the Prefix for your commands, default is /
        /// </summary>
        public string InteractionPrefix = "/";
        
        /// <summary>
        /// This determines if the InteractionsClient should ignore its own interactions, default is true
        /// </summary>
        public bool IgnoreSelf = true;

        /// <summary>
        /// The LogLevel you set for the <see cref="Log"/> event
        /// </summary>
        public LogLevels LogLevel = LogLevels.None;

        /// <summary>
        /// This determines if your Interactions should be automatically handled, the default is false
        /// </summary>
        /// <remarks>Note: You can currently not edit the property as the Automatic interaction queue is not implemented</remarks>
        public bool AutoHandleInteractions { get; } = false;

        /// <summary>
        /// This determines if your Chats should automatically be cached, beware that this comes at the cost of API calls which can lead to a potential rate limit
        /// </summary>
        public bool AutoCacheChats { get; } = false;
        /// <summary>
        /// This determines if your Users should automatically be cached, beware that this comes at the cost of API calls which can lead to a potential rate limit
        /// </summary>
        public bool AutoCacheUsers { get; } = false;

        /// <summary>
        /// The event that fires when an Interaction is detected
        /// </summary>
        public event Action<Interaction> InteractionCreated;

        /// <summary>
        /// This event fires each time an interaction cannot be completed due to missing Cache
        /// </summary>
        public event Action<Interaction> InteractionInvalidCache;
        /// <summary>
        /// The event that fires when a Log is created
        /// </summary>
        public event Action<LogMessage> Log;


        /// <summary>
        /// The contructor of your InteractionsClient
        /// </summary>
        /// <param name="client">Your <see cref="Amino.Client"/> you want to use for this InteractionsClient, note that you should not turn off the websocket and that you must be logged in</param>
        public InteractionsClient(Amino.Client client)
        {
            this.AminoClient = client;
            
            this.InteractionModules = new Dictionary<string, Objects.InteractionModule>();

            if(AutoHandleInteractions)
            {
                this.InteractionQueue = new Queue<Objects.Interaction>();
                _ = Task.Run(async () => { await HandleInteractionQueue(); });
            }
            
            AminoClient.onMessage += HandleMessageSocket;

        }


        private void HandleMessageSocket(Amino.Objects.Message message)
        {
            if(message.Author.UserId == this.AminoClient.UserId && IgnoreSelf) { return; }
            if (message.Content.StartsWith(InteractionPrefix))
            {
                if (InteractionModules.ContainsKey(message.Content.Substring(InteractionPrefix.Length).Split(" ")[0]))
                {
                    InteractionModule module = InteractionModules[message.Content.Substring(InteractionPrefix.Length).Split(" ")[0]];
                    Interaction context = new Interaction();
                    context.Message = message;
                    context.InteractionParameters.AddRange(message.Content.Split(" ")[1..]);
                    context.InteractionChatId = message.ChatId;
                    context.AminoClient = this.AminoClient;
                    context.InteractionId = Guid.NewGuid().ToString();
                    context.InteractionName = module.ModuleCommandName;
                    context.InteractionTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    context.InteractionBaseModule = module;
                    
                    if(this.InteractionCreated != null) { this.InteractionCreated.Invoke(context); }

                }
            }
        }

        /// <summary>
        /// This function registers a single module into <see cref="InteractionModules"/>
        /// </summary>
        /// <typeparam name="T">Any Class / Module that has a valid structure, see <seealso href="https://github.com/Amino-NET-Group/Amino.NET.Interactions"/> to see how it works</typeparam>
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
                var enabledInGCsAttribute = method.GetCustomAttribute<EnabledInGroupChats>();
                var enabledInPCsAttribute = method.GetCustomAttribute<EnabledInPublicChats>();
                var parameters = new FunctionAnalyzer().GetParameters(method);
                

                InteractionModule module = new InteractionModule();
                module.ModuleCommandName = commandAttribute.CommandName;
                if (commandAttribute.CommunityId != null) { module.ModuleCommandCommunity = Convert.ToInt32(commandAttribute.CommunityId); }
                if (commandAttribute.CommandDescription != null) { module.ModuleCommandDescription = commandAttribute.CommandDescription; }
                if (enabledInDmsAttribute != null) { module.ModuleCommandEnabledInDms = enabledInDmsAttribute.IsEnabledInDms; }
                if(enabledInGCsAttribute != null) { module.ModuleCommandEnabledInGCs = enabledInGCsAttribute.IsEnabledInGCs; }
                if(enabledInPCsAttribute != null) { module.ModuleCommandEnabledInPCs = enabledInPCsAttribute.isEnabledInPCs; }
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


        /// <summary>
        /// Registers all valid Modules of a given <see cref="Assembly"/>
        /// </summary>
        /// <param name="entrypoint">The Assembly you are passing in</param>
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
                    var enabledInGCsAttribute = method.GetCustomAttribute<EnabledInGroupChats>();
                    var enabledInPCsAttribute = method.GetCustomAttribute<EnabledInPublicChats>();

                    var permissionGroup = method.GetCustomAttribute<PermissionGroup>();
                    var parameters = new FunctionAnalyzer().GetParameters(method);

                    InteractionModule module = new InteractionModule();
                    module.ModuleCommandName = commandAttribute.CommandName;
                    if(commandAttribute.CommunityId != null) { module.ModuleCommandCommunity = Convert.ToInt32(commandAttribute.CommunityId); }
                    if(commandAttribute.CommandDescription != null) { module.ModuleCommandDescription = commandAttribute.CommandDescription; }
                    if(enabledInDmsAttribute != null) { module.ModuleCommandEnabledInDms = enabledInDmsAttribute.IsEnabledInDms; }
                    if(enabledInGCsAttribute != null) { module.ModuleCommandEnabledInGCs = enabledInGCsAttribute.IsEnabledInGCs; }
                    if(enabledInPCsAttribute != null) { module.ModuleCommandEnabledInPCs = enabledInPCsAttribute.isEnabledInPCs; }
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
        
        /// <summary>
        /// A function that allows you to redirect an <see cref="Interaction"/> to its corresponding module
        /// </summary>
        /// <param name="interactionContext">The Interaction you pass in as Context</param>
        public void HandleInteraction(Objects.Interaction interactionContext)
        {
            List<object> args = new List<object>() { interactionContext };
            SubClient sub = new Amino.SubClient(interactionContext.AminoClient, interactionContext.Message.SocketBase.CommunityId.ToString());

            if(!interactionContext.InteractionBaseModule.ModuleCommandEnabledInPCs || !interactionContext.InteractionBaseModule.ModuleCommandEnabledInGCs || !interactionContext.InteractionBaseModule.ModuleCommandEnabledInDms)
            {
                if (!InteractionChatCache.ContainsKey(interactionContext.Message.ChatId))
                {
                    if (AutoCacheChats)
                    {
                        
                    } else
                    {
                        // Fire invalid cache event
                    }
                }
            }

            if(interactionContext.InteractionBaseModule.ModulePermissionGroup != PermissionGroup.PermissionGroups.All)
            {
                if (!InteractionUserCache.ContainsKey(interactionContext.Message.Author.UserId))
                {


                    if (AutoCacheUsers)
                    {
                        var user = sub.get_user_info(interactionContext.Message.Author.UserId);
                        UserCache _cache = new UserCache();
                        _cache.UserId = interactionContext.Message.Author.UserId;
                        // Handle getting user permission types
                    }
                }
            }

            ChatCache targetChat = null;
            UserCache targetUser = null;


            for (int i = 1; i < interactionContext.InteractionBaseModule.ModuleCommandParameters.Count; i++)
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
