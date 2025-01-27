using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using PluginAPI.Core;
using Utils.NonAllocLINQ;

namespace CoinFlipper.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
public class CoinEventCommand : ICommand
{
	public string Command => "coinevent";

	public string Description => "Forces a coin event to occur.";

	public string[] Aliases { get; } = Array.Empty<string>();


	public bool SanitizeResponse => false;

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		if (arguments.Count < 2)
		{
			response = "Invalid usage!\ncoinevent <players> <event_id>";
			return false;
		}
		List<int> list = Misc.ProcessRaPlayersList(arguments.At(0));
		if (list.Count < 1)
		{
			response = "No players found.";
			return false;
		}
		IEnumerable<Player> enumerable = list.Select(Player.Get);
		if (enumerable.Count() < 1)
		{
			response = "No players found.";
			return false;
		}
		if (!CoinEvents.Events.ToList().TryGetFirst((CoinEventInfo ev) => ev.Event.Id.ToLower() == arguments.At(1).ToLower(), out var first))
		{
			response = "Unknown event.";
			return false;
		}
		foreach (Player item in enumerable)
		{
			first.Event.Apply(item);
		}
		response = $"Run event '{first.Event.Id}' on {enumerable.Count()} player(s).";
		return true;
	}
}
