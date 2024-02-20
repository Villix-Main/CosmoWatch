using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;


namespace CosmoWatchControllerApp.Models
{
    public class SensorControllerService
    {
        private string mServerConnectionString;

        public IEnumerable<SensorReading> GetAllSensorReadings()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(mServerConnectionString)
                .WithAutomaticReconnect()
                .Build();
            return new SensorReading[3];
            
        }

    }
}
