# Amino.NET.Interactions Attributes
Attributes in Amino.NET.Interactions are used to define your commands and set properties for it, here you will see all attributes along with what they are for
All Attributes are located in the **Amino.Interactions.Attributes** namespace.

## Command
The Command attribute is the Attribute that is defining that you want a function to be a command, note that this Attribute **is required** for every command.
| Value | DataType | Description | Is Required |
|-------|----------|-------------|-------------|
| commandName | string | The name you want to give to the command | Yes | 
| commandDescription | string | A Description you can give to this command, it _could_ be used to generate help commands and such as you can easily fetch all Command Modules | No |
| CommunityId | string | The Community ID you want to lock your command to | No

## EnabledInDms
The EnabledInDms attribute will decide if your command can be used in DMs and or DM Group chats, this Attribute **is not** required to make a command.
| Values | DataType | Description | Is required |
|--------|----------|-------------|-------------|
| isEnabledInDms | bool | The boolean value that decides if the command is availab in DMs | Yes

## PermissionGroup
The PermissionGroup Attribute is used to set the required permission level for your command without you needing to do extra handling for it, it contains an **enum** that you **will need** so make sure to get it from its class **Amino.Interactions.Attributes.PermissionGroup.PermissionGroups**, this Attribute **is not** required to make your command work.
| Values | DataType | Description | Is Required | 
|--------|----------|-------------|-------------|
| permisionGroup | PermissionGroups | The permission group you set to be required for your command | Yes