using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using log4net;
using Services;

namespace Petroineos.Dev.Model
{
    public class ReportController : IReportController
    {
        private readonly IReportOutputGenerator _reportGenerator;
        private readonly IPowerService _powerService;
        private readonly IPowerPeriodCalculator _powerPeriodCalculator;
        readonly string _fileDirectory = ConfigurationManager.AppSettings["FileDirectory"];
        readonly string _filenameDateTimeFormat = ConfigurationManager.AppSettings["FilenameDateTimeFormat"];
        readonly string _fileExtension = ConfigurationManager.AppSettings["FileExtension"];

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ReportController(IReportOutputGenerator reportGenerator,
                                 IPowerService powerService,
                                 IPowerPeriodCalculator powerPeriodCalculator)
        {
            _reportGenerator = reportGenerator;
            _powerService = powerService;
            _powerPeriodCalculator = powerPeriodCalculator;
        }

       

        public void Run()
        {
            IEnumerable<PowerTrade> trades = null;
            do
            {
                Log.Info(" Begin Run: getting Trades ");
                try
                {
                    trades = _powerService.GetTrades(DateTime.Now);
                }
                catch (PowerServiceException  powerServiceException)
                {
                    Log.Error($"Error while retrieving trades from power service {powerServiceException.Message}");
                }
            } while (trades == null);


            var powerPeriods = _powerPeriodCalculator.CalculateVolume(trades);

            _reportGenerator.ExtractReport(GetFileName(), powerPeriods);

        }

        public string GetFileName()
        {
            try
            {
                if (!Directory.Exists(_fileDirectory))
                {
                    Log.Info($"Creating Directory for the path {_fileDirectory}");
                    Directory.CreateDirectory(_fileDirectory);
                }
                var filepath =
                    $@"{_fileDirectory}\PowerPosition_{DateTime.Now.ToString(_filenameDateTimeFormat)}{_fileExtension}";
                Log.Info($"The filename with full path is {filepath}");
                return filepath;
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot create directory for the path {_fileDirectory}, ErrorMessage -{ex.Message}");
            }

            return string.Empty;
        }



    }
}
