
using PrakrikaUpdate.Model;
using PrakrikaUpdate.ViewModel;
using System.Windows;

namespace PrakrikaUpdate
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var model = new MainWindowModel();
            var vmodel = new MainWindowVM(model);
            MainWindow wnd = new MainWindow(vmodel);
            wnd.Show();
        }
    }
}
