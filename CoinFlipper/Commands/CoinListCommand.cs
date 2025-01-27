using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;

namespace CoinFlipper.Commands;

[CommandHandler(typeof(GameConsoleCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CoinListCommand : ICommand
{
	public string Command => "coinlist";

	public string Description => "Shows a list of all coin events.";

	public string[] Aliases { get; } = Array.Empty<string>();


	public bool SanitizeResponse => false;

	public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
	{
		List<CoinEventInfo> list = CoinEvents.Events.ToList();
		if (list.Count < 1)
		{
			response = "No events loaded.";
			return false;
		}
		response = $"Coin Events ({list.Count}):\n";
		foreach (CoinEventInfo item in list)
		{
			if (CoinEvents.Disabled.Contains(item.Event.Id))
			{
				response = response + "- " + item.Event.Id + " [DISABLED]\n";
			}
			else
			{
				response = response + "- " + item.Event.Id + " [ENABLED]\n";
			}
		}
		return true;
	}
}
