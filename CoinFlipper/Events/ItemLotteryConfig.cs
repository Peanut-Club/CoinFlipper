using System.Collections.Generic;

namespace CoinFlipper.Events;

public class ItemLotteryConfig
{
	public int MaxQueueSize { get; set; } = 15;


	public int MinQueueSize { get; set; } = 5;


	public int NoItemChance { get; set; } = 30;


	public int SharedChance { get; set; } = 15;


	public List<ItemType> BlacklistedItems { get; set; } = new List<ItemType>();


	public Dictionary<ItemType, int> ItemChances { get; set; } = new Dictionary<ItemType, int>();


	public Dictionary<ItemCategory, int> CategoryChances { get; set; } = new Dictionary<ItemCategory, int>();

}
