using CoinFlipper.Interfaces;

namespace CoinFlipper;

public struct CoinEventInfo
{
	public readonly ICoinEvent Event;

	public readonly CoinEventConfig EventConfig;

	public CoinEventInfo(ICoinEvent coinEvent, CoinEventConfig coinEventConfig)
	{
		Event = coinEvent;
		EventConfig = coinEventConfig;
	}
}
