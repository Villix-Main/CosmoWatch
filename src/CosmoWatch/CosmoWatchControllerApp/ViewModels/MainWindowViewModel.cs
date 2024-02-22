using CosmoWatchControllerApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CosmoWatchControllerApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ServerConnectionService mConnectionService;

        private List<string>? mSensorNames;

        private Timer? readingTimer;
        private Timer? pingTimer;

        // Server Connection Status
        private string? mConnectionStatus;
        public string? ConnectionStatus
        {
            get => mConnectionStatus;
            private set => this.RaiseAndSetIfChanged(ref mConnectionStatus, value);
        }
        private string? mConnectionStatusCol;
        public string? ConnectionStatusCol
        {
            get => mConnectionStatusCol;
            private set => this.RaiseAndSetIfChanged(ref mConnectionStatusCol, value);
        }
        

        // Room Status Value
        private int mStatusVal = 1000;
        private string? mStatusPercentage = "N/A";
        public string? StatusPercentage
        {
            get => mStatusPercentage;
            set => this.RaiseAndSetIfChanged(ref mStatusPercentage, value);
        }
        private string? mStatusLabel;
        public string? StatusLabel
        {
            get => mStatusLabel;
            private set => this.RaiseAndSetIfChanged(ref mStatusLabel, value);
        }


        // Temperature Reading
        private string? mTemperatureReading = "N/A";
        public string? TemperatureReading
        {
            get => mTemperatureReading;
            private set => this.RaiseAndSetIfChanged(ref mTemperatureReading, value);
        }
        // Oxygen Reading
        private string? mOxygenReading = "N/A";
        public string? OxygenReading
        {
            get => mOxygenReading;
            private set => this.RaiseAndSetIfChanged(ref mOxygenReading, value);
        }
        // Carbon Dioxide Reading
        private string? mCarbonDioxideReading = "N/A";
        public string? CarbonDioxideReading
        {
            get => mCarbonDioxideReading;
            private set => this.RaiseAndSetIfChanged(ref mCarbonDioxideReading, value);
        }


        // Oxygen Severity Color
        private string? mOxygenSeverityCol = "#008170";
        public string? OxygenSeverityCol
        {
            get => mOxygenSeverityCol;
            private set => this.RaiseAndSetIfChanged(ref mOxygenSeverityCol, value);
        }
        // Carbon Dioxide Severity Color
        private string? mCarbonDioxideSeverityCol = "#008170";
        public string? CarbonDioxideSeverityCol
        {
            get => mCarbonDioxideSeverityCol;
            private set => this.RaiseAndSetIfChanged(ref mCarbonDioxideSeverityCol, value);
        }
        // Temperature Severity Color
        private string? mTemperatureSeverityCol = "#6B78CC";
        public string? TemperatureSeverityCol
        {
            get => mTemperatureSeverityCol;
            private set => this.RaiseAndSetIfChanged(ref mTemperatureSeverityCol, value);
        }

        // Temperature Control
        private string? mTemperatureControl = "0";
        public string? TemperatureControl
        {
            get => mTemperatureControl;
            private set => this.RaiseAndSetIfChanged(ref mTemperatureControl, value);
        }
        // Temperature Control Slider Val
        private int mTemperatureSliderVal = 0;
        public int TemperatureSliderVal
        {
            get => mTemperatureSliderVal;
            private set => this.RaiseAndSetIfChanged(ref mTemperatureSliderVal, value);
        }

        private int count = 0;

        public MainWindowViewModel()
        {
            ShowSensorDialog = new Interaction<SensorControllerViewModel, SensorsViewModel?>();
            ShowAlarmDialog = new Interaction<AlarmControllerViewModel, AlarmsViewModel?>();

            OpenSensorController = ReactiveCommand.Create(async () =>
            {
                var controller = new SensorControllerViewModel();

                var result = await ShowSensorDialog.Handle(controller);
            });

            OpenAlarmController = ReactiveCommand.Create(async () =>
            {
                var controller = new AlarmControllerViewModel();

                var result = await ShowAlarmDialog.Handle(controller);
            });

            mConnectionService = new ServerConnectionService();
            var result = Task.Run(() => mConnectionService.Initialize()).Result;

            if (!result.ServerConnected)
            {
                ConnectionStatus = "Disconnected";
                ConnectionStatusCol = StatusColors.Critical;
                return;
            }
            else
            {
                ConnectionStatus = "Connected";
                ConnectionStatusCol = StatusColors.Success;
            }

            mSensorNames = result?.SensorNames;

            readingTimer = new Timer(GetSensorsReading, null, 0, 5000);
            pingTimer = new Timer(CheckConnection, null, 0, 3000);

            mConnectionService.SensorControllerHub.On<string, string>("SendSensorReading", GetSensorReading);
        }

        private void GetSensorsReading(object? state)
        {
            mStatusVal = 1000;
            count = 0;

            foreach (var name in mSensorNames!)
            {
                mConnectionService.SensorControllerHub.InvokeAsync("GetSensorReading", name);
            }
        }
        private void GetSensorReading(string sensor, string value)
        {

            switch (sensor)
            {
                case "temperature":
                    TemperatureReading = value + "°F";
                    TemperatureControl = TemperatureReading;
                    SetTemperatureSevColor(int.Parse(value));
                    break;
                case "oxygen":
                    OxygenReading = value + "%";
                    SetOxygenSevCol(int.Parse(value));
                    break;
                case "carbon_dioxide":
                    CarbonDioxideReading = value;
                    SetCarbonDioxideSevCol(int.Parse(value));
                    break;
            }

            if (++count >= mSensorNames?.Count)
            {
                var percentage = Math.Floor(mStatusVal / 1000.0 * 100);

                if (percentage >= 80)
                    StatusLabel = "Stable";
                else if (percentage >= 65)
                    StatusLabel = "Fair";
                else if (percentage < 65)
                    StatusLabel = "Critical";

                StatusPercentage = percentage.ToString() + '%';
            }
        
        }


        private void SetTemperatureSevColor(int value)
        {
            if (value >= 95)
            {
                TemperatureSeverityCol = StatusColors.Critical;
                mStatusVal -= 250;
            }
            else if (value >= 62)
                TemperatureSeverityCol = StatusColors.Purple;
            else if (value < 62)
            {
                TemperatureSeverityCol = StatusColors.Critical;
                mStatusVal -= 200;
            }
        }
        private void SetOxygenSevCol(int value)
        {
            if (value >= 80)
                OxygenSeverityCol = StatusColors.Success;
            else if (value > 65)
            {
                OxygenSeverityCol = StatusColors.Warning;
                mStatusVal -= 200;
            }
            else if (value <= 65)
            {
                OxygenSeverityCol = StatusColors.Critical;
                mStatusVal -= 300;
            }
        }
        private void SetCarbonDioxideSevCol(int value)
        {
            if (value >= 1600)
            {
                CarbonDioxideSeverityCol = StatusColors.Critical;
                mStatusVal -= 400;
            }
            else if (value >= 1100)
            {
                CarbonDioxideSeverityCol = StatusColors.Warning;
                mStatusVal -= 150;
            }
            else if (value < 1100)
                CarbonDioxideSeverityCol = StatusColors.Success;
        }


        private void CheckConnection(object? state)
        {
            var test = mConnectionService.TestConnection();
        }

        public ICommand OpenSensorController { get; }

        public ICommand OpenAlarmController { get; }

        public Interaction<SensorControllerViewModel, SensorsViewModel?> ShowSensorDialog { get; }
        public Interaction<AlarmControllerViewModel, AlarmsViewModel?> ShowAlarmDialog { get; }

    }
}
