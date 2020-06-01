using LaptopBatteryGraph.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace LaptopBatteryGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ((ViewModelLocator)this.DataContext).Main.Initialize();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            ((ViewModelLocator)this.DataContext).Main.Cleanup();
        }
    }
}
