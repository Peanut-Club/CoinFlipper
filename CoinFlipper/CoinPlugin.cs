using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using System;

namespace CoinFlipper;

public class CoinPlugin : Plugin<CoinConfig> {
	public static CoinPlugin Plugin;

    public override string Name { get; } = "CoinFlipper";

    public override string Description { get; } = "Adds random events that can occur when a player flips a coin.";

    public override string Author { get; } = "marchellc";

    public override Version Version { get; } = new Version(1, 0, 0);

    public override Version RequiredApiVersion { get; }

    public override void Enable() {
        Plugin.Enable();
        CoinEvents.ReloadEvents();

        LabApi.Events.Handlers.PlayerEvents.FlippingCoin += OnFlippingCoin;
    }

    public override void Disable() {
        Plugin.Disable();

        LabApi.Events.Handlers.PlayerEvents.FlippingCoin -= OnFlippingCoin;
    }

    public void OnFlippingCoin(PlayerFlippingCoinEventArgs ev)
	{
		bool success = CoinUtils.PickBool(50);
        ev.IsTails = success;

        CoinEvents.RunEvents(ev.Player, (ev.Player.CurrentItem as CoinItem).Base, success);
	}
}
