using System;
using System.Linq;
using CoinFlipper.Interfaces;
using MapGeneration;
using PlayerRoles;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace CoinFlipper.Events;

public class SwitchEvent : ICoinEvent
{
	public static readonly RoomName[] BlacklistedRooms = new RoomName[]
	{
		RoomName.EzCollapsedTunnel,
		RoomName.EzEvacShelter,
		RoomName.EzRedroom,
		RoomName.HczMicroHID,
		RoomName.HczTestroom,
		RoomName.Pocket,
		RoomName.Unnamed
	};

	public string Id => "switch";

	public bool RemovesCoin => true;

	public CoinEventConfig Config { get; set; }

	public void Apply(Player player)
	{
		if (player.Room.Name == RoomName.Pocket) {
            RandomTeleportEvent.StaticApply(player);
            return;
		}
        Player[] array = (from p in Player.List
			where p.NetworkId != player.NetworkId && ProcessPlayer(player, p)
			select p).ToArray();
		if (array.Length < 1)
		{
			player.SendBroadcast("<b><color=#ff0000>[SWITCH]</color>\nNemáš s kým si prohodit pozice.</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
			return;
		}
		Player player2 = array.RandomItem();
		if (player2 == null)
		{
			player.SendBroadcast("<b><color=#ff0000>[SWITCH]</color>\nNemáš s kým si prohodit pozice.</b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
			return;
		}
		Vector3 position = player.Position;
		player.SendBroadcast($"<b><color=#ff0000>[SWITCH]</color>\nProhodíš si místa s <color=#d4ff33>{player2.Role} {player2.Nickname}</color></b>", 10, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
		player.Position = player2.Position;
		player2.SendBroadcast($"<b><color=#ff0000>[SWITCH]</color>\nHráč <color=#d4ff33>{player.Role} {player.Nickname}</color> použíl minci a prohodil si s tebou místa.</b>", 10, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
		player2.Position = position;
	}

	public bool CanApply(Player player)
	{
		return Player.List.Any((Player p) => p.NetworkId != player.NetworkId && ProcessPlayer(player, p));
	}

	public void Load()
	{
	}

	public void Unload()
	{
	}

	private static bool ProcessPlayer(Player player, Player target)
	{
		_ = player.ReferenceHub;
		ReferenceHub referenceHub = target.ReferenceHub;
		RoleTypeId roleTypeId = target.ReferenceHub.roleManager.CurrentRole.RoleTypeId;
		RoomIdentifier roomIdentifier = RoomIdUtils.RoomAtPosition(((Component)(object)target.ReferenceHub).transform.position);
		if (!referenceHub.IsAlive())
		{
			return false;
		}
		if (roleTypeId == RoleTypeId.Scp079 || roleTypeId == RoleTypeId.Tutorial)
		{
			return false;
        }
        if (roomIdentifier != null && BlacklistedRooms.Contains(roomIdentifier.Name)) {
            return false;
        }
        return true;
	}
}
