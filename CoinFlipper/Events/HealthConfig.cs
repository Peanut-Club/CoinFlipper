namespace CoinFlipper.Events;

public class HealthConfig
{
	public int MinAddHealth { get; set; } = 300;


	public int MaxAddHealth { get; set; } = 100;


	public int MinRemoveHealth { get; set; } = 5;


	public int MaxRemoveHealth { get; set; } = 80;


	public int RemoveChance { get; set; } = 40;

}
