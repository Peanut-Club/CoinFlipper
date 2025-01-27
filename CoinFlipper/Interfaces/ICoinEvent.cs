using PluginAPI.Core;

namespace CoinFlipper.Interfaces;

public interface ICoinEvent
{
	string Id { get; }

	bool RemovesCoin { get; }

	CoinEventConfig Config { get; set; }

	void Load();

	void Unload();

	void Apply(Player player);

	bool CanApply(Player player);
}
