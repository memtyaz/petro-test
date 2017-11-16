using System.Collections.Generic;
using Services;

namespace Petroineos.Dev.Model
{
    public interface IReportOutputGenerator
    {
        void ExtractReport(string filename, IEnumerable<PowerPeriod> listofPowerPeriod);
    }
}
