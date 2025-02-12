using System.Collections.Generic;
using System.Linq;
using CoinFlipper.Interfaces;
using CustomPlayerEffects;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using PluginAPI.Core;
using PluginAPI.Events;
using UnityEngine;
using static PlayerList;

namespace CoinFlipper.Events;

public class RandomTeleportEvent : ICoinEvent
{
	public static readonly IReadOnlyDictionary<RoomName, Vector3> Offsets = new Dictionary<RoomName, Vector3>
	{
		[RoomName.Lcz173] = new Vector3(15f, 12f, 8f),
		[RoomName.Lcz330] = new Vector3(0f, 0f, -2f),
		[RoomName.Hcz049] = new Vector3(-5f, 192f, -10f),
		[RoomName.Hcz096] = new Vector3(-2f, 0f, 0f),
		[RoomName.Hcz106] = new Vector3(20f, 0f, -5f),
		[RoomName.Hcz079] = new Vector3(7.5f, -1f, 2.25f), // inside armory (3f, -0.25f, -6f)
        [RoomName.HczTesla] = new Vector3(5f, 0f, 0f),
        [RoomName.HczTestroom] = new Vector3(0f, 0f, -6f),
        [RoomName.HczMicroHID] = new Vector3(0f, 5f, 0f),
        [RoomName.HczWarhead] = new Vector3(37f, -74f, 0f),
        [RoomName.EzCollapsedTunnel] = new Vector3(0f, 0f, 5f),
		[RoomName.EzEvacShelter] = new Vector3(0f, 0f, 5f),
		[RoomName.EzIntercom] = new Vector3(-4f, -3f, -2f)
	};

	public string Id => "random_teleport";

	public bool RemovesCoin => true;

	public CoinEventConfig Config { get; set; }

	public static void StaticApply(Player player)
	{
		bool wasInPocket = player.Room.Name == RoomName.Pocket;
		_ = RoomIdentifier.AllRoomIdentifiers;
		RoomIdentifier roomIdentifier = GetValidRooms().ToArray().RandomItem();
		if ((object)roomIdentifier != null)
		{
			Vector3 position = roomIdentifier.transform.position;
			position.y += 0.75f;
			if (Offsets.TryGetValue(roomIdentifier.Name, out var value) && value != Vector3.zero)
			{
				position += roomIdentifier.transform.rotation * value;
			}
			player.SendBroadcast($"<b><color=#ff0000>[TELEPORT]</color>\nMístnost: <color=#d4ff33>{roomIdentifier.Name}</color></b>", 5, Broadcast.BroadcastFlags.Normal, shouldClearPrevious: true);
			player.Position = position;

			if (wasInPocket) {
				var hub = player.ReferenceHub;
				hub.playerEffectsController.EnableEffect<Disabled>(10f, addDuration: true);
				hub.playerEffectsController.EnableEffect<Traumatized>();
				hub.playerEffectsController.DisableEffect<PocketCorroding>();
				hub.playerEffectsController.DisableEffect<Corroding>();
			}

            //PocketDimensionTeleport.OnPlayerEscapePocketDimension.Invoke(hub);
            //PlayerEvents.OnLeftPocketDimension(new PlayerLeftPocketDimensionEventArgs(hub, instance, isSuccessful: true));
        }
	}

	public void Apply(Player player) {
		StaticApply(player);
	} 

	public bool CanApply(Player player)
	{
		return GetValidRooms().Any();
	}

	public void Load()
	{
	}

	public void Unload()
	{
	}

	public static IEnumerable<RoomIdentifier> GetValidRooms()
	{
		return RoomIdentifier.AllRoomIdentifiers.Where((RoomIdentifier r) => r.gameObject != null && DoorVariant.AllDoors.Any((DoorVariant d) => d.Rooms != null && d.Rooms.Contains(r) && d.IsConsideredOpen()));
	}
}
