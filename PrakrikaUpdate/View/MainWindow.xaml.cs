using PrakrikaUpdate.ViewModel;
using System.Windows;
namespace PrakrikaUpdate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowVM model)
        {
            InitializeComponent();
            this.DataContext = model;
        }

    }
}
