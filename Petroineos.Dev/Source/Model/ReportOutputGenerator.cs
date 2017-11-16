using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Services;

namespace Petroineos.Dev.Model
{
    public class ReportOutputGenerator: IReportOutputGenerator
    {
        const string LocalTime = "Local Time";
        const string Volume = "Volume";

        public string[] LocalTimeForOutput = {

                                                 "23:00", "00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00",
                                                 "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00",
                                                 "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00"
                                             };


        public void ExtractReport(string filename, IEnumerable<PowerPeriod> listofPowerPeriod)
        {
            if (filename == null)
                throw new Exception("Null File Name exception");
            WriteToCSV(filename, listofPowerPeriod);        

        }

        void WriteToCSV(string filename, IEnumerable<PowerPeriod> listofPowerPeriod)
        {

            using (var streamWriter = new StreamWriter(filename))
            {
                var s = $"{LocalTime},{Volume}";
                streamWriter.WriteLine("{0},{1}", LocalTime, Volume);              

                foreach (var volume in listofPowerPeriod.OrderBy(it => it.Period).ToList())
                    streamWriter.WriteLine("{0}, {1}", LocalTimeForOutput[volume.Period-1], volume.Volume);
            }
        }

    }
}
