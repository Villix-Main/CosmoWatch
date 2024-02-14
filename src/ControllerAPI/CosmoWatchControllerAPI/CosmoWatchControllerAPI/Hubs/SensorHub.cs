using Microsoft.AspNetCore.SignalR;

namespace CosmoWatchControllerAPI.Hubs
{
    public class SensorHub : Hub
    {
        public async Task GetSensorReading(string sensorName)
        {
            await Clients.All.SendAsync("ReceiveMessage", sensorName);
        }

        public async Task SendSensorReading(string sensorName, string value)
        {
            await Clients.All.SendAsync("ReceiveMessage", sensorName, value);
        }

        public async Task ChangeControllerSetting(string settingName, string value)
        {
            await Clients.All.SendAsync("ReceiveMessage", settingName, value);
        }
    }
}
