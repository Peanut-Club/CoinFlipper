using InventorySystem.Items.Coin;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace CoinFlipper;

public class CoinPlugin
{
	public static PluginHandler Handler;

	public static CoinPlugin Plugin;

	[PluginConfig]
	public CoinConfig Config;

	[PluginEntryPoint("CoinFlipper", "1.0.0", "Adds random events that can occur when a player flips a coin.", "marchellc")]
	public void Load()
	{
		CoinEvents.ReloadEvents();
		EventManager.RegisterAllEvents(this);
	}

	[PluginEvent]
	public PlayerPreCoinFlipCancellationData OnCoin(PlayerPreCoinFlipEvent ev)
	{
		bool flag = CoinUtils.PickBool(50);
		PlayerPreCoinFlipCancellationData result = new PlayerPreCoinFlipCancellationData((!flag) ? PlayerPreCoinFlipCancellationData.CoinFlipCancellation.Heads : PlayerPreCoinFlipCancellationData.CoinFlipCancellation.Tails);
		CoinEvents.RunEvents(ev.Player, ev.Player.ReferenceHub.inventory.CurInstance as Coin, flag);
		return result;
	}

	public static void SaveConfig()
	{
		Handler.SaveConfig(Plugin, "Config");
	}
}
