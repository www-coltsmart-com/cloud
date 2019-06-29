using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coltsmart.Portal.Models
{
    public class StatsInfo
    {
        public StatsInfo()
        {
            this.TotalDeviceCount = this.OnlineDeviceCount = this.TotalUserCount = 0;
            this.TotalDeviceDisplay = this.OnlineDeviceDisplay = this.TotalUserDisplay = false;
        }

        public int TotalDeviceCount { get; set; }

        public bool TotalDeviceDisplay { get; set; }

        public int OnlineDeviceCount { get; set; }

        public bool OnlineDeviceDisplay { get; set; }

        public int TotalUserCount { get; set; }

        public bool TotalUserDisplay { get; set; }
    }
}
