using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CosmoWatchControllerApp.Models
{
    public class ServerConnectionService
    {
        //private string mConnectionString = "https://localhost:7153";
        private string mConnectionString = "https://localhost:44325";

        private HttpClient mClient;

        private HubConnection mSensorControllerHub;
        private HubConnection mAlarmControllerHub;
        private bool mIsConnected = false;

        private List<string> mSensorNames = new List<string>();

        public ServerConnectionService()
        {
            mSensorControllerHub = new HubConnectionBuilder()
                .WithUrl(mConnectionString + "/sensorHub")
                .WithAutomaticReconnect()
                .Build();

            mAlarmControllerHub = new HubConnectionBuilder()
                .WithUrl(mConnectionString + "/alarmHub")
                .WithAutomaticReconnect()
                .Build();

            mClient = new HttpClient();
            mClient.BaseAddress = new Uri(mConnectionString);
            
            mSensorControllerHub.On<string>("SendAllAvailableSensors", SetSensorNames);
        }

        public ConnectionResult? CurrentResult { get; set; }

        public HubConnection SensorControllerHub { get =>  mSensorControllerHub; }

        public async Task<ConnectionResult> Initialize()
        {
            try
            {
                var test = mSensorControllerHub.State;
                await mSensorControllerHub.StartAsync();
                await mAlarmControllerHub.StartAsync();
                mIsConnected = true;
            }
            catch
            {
                mIsConnected = false;
                return new ConnectionResult
                {
                    ServerConnected = false
                };
            }

            var result = await mClient.GetAsync("/Sensor/getNames");
            SetSensorNames(await result.Content.ReadAsStringAsync());

            return new ConnectionResult
            {
                SensorNames = mSensorNames,
                ServerConnected = true,
                AlarmControllerConnected = mIsConnected,
                SensorControllerConnected = mIsConnected
            };
        }

        public bool TestConnection()
        {
            bool isConnected;

            var response = mClient.GetAsync("/");
            isConnected = response.IsCompletedSuccessfully;

            return isConnected;
        }

        private void SetSensorNames(string sensors)
        {
            mSensorNames = sensors.Split(',').ToList();
        }
        
    }
}
