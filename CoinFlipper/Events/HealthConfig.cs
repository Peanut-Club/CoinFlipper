namespace CoinFlipper.Events;

public class HealthConfig
{
	public int MinAddHealth { get; set; } = 10;


	public int MaxAddHealth { get; set; } = 100;


	public int MinRemoveHealth { get; set; } = 10;


	public int MaxRemoveHealth { get; set; } = 100;


	public int RemoveChance { get; set; } = 60;

}
