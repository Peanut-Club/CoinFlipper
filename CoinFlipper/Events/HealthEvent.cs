using CoinFlipper.Interfaces;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace CoinFlipper.Events;

public class HealthEvent : ICoinEvent
{
	private static HealthConfig _config;

	public string Id => "health";

	public bool RemovesCoin => true;

	public CoinEventConfig Config { get; set; }

	public void Apply(Player player)
	{
		if (CoinUtils.PickBool(_config.RemoveChance))
		{
			int num = (int)Mathf.Clamp(CoinUtils.Random.Next(_config.MinRemoveHealth, _config.MaxRemoveHealth), 0f, player.Health);
			if (num > 0)
			{
				player.Health -= num;
			}
			else
			{
				player.Kill("Gambling");
			}
			player.SendBroadcast($"<b><color=#ff0000>[HEALTH]</color> Bylo ti odebráno <color=#d4ff33>{num}</color> HP!</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
		}
		else
		{
			int num2 = (int)Mathf.Clamp(CoinUtils.Random.Next(_config.MinAddHealth, _config.MaxAddHealth), 0f, player.MaxHealth - player.Health);
			if (num2 > 0)
			{
				player.Heal(num2);
				player.SendBroadcast($"<b><color=#ff0000>[HEALTH]</color> Bylo ti přidáno <color=#d4ff33>{num2}</color> HP!</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
			}
		}
	}

	public bool CanApply(Player player)
	{
		return _config != null;
	}

	public void Load()
	{
		_config = CoinConfig.Instance.HealthConfig;
	}

	public void Unload()
	{
		_config = null;
	}
}
