using Avalonia.Controls;
using Avalonia.ReactiveUI;
using CosmoWatchControllerApp.ViewModels;
using Microsoft.VisualBasic;
using ReactiveUI;
using System.Threading.Tasks;

namespace CosmoWatchControllerApp.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            CanResize = false;

            this.WhenActivated(action =>
            {
                action(ViewModel!.ShowSensorDialog.RegisterHandler(DoShowSensorDialogAsync));
                action(ViewModel!.ShowAlarmDialog.RegisterHandler(DoShowAlarmDialogAsync));
            });
        }

        private async Task DoShowSensorDialogAsync(InteractionContext<SensorControllerViewModel, SensorsViewModel?> interaction)
        {
            var dialog = new SensorControllerWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<SensorsViewModel?>(this);
            interaction.SetOutput(result);
        }
        private async Task DoShowAlarmDialogAsync(InteractionContext<AlarmControllerViewModel, AlarmsViewModel?> interaction)
        {
            var dialog = new AlarmControllerWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<AlarmsViewModel?>(this);
            interaction.SetOutput(result);
        }
    }
}