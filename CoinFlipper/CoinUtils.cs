using System;
using System.Collections.Generic;
using System.Linq;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using UnityEngine;

namespace CoinFlipper;

public static class CoinUtils
{
	public static readonly System.Random Random = new System.Random();

	public static readonly bool[] BoolRandom = new bool[2] { true, false };

	public static T PickItem<T>(this IEnumerable<T> items, Func<T, int> weightSelector)
	{
		return items.ElementAtOrDefault(PickIndex(items.Sum(weightSelector), items.Count(), (int index) => weightSelector(items.ElementAtOrDefault(index))));
	}

	public static bool PickBool(int trueChance)
	{
		return BoolRandom[PickIndex(100, 2, (int index) => (!BoolRandom[index]) ? (100 - trueChance) : trueChance)];
	}

	public static int PickIndex(int total, int size, Func<int, int> picker)
	{
		int num = Random.Next(0, total);
		int num2 = 0;
		for (int i = 0; i < size; i++)
		{
			int num3 = picker(i);
			for (int j = num2; j < num3 + num2; j++)
			{
				if (j >= num)
				{
					return i;
				}
			}
			num2 += num3;
		}
		return 0;
	}

	public static void ThrowItBack(Player player, string reason)
	{
		Vector3 startVelocity = ((Component)(object)player.ReferenceHub).transform.rotation * Vector3.back;
		startVelocity.y = 1f;
		startVelocity.Normalize();
		startVelocity *= 5f;
		startVelocity.y *= 2f;
		CustomReasonDamageHandler customReasonDamageHandler = new CustomReasonDamageHandler(reason, -1f);
		customReasonDamageHandler.ApplyDamage(player.ReferenceHub);
		customReasonDamageHandler.StartVelocity = startVelocity;
		player.ReferenceHub.playerStats.KillPlayer(customReasonDamageHandler);
	}
}
