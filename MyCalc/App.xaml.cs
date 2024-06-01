using System.Collections.ObjectModel;

namespace MyCalc
{
    public partial class App : Application
    {
        public ObservableCollection<double> memoria { get; set; }
        public ObservableCollection<string> cronologia { get; set; }
        public App()
        {
            cronologia = new ObservableCollection<string>();
            memoria = new ObservableCollection<double>();
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}