using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;


string availableSensors = "temperature,oxygen,carbon_dioxide";

HttpClient client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:7153");


using StringContent content = new(
    JsonSerializer.Serialize(new
    {
        names = availableSensors
    }), System.Text.Encoding.UTF8, "application/json");

var formContent = new FormUrlEncodedContent(new[]
{
    new KeyValuePair<string, string>("", "sensorNames")
});


var response = await client.PostAsync("/Sensor/sendNames", content);
string resultContent = await response.Content.ReadAsStringAsync();
Console.WriteLine(resultContent);


var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7153" + "/sensorHub")
                .WithAutomaticReconnect()
                .Build();

connection.On<string>("GetSensorReading", SendSensorReading);

var t = connection.StartAsync();

t.Wait();

async Task SendSensorReading(string sensor)
{
    Console.WriteLine("Sensor Reading Requested");
    switch (sensor)
    {
        case "temperature":
            await connection.InvokeAsync("SendSensorReading", "temperature", "87");
            break;
        case "oxygen":
            await connection.InvokeAsync("SendSensorReading", "oxygen", "54");
            break;
        case "carbon_dioxide":
            await connection.InvokeAsync("SendSensorReading", "carbon_dioxide", "1500");
            break;

    }

}


Console.Read();