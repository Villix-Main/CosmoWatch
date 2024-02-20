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

            this.WhenActivated(action => action(ViewModel!.ShowSensorDialog.RegisterHandler(DoShowDialogAsync)));
        }

        private async Task DoShowDialogAsync(InteractionContext<SensorControllerViewModel, SensorsViewModel?> interaction)
        {
            var dialog = new SensorControllerWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<SensorsViewModel?>(this);
            interaction.SetOutput(result);
        }
    }
}