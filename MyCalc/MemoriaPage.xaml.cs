using System.Collections.ObjectModel;

namespace MyCalc
{
    public partial class MemoriaPage : ContentPage
    {
        public ObservableCollection<double> MemoryItems { get; set; }
        public MemoriaPage()
        {
            InitializeComponent();
            MemoryItems = (App.Current as App).memoria;
            lamiamemoria.ItemsSource = MemoryItems;
            Title = "Memoria";
        }
        private void EliminaButton(object sender, TappedEventArgs e)
        {
            MemoryItems.Clear();
        }
    }
}