using CosmoWatchControllerApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CosmoWatchControllerApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ServerConnectionService mConnectionService;

        private Timer timer;
        private int num = 0;

        private string? mNumber = "0";
        public string? Number
        {
            get => mNumber;
            private set => this.RaiseAndSetIfChanged(ref mNumber, value);
        }

        public MainWindowViewModel()
        {
            ShowSensorDialog = new Interaction<SensorControllerViewModel, SensorsViewModel?>();

            OpenSensorController = ReactiveCommand.Create(async () =>
            {
                var controller = new SensorControllerViewModel();

                var result = await ShowSensorDialog.Handle(controller);
            });

            OpenAlarmController = ReactiveCommand.Create(() =>
            {

            });

            mConnectionService = new ServerConnectionService();
            var result = Task.Run(() => mConnectionService.Initialize()).Result;
            var test = result?.SensorNames;

            timer = new Timer(DoSum, null, 0, 5000);
            mConnectionService.SensorControllerHub.On<string, string>("SendSensorReading", GetSensorReading);

        }

        private void DoSum(object? state)
        {
            mConnectionService.SensorControllerHub.InvokeAsync("GetSensorReading", "Oxygen");
        }
        private void GetSensorReading(string sensor, string value)
        {
            Number = value + '%';
        }


        public ICommand OpenSensorController { get; }

        public ICommand OpenAlarmController { get; }

        public Interaction<SensorControllerViewModel, SensorsViewModel?> ShowSensorDialog { get; }

    }
}
