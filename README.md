# Amino.NET.Interactions
The official Amino.NET Framework made for chatbots!
# What is this framework about?
Making chatbots on Amino has always been quite a hassle, with the Amino.NET.Interactions framework this has never been easier!

## Extras & Credits
- This C# library is based on [Amino.NET](https://github.com/Amino-NET-Group/Amino.NET) and will therefore include it as a dependency, in case anything on this Library breaks consider updating Amino.NET to the latest stable (or dev) version, if this does not fix your issue, open an issue on github and or [join our community Discord Server](https://discord.gg/qyv8P2gegK)
- If you find a bug or have an idea you'd like to see in this project, please open an issue for it or join our community discord, alternatively you can always make a pull request if you'd like to contribute to the development of it
- Sometimes the documentation is not always up to date, we are aware of it and will fix it over time
- Amino.NET Group / Amino Scripting group and its sub parts do not take any responsibility for any harm being done using this framework, as it is free and **open source**, therefore we can not prevent any harm done.
- This is a non-profit project, however if you do want to us and our work you can check the Sponsor options of this repository, alternatively you can [check out our Ko-Fi!](https://ko-fi.com/fabiothefox)
## Important Notice
By using this library you agree that you are aware of the fact that you are breaking the App services Terms of Service - as Team Amino strictly forbids the use of any sort of third party software / scripting to gain an advantage over other members, any activity by third party tools found by Team Amino may result in your account getting banned from their services!
## How to install
You can install Amino.NET.Interactions [on NuGet](https://www.nuget.org) and in any NuGet package manager of your choice!
## Quick Links
- [General idea and setup](#where-to-start)
- [Objects and Values](./Objects.md)
- [Attributes Guide](./Attributes.md)
- [Command Examples](./examples.md)


# Where to start
Because Amino.NET.Interactions is a framework built on top of Amino.NET you first need to initialize your Amino Client
```cs
Amino.Client client = new Amino.Client(); // Note that you should fill in possible parameters if you want
client.login("email", "password"); // You need to be logged into your Amino Account (or the account you plan to use as a bot) make sure to NOT disable your websocket as it will be needed
```
### Creating your interaction Client
The InteractionsClient can be found in the **Amino.Interactions** namespace and needs an **Amino.Client** to be constructed
```cs
InteractionsClient interactionClient = new InteractionsClient(client);
// Our InteractionsClient has a few values we could set, but we will leave it on default for now
```
### Creating your first command module
With Amino.NET.Interactions you can easily create Command Modules, in short: Every class that is **public** and inherits **InteractionBase** is valid to be registered
```cs
public class MyCommandClass : InteractionBase { ... } // This module is valid because it is both public and inherits InteractionBase
```
### Creating your first command
Amino.NET.Interactions has a few Attributes you can use for your commands, they are stored in the **Amino.Interactions.Attributes** namespace, in this case we will use Async Tasks to get our command done, all commands should be stored inside of a **valid module**
```cs
[Command("ping", "Simple Ping Command")]
public async Task PingCommand(Interaction context) 
{
    await Respond(context, "pong");
}
```
### Registering your modules
In order for your commands to take effect and be usable, you must first register your modules, you have 2 options for it, register them all or register specific ones
```cs
interactionClient.RegisterModule<MyCommandClass>(); // This will register the MyCommandClass as it is a valid module
interactionClient.RegisterModules(Assembly.GetEntryAssemlbly()); // This will register all valid modules of the project, note that it will always need an Assembly object to be constructed, just pass in the EntryAssembly as shown in the example
```
### Handling Interactions
In order for your Interactions to be received, you must first subscribe to an event on the `InteractionsClient`, once that is done, Amino.NET.Interactions will automatically redirect incoming interactions into the corresponding functions.
```cs
interactionClient.InteractionCreated += (ctx) => {
    interactionClient.HandleInteraction(ctx); // Note that you could also just build a function instead of catching the event like this
};
```
### Rules and conventions
Amino.NET.Interactions has a few rules to keep in mind, these are:
- In order for a module to be valid it must be **public** and inherit **InteractionBase**
- Every Command function **must** have a parameter of type **Interaction** as its first parameter, this object will be used for our interaction Context, it is located in the **Amino.Interactions.Objects** namespace
- For command parameters you currenly only have 2 options, String and String[], a `string` will contain a single word while `string[]` assume that you want to collect the rest of the message, which is why it is advised to be the last parameter, if you choose to collect all remaining words using a `string[]` parameter, you can find them in `index 0` of the array
- Commands can **not** contain spaces, consider using `-` or `_`
