using System;
using System.Collections.Generic;
using CoinFlipper.Interfaces;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Items.Firearms.Modules;
using InventorySystem.Items.Pickups;
using MEC;
using LabApi.Features.Wrappers;
using LabApi.Features.Console;

namespace CoinFlipper.Events;

public class ItemLotteryEvent : ICoinEvent
{
	private static List<ItemBase> _prefabs = new List<ItemBase>();

	private static ItemLotteryConfig _config;

	public string Id => "item_lottery";

	public bool RemovesCoin => true;

	public CoinEventConfig Config { get; set; }

	public void Apply(Player player)
	{
		ItemBase[] generatedQueue = GenerateItemQueue(CoinUtils.Random.Next(_config.MinQueueSize, _config.MaxQueueSize));
		Timing.RunCoroutine(Lottery(player, generatedQueue, delegate(ItemType pickedItem)
		{
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			ItemBase result;
			if (pickedItem == ItemType.None)
			{
				player.SendBroadcast("<b><color=#ff0000>[LOTERIE]</color>\nNevyhrál jsi nic.</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
			}
			else if (!InventoryItemLoader.TryGetItem<ItemBase>(pickedItem, out result))
			{
				player.SendBroadcast("<b><color=#ff0000>[LOTERIE]</color>\nNevyhrál jsi nic.</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
			}
			else if (result.Category == ItemCategory.Ammo)
			{
				int num = CoinUtils.Random.Next(20, 200);
				player.AddAmmo(pickedItem, (ushort)num);
				player.SendBroadcast($"<b><color=#ff0000>[LOTERIE]</color>\nVyhrál jsi <color=#d4ff33>{num}</color> nábojů <color=#d4ff33>{pickedItem}</color></b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
			}
			else if (player.ReferenceHub.inventory.UserInventory.Items.Count >= 8)
			{
				player.SendBroadcast($"<b><color=#ff0000>[LOTERIE]</color>\nVyhrál jsi <color=#d4ff33>{pickedItem}</color>! <i>(máš plný inventář - výhra je na zemi)</i></color></b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
				InventoryExtensions.ServerCreatePickup(player.ReferenceHub.inventory, result, new PickupSyncInfo(pickedItem, result.Weight, (ushort)0), true, (Action<ItemPickupBase>)null);
			}
			else
			{
				ItemBase itemBase = InventoryExtensions.ServerAddItem(player.ReferenceHub.inventory, pickedItem, ItemAddReason.AdminCommand, (ushort)0, (ItemPickupBase)null);
				if (itemBase != null && itemBase is Firearm firearm)
				{
					if (AttachmentsServerHandler.PlayerPreferences.TryGetValue(player.ReferenceHub, out var value) && value.TryGetValue(pickedItem, out var value2)) {
						firearm.ApplyAttachmentsCode(value2, true);
					} else {
                        firearm.ApplyAttachmentsCode(AttachmentsUtils.GetRandomAttachmentsCode(pickedItem), true);
					}
					if (firearm.HasAdvantageFlag(AttachmentDescriptiveAdvantages.Flashlight)) {
						foreach (var at in firearm.Attachments) {
							if (at is FlashlightAttachment) {
                                at.IsEnabled = true;
                                break;
							}
						}
					}

				}
				player.SendBroadcast($"<b><color=#ff0000>[LOTERIE]</color>\nVyhrál jsi <color=#d4ff33>{pickedItem}</color>!</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
			}
		}));
	}

	public bool CanApply(Player player)
	{
		if (_config != null && _config.MinQueueSize > 0)
		{
			return _config.MaxQueueSize > 0;
		}
		return false;
	}

	public void Load()
	{
		_config = CoinConfig.Instance.ItemLotteryConfig;
		_prefabs = new List<ItemBase>();
		ItemType[] values = EnumUtils<ItemType>.Values;
		foreach (ItemType itemType in values)
		{
			if (InventoryItemLoader.TryGetItem<ItemBase>(itemType, out var result) && !_config.BlacklistedItems.Contains(itemType) && (!_config.ItemChances.TryGetValue(itemType, out var value) || value >= 1) && (!_config.CategoryChances.TryGetValue(result.Category, out var value2) || value2 >= 1))
			{
				_prefabs.Add(result);
			}
		}
	}

	public void Unload()
	{
		_prefabs.Clear();
		_prefabs = null;
		_config = null;
	}

	private IEnumerator<float> Lottery(Player player, ItemBase[] generatedQueue, Action<ItemType> pickedItem)
	{
		int curSpins = 0;
		for (int targetSpins = 3; curSpins != targetSpins; curSpins++)
		{
			for (int i = 0; i < generatedQueue.Length; i++)
			{
				player.SendBroadcast("<b><color=#ff0000>[LOTERIE]</color>\nMožná výhra: <color=#d4ff33>" + (generatedQueue[i]?.ItemTypeId.ToString() ?? "Žádná výhra") + "</color></b>", 1, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
				yield return Timing.WaitForSeconds((curSpins >= targetSpins / 2) ? 0.1f : 0.2f);
			}
		}
		try
		{
			pickedItem(generatedQueue.PickItem(ItemChancePicker)?.ItemTypeId ?? ItemType.None);
		}
		catch (Exception message)
		{
			Logger.Error(message);
		}
	}

	private static ItemBase[] GenerateItemQueue(int queueSize)
	{
		ItemBase[] array = new ItemBase[queueSize];
		for (int i = 0; i < queueSize; i++)
		{
			if (CoinUtils.PickBool(_config.NoItemChance))
			{
				array[i] = null;
			}
			else
			{
				array[i] = _prefabs.PickItem(ItemChancePicker);
			}
		}
		return array;
	}

	private static int ItemChancePicker(ItemBase item)
	{
		if ((object)item == null)
		{
			return _config.NoItemChance;
		}
		if (_config.BlacklistedItems.Contains(item.ItemTypeId))
		{
			return 0;
		}
		if (_config.ItemChances.TryGetValue(item.ItemTypeId, out var value))
		{
			return value;
		}
		if (_config.CategoryChances.TryGetValue(item.Category, out var value2))
		{
			return value2;
		}
		return _config.SharedChance;
	}
}
