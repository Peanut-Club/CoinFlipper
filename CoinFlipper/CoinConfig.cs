using System.Collections.Generic;
using System.ComponentModel;
using CoinFlipper.Events;

namespace CoinFlipper;

public class CoinConfig
{
	public static CoinConfig Instance;

	[Description("Configures behaviour of each coin event.")]
	public Dictionary<string, CoinEventConfig> CoinEvents { get; set; } = new Dictionary<string, CoinEventConfig> {
        [ExplodeEvent.PublicId] = new CoinEventConfig {
            Chance = 20
        },
        [HealthEvent.PublicId] = new CoinEventConfig {
            Chance = 30
        },
        [ItemLotteryEvent.PublicId] = new CoinEventConfig {
            Chance = 30
        },
        [KickEvent.PublicId] = new CoinEventConfig {
            Chance = 7
        },
        [NoEvent.PublicId] = new CoinEventConfig {
            Chance = 10
        },
        [RandomTeleportEvent.PublicId] = new CoinEventConfig {
            Chance = 30
        },
        [SwitchEvent.PublicId] = new CoinEventConfig {
            Chance = 10
        },
    };


	[Description("Configures the chance of an event happening if the coin lands on Tails.")]
	public int CoinTailsChance { get; set; } = 70;


	[Description("Configures the chance of an event happening if the coin lands on Heads.")]
	public int CoinHeadsChance { get; set; } = 75;


	[Description("Configuration for the Item Lottery event.")]
	public ItemLotteryConfig ItemLotteryConfig { get; set; } = new ItemLotteryConfig();


	[Description("Configuration for the Health event.")]
	public HealthConfig HealthConfig { get; set; } = new HealthConfig();


	public CoinConfig()
	{
		Instance = this;
	}
}
