using CoinFlipper.Interfaces;
using PluginAPI.Core;

namespace CoinFlipper.Events;

public class NoEvent : ICoinEvent
{
	public string Id => "no_event";

	public bool RemovesCoin => true;

	public CoinEventConfig Config { get; set; }

	public bool CanApply(Player player)
	{
		return true;
	}

	public void Apply(Player player)
	{
		player.SendBroadcast("<b><color=#ff0000>[COIN]</color> Docela neštěstí.</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
	}

	public void Load()
	{
	}

	public void Unload()
	{
	}
}
