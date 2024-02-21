using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7153" + "/sensorHub")
                .WithAutomaticReconnect()
                .Build();

connection.On<string>("GetSensorReading", SendSensorReading);

var t = connection.StartAsync();

t.Wait();


async Task SendSensorReading(string sensor)
{
    Console.WriteLine("hello there");
    await connection.InvokeAsync("SendSensorReading", "Oxygen", "32");
}


Console.Read();