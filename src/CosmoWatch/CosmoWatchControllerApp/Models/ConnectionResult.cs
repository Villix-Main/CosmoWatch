using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmoWatchControllerApp.Models
{
    public class ConnectionResult
    {
        public List<string>? SensorNames { get; set; }

        public bool ServerConnected { get; set; }

        public bool SensorControllerConnected { get; set; }

        public bool AlarmControllerConnected { get; set; }
    }
}
