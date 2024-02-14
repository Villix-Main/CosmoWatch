using Microsoft.AspNetCore.SignalR;

namespace CosmoWatchControllerAPI.Hubs
{
    public class AlarmHub : Hub
    {
        public async Task SendAlarmMessage(string alarmType, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", alarmType, message);
        }

        public async Task RequestStatusCheck()
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
        public async Task SendStatusCheck(string status)
        {
            await Clients.All.SendAsync("ReceiveMessage", status);
        }

    }
}
