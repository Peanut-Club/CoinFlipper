using System.Collections.Generic;

namespace CoinFlipper;

public class CoinEventConfig
{
	public static readonly CoinEventConfig DefaultConfig = new CoinEventConfig();

	public int Chance { get; set; }

	public string Message { get; set; } = "";


	public ushort MessageDuration { get; set; }

	public bool IsDisabled { get; set; }

	public bool IsTailsOnly { get; set; }

	public bool IsHeadsOnly { get; set; }

	public Dictionary<string, int> PersonalizedChances { get; set; } = new Dictionary<string, int>();

}
