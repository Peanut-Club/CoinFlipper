using CoinFlipper.Interfaces;
using LabApi.Features.Wrappers;

namespace CoinFlipper.Events;

public class KickEvent : ICoinEvent
{
	public string Id => "kick";

	public bool RemovesCoin => false;

	public CoinEventConfig Config { get; set; }

	public void Apply(Player player)
	{
		player.Kick("Gambling");
	}

	public bool CanApply(Player player)
	{
		return !player.IsTutorial;
	}

	public void Load()
	{
	}

	public void Unload()
	{
	}
}
