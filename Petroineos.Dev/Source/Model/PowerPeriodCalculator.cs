using System.Collections.Generic;
using System.Linq;
using Services;

namespace Petroineos.Dev.Model
{
    public class PowerPeriodCalculator : IPowerPeriodCalculator
    {
        public IEnumerable<PowerPeriod> CalculateVolume(IEnumerable<PowerTrade> powerTrades)
        {
            if (powerTrades == null)
                return null;

            Dictionary<int, PowerPeriod> listOfVolumes = new Dictionary<int, PowerPeriod>();

            
            powerTrades.ToList().ForEach(powerTrade =>
            {
                powerTrade.Periods.ToList().ForEach(x =>
                {
                    if (!listOfVolumes.ContainsKey(x.Period))
                    { 
                        listOfVolumes[x.Period] = new PowerPeriod{ Period = x.Period,Volume = x.Volume};
                    }
                    else
                        listOfVolumes[x.Period].Volume += x.Volume;
                });
            });
            return listOfVolumes.Values;
        }
    }
}
