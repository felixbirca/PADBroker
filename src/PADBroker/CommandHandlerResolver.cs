// public class CommandHandlerResolver
// {
//     private Dictionary<string, IPADCommandHandler> _handlers;

//     public CommandHandlerResolver(IEnumerable<IPADCommandHandler> handlers)
//     {
//         _handlers = handlers.ToDictionary(h => h.CommandName, StringComparer.OrdinalIgnoreCase);
//     }

//     public IRedisCommandHandler Resolve(List<string> commands)
//     {
//         if (_handlers.TryGetValue(commands[0].ToLower(), out var handler))
//         {
//             return handler;
//         }

//         throw new Exception("Failed to get handler");
//     }
// }
