using System.Collections.Generic;
using Services;

namespace Petroineos.Dev.Model
{
    public interface IPowerPeriodCalculator
    {
        IEnumerable<PowerPeriod> CalculateVolume(IEnumerable<PowerTrade> powerTrades);
    }
}
