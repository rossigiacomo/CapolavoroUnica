using System.Collections.ObjectModel;

namespace MyCalc
{
    public partial class CronologiaPage : ContentPage
    {
        public ObservableCollection<string> CronologiaItems { get; set; }
        public CronologiaPage()
        {
            InitializeComponent();
            CronologiaItems = (App.Current as App).cronologia;
            lamiacronologia.ItemsSource = CronologiaItems;
            Title = "Cronologia";
        }
        

        private void CancellaCronologia(object sender, EventArgs e)
        {
            CronologiaItems?.Clear();
        }
    }
}