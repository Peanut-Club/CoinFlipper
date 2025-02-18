using System.Collections.Generic;

namespace CoinFlipper.Events;

public class ItemLotteryConfig
{
	public int MaxQueueSize { get; set; } = 20;


	public int MinQueueSize { get; set; } = 10;


	public int NoItemChance { get; set; } = 30;


	public int SharedChance { get; set; } = 60;


	public List<ItemType> BlacklistedItems { get; set; } = new List<ItemType> {
		ItemType.ArmorLight,
		ItemType.ArmorCombat,
		ItemType.ArmorHeavy,
		ItemType.SCP244a,
		ItemType.SCP244b
	};

	public Dictionary<ItemType, int> ItemChances { get; set; } = new Dictionary<ItemType, int> {
        [ItemType.KeycardContainmentEngineer] = 25,
        [ItemType.KeycardFacilityManager] = 20,
        [ItemType.KeycardO5] = 15,
        [ItemType.GunCOM15] = 30,
        [ItemType.GunCOM18] = 30,
        [ItemType.GunCom45] = 15,
        [ItemType.GunE11SR] = 20,
        [ItemType.GunCrossvec] = 20,
        [ItemType.GunLogicer] = 15,
        [ItemType.GunFSP9] = 25,
        [ItemType.GunRevolver] = 20,
        [ItemType.GunAK] = 20,
        [ItemType.GunShotgun] = 20,
        [ItemType.GunFRMG0] = 25,
        [ItemType.GunA7] = 30,
        [ItemType.ParticleDisruptor] = 10,
        [ItemType.Jailbird] = 15,
        [ItemType.MicroHID] = 5,
        [ItemType.GrenadeHE] = 25,
        [ItemType.GrenadeFlash] = 30,
        [ItemType.SCP018] = 25,
        [ItemType.Coin] = 40,
    };


	public Dictionary<ItemCategory, int> CategoryChances { get; set; } = new Dictionary<ItemCategory, int>();

}
