using CoinFlipper.Interfaces;
using LabApi.Features.Wrappers;
using Utils;

namespace CoinFlipper.Events;

public class ExplodeEvent : ICoinEvent
{
	public static string PublicId => "explode";

	public string Id => PublicId;

	public bool RemovesCoin => true;

	public CoinEventConfig Config { get; set; }

	public void Apply(Player player)
	{
		ExplosionUtils.ServerSpawnEffect(player.Position, ItemType.GrenadeHE);
		CoinUtils.ThrowItBack(player, "Gambling");
	}

	public bool CanApply(Player player)
	{
		if (player.IsAlive && !player.IsSCP)
		{
			return !player.IsTutorial;
		}
		return false;
	}

	public void Load()
	{
	}

	public void Unload()
	{
	}
}
