namespace MyCalc;

public partial class InformazioniPage : ContentPage
{
    public InformazioniPage()
    {
        InitializeComponent();
    }

    private async void OnLabelClicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://github.com/GreppiLabs/4ia2324-activity3-calc-GiacomoRossi");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {

        }
    }
}