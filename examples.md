# Amino.NET.Interactions Command Examples
These are some examples of how you could use Amino.NET.Interactions to see what it is capable of.

## Simple Ping command
```cs
[Command("ping", "Simple Ping command")]
public async Task PingCommand(Interaction context)
{
    await Respond(context, "Pong");
}
```
## Simple Report Command
```cs
[Command("report", "Simple report command")]
public async Task ReportCommand(Interaction context, string user, string[] reason)
{
    Console.WriteLine($"User {context.Message.Author.userName} has reported {user} with reason {reason[0]}");
    // Additionally store the report somewhere and or notify staff
    await Respond(context, "Thank you for your report, a staff member will shortly look into it")
}
```

## Utility command that only works in a spcific community
```cs
[Command("utility", "Some utility command", 123456)] // 123456 in this case is the communityId
public async Task CommunityCommand(Interaction context)
{
    await Respond(context, "Some response");
}
```
## Utility command that does not work in DMs
```cs
[Command("utility", "Some utility command")]
[EnabledInDms(false)]
public async Task NoDmCommand(Interaction context)
{
    await Respond(context, "Some response");
}
```
## Utiliy command that can only be accessed by a staff member
```cs
[Command("utility", "Some utility command")]
[PermisionGroup(PermissionGroups.Staff)] // PermissionGroups is located in Amino.Interactions.Attributes.PermissionGroup
public async Task StaffCommand(Interaction context)
{
    await Respond(context, "Some response");
}
```
### Additional information
You can mix all these attributes together, the examples only show the bare, to see what other types you can respond with, kindly check out [The Objects Guide](./Objects.md)