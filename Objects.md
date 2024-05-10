# Amino.NET.Interaction Objects Guide
This file will contain all (public) information about Objects and Clients you will use
Note that some Parameters have default values, they will be displayed _like this_

## InteractionsClient
| Values | DataType | Description | Default |
|--------|----------|-------------|---------|
| InteractionModules| Dictionary<string, [InteractionModule](#interactionmodule)> | The internal "database" of all registred modules
|InteractionQueue | Queue<[Interaction](#interaction)>| A list of upcoming Interactions, not used if AutoHandleInteractions isn't marked as `true`
| LogLevels | enum | An enum collection of the available LogLevels
| InteractionCooldown | int | The cooldown used in automatic interaction handling | 2000
| InteractionPrefix | string | The prefix of your commands | /
| IgnoreSelf | bool | Determines if the bot should ignore its own actions | true
| LogLevel | LogLevels | The desired LogLevel for that InteractionsClient instance | LogLevels.None
| AutoHandleInteractions | bool | Determines if Interactions should be automatically handled | false
| InteractionCreated | Action<[Interaction](#interaction)> | The event that fires when an Interaction is created
| Log | Action<[LogMessage](#logmessage)> | The event that fires when a Log message is availabe

## InteractionsClient Functions
| Usage | Description | Return |
|-------|-------------|--------|
| InteractionsClient(Amino.Client) | The constructor for your InteractionsClient, it will initiate all important things needed
| RegisterModule<T\>() | This function will allow you to register an Interactions Module, please see [The general guide](./README.md) to see what makes a valid module
| RegisterModules(Assembly) | This function will register all modules of the given Assembly, please see [The general guide](./README.md) to see what makes a valid module
| HandleInteraction([Interaction](#interaction)) | The function used to redirect your interactions to the proper functions.

## InteractionBase Functions
| Usage | Description | Return |
|-------|-------------|--------|
| Respond([Interaction](#interaction), string, string, _bool_) | The function to respond to an Interaction using a normal message | Task<Amino.Objects.Message>
| RespondWithFile([Interaction](#interaction), byte[], Amino.Types.upload_file_types) | A function to respond to an Interaction with a file
| RespondWithSticker([Interaction](#interaction), string) | A function that responds to an Interaction using a Sticker
| RespondWithEmbed([Interaction](#interaction), _string_, _string_, _string_, _string_, _string_, _byte[]_) | A function that responds to an Interaction using an Embed

## LogMessage
| Values | DataType | Description | Default |
|--------|----------|-------------|---------|
| Message | string | The message of the LogMessage |
| Timestamp | long | The UNIX timestamp of the LogMessage |
| LogLevel | [LogLevels](#interactionsclient) | The LogLevel of the LogMessage |

## InteractionModule
| Values | DataType | Description | Default |
|--------|----------|-------------|---------|
| ModuleCommandName | string | The name of the command|
| ModuleCommandDescription | string | The description of your command module |
| ModuleCommandCommunity | int? | The community ID you have locked your command module to |
| ModuleInteractionBase | [InteractionBase](#interactionbase-functions) | The base interaction class of this Module |
| ModuleCommandEnabledInDms | bool | The value that decides if your module is available in DMs | true
| ModulePermissionGroup | PermissionGroups | The required user Permission to use this command module | PermissionGroups.All
| ModuleCommandParameters | List<(string, bool)> | A List of Parameters of this module, string is for parameter Type and bool decides if the parameter is optional or not | new List<(string, bool)>
| ModuleInteractionMethod | InteractionMethodDelegate | The function linked to this module

## Interaction
| Values | DataType | Description | Default |
|--------|----------|-------------|---------|
| InteractionChatId | string? | The Chat ID the Interaction has taken place in
| InteractionName | string? | The name of the module that is getting targeted
| AminoClient | Amino.Client | The Amino Client the Interaction has happened with
| Message | Amino.Objects.Message | The Message object of the Interaction
| InteractionTimestamp | long | The UNIX Timestamp of the Interaction
| InteractionId | string? | A Unique identifier for this Interaction
| InteractionParameters | List< string > | A list of parameters given in the Interaction | new List< string >
| InteractionBaseModule | [InteractionModule](#interactionmodule) | The InteractionModule the current Interaction is linked to