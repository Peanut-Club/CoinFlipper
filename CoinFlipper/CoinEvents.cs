using System;
using System.Collections.Generic;
using System.Linq;
using CoinFlipper.Interfaces;
using InventorySystem;
using InventorySystem.Items.Coin;
using MEC;
using NorthwoodLib.Pools;
using PluginAPI.Core;
using Utils.NonAllocLINQ;

namespace CoinFlipper;

public static class CoinEvents
{
	private static readonly Dictionary<string, CoinEventInfo> _coinEvents = new Dictionary<string, CoinEventInfo>();

	private static readonly List<string> _disabledEvents = new List<string>();

	public static IEnumerable<CoinEventInfo> Events => _coinEvents.Values;

	public static IEnumerable<string> Disabled => _disabledEvents;

	public static void ReloadEvents()
	{
		foreach (CoinEventInfo value2 in _coinEvents.Values)
		{
			value2.Event?.Unload();
		}
		_coinEvents.Clear();
		_disabledEvents.Clear();
		Type[] types = typeof(CoinEvents).Assembly.GetTypes();
		foreach (Type type in types)
		{
			if (type == typeof(ICoinEvent) || !typeof(ICoinEvent).IsAssignableFrom(type))
			{
				continue;
			}
			try
			{
				if (Activator.CreateInstance(type) is ICoinEvent coinEvent)
				{
					coinEvent.Load();
					Info("Loaded coin event &1" + coinEvent.Id + "&r!");
					if (!CoinConfig.Instance.CoinEvents.TryGetValue(coinEvent.Id, out var value))
					{
						value = CoinEventConfig.DefaultConfig;
					}
					_coinEvents[coinEvent.Id] = new CoinEventInfo(coinEvent, value);
					if (value.IsDisabled || value.Chance < 1)
					{
						_disabledEvents.Add(coinEvent.Id);
					}
				}
				else
				{
					Warn("Failed to construct coin event &2" + type.Name + "&r!");
				}
			}
			catch (Exception arg)
			{
				Error($"An error occured while constructing coin event &2{type.Name}&r!\n{arg}");
			}
		}
	}

	public static void RunEvents(Player player, Coin coinItem, bool isTails)
	{
		try
		{
			int num = (isTails ? CoinConfig.Instance.CoinTailsChance : CoinConfig.Instance.CoinHeadsChance);
			if (num < 1)
			{
				player.SendBroadcast("<b><color=#ff0000>[COIN]</color> Zkus to znova.</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
				return;
			}
			if (!CoinUtils.PickBool(num))
			{
				player.SendBroadcast("<b><color=#ff0000>[COIN]</color> Zkus to znova.</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
				return;
			}
			List<CoinEventInfo> list = ListPool<CoinEventInfo>.Shared.Rent();
			foreach (CoinEventInfo value in _coinEvents.Values)
			{
				if (!_disabledEvents.Contains(value.Event.Id) && !value.EventConfig.IsDisabled && value.EventConfig.Chance >= 1 && !(value.EventConfig.IsHeadsOnly && isTails) && (!value.EventConfig.IsTailsOnly || isTails) && value.Event.CanApply(player))
				{
					list.Add(value);
				}
			}
			if (list.Count < 1)
			{
				player.SendBroadcast("<b><color=#ff0000>[COIN]</color> Zkus to znova.</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
				ListPool<CoinEventInfo>.Shared.Return(list);
				return;
			}
			CoinEventInfo coinEventInfo2 = list.PickItem((CoinEventInfo coinEventInfo) => ChancePicker(coinEventInfo, player));
			if (coinEventInfo2.Event != null)
			{
				coinEventInfo2.Event.Apply(player);
				if (coinEventInfo2.Event.RemovesCoin && coinItem != null)
				{
					Timing.CallDelayed(0.5f, delegate
					{
						player.ReferenceHub.inventory.ServerRemoveItem(coinItem.ItemSerial, coinItem.PickupDropModel);
					});
				}
				if (!string.IsNullOrWhiteSpace(coinEventInfo2.EventConfig.Message) && coinEventInfo2.EventConfig.MessageDuration > 0)
				{
					player.SendBroadcast(coinEventInfo2.EventConfig.Message, coinEventInfo2.EventConfig.MessageDuration, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
				}
			}
			else
			{
				player.SendBroadcast("<b><color=#ff0000>[COIN]</color> Zkus to znova.</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
			}
			ListPool<CoinEventInfo>.Shared.Return(list);
		}
		catch (Exception arg)
		{
			Error($"Caught an error while picking a random coin event:\n{arg}");
		}
	}

	public static void Error(object message)
	{
		Log.Info(message.ToString(), "Coin Flipper");
	}

	public static void Warn(object message)
	{
		Log.Info(message.ToString(), "Coin Flipper");
	}

	public static void Info(object message)
	{
		Log.Info(message.ToString(), "Coin Flipper");
	}

	private static int ChancePicker(CoinEventInfo coinEventInfo, Player player)
	{
		if (coinEventInfo.EventConfig.IsDisabled || coinEventInfo.EventConfig.Chance < 1)
		{
			return 0;
		}
		if (coinEventInfo.EventConfig.PersonalizedChances.TryGetValue(player.UserId, out var value))
		{
			return value;
		}
		if (player.ReferenceHub.serverRoles.Group != null && coinEventInfo.EventConfig.PersonalizedChances.ToList().TryGetFirst((KeyValuePair<string, int> p) => player.ReferenceHub.serverRoles.Group.BadgeText.Contains(p.Key), out var first))
		{
			return first.Value;
		}
		return coinEventInfo.EventConfig.Chance;
	}
}
