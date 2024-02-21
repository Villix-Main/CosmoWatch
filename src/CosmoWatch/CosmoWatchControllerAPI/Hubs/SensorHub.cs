using Microsoft.AspNetCore.SignalR;

namespace CosmoWatchControllerAPI.Hubs
{
    public class SensorHub : Hub
    {
        public async Task GetAllAvailableSensors()
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
        public async Task SendAllAvailableSensors(string sensors)
        {
            await Clients.All.SendAsync("ReceiveMessage", sensors);
        }

        public async Task GetSensorReading(string sensorName)
        {
            await Clients.All.SendAsync("GetSensorReading", sensorName);
        }

        public async Task SendSensorReading(string sensorName, string value)
        {
            await Clients.All.SendAsync("SendSensorReading", sensorName, value);
        }

        public async Task ChangeControllerSetting(string settingName, string value)
        {
            await Clients.All.SendAsync("ReceiveMessage", settingName, value);
        }
    }
}
