using System.Collections.Generic;
using System.ComponentModel;
using CoinFlipper.Events;

namespace CoinFlipper;

public class CoinConfig
{
	public static CoinConfig Instance;

	[Description("Configures behaviour of each coin event.")]
	public Dictionary<string, CoinEventConfig> CoinEvents { get; set; } = new Dictionary<string, CoinEventConfig> { ["example_id"] = new CoinEventConfig() };


	[Description("Configures the chance of an event happening if the coin lands on Tails.")]
	public int CoinTailsChance { get; set; } = 50;


	[Description("Configures the chance of an event happening if the coin lands on Heads.")]
	public int CoinHeadsChance { get; set; } = 50;


	[Description("Configuration for the Item Lottery event.")]
	public ItemLotteryConfig ItemLotteryConfig { get; set; } = new ItemLotteryConfig();


	[Description("Configuration for the Health event.")]
	public HealthConfig HealthConfig { get; set; } = new HealthConfig();


	public CoinConfig()
	{
		Instance = this;
	}
}
