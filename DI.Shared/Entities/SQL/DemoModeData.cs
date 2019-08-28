using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.SQL
{
    public class DemoModeData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int DayMark { get; set; }

        public int Trips { get; set; }

        public int Accelerations { get; set; }

        public int Mileage { get; set; }

        public int PeriodMark { get; set; }

        public DateTime RoundDate { get; set; }

        public int RateDynamics { get; set; }

        public bool IsInit { get; set; }
        public bool WithPeriod { get; set; }
    }
}
