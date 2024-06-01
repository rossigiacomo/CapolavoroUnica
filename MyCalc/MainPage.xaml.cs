namespace MyCalc
{
    public partial class MainPage : ContentPage
    {
        CalcEngine calc;
        string? temp = "";
        public MainPage()
        {
            InitializeComponent();
            calc = new();

        }

        private void ClearButton(object sender, EventArgs e)
        {
            calc.ClearCalculation();
            CurrentCalculation.Text = string.Empty;
            resultText.Text = "0";
        }
        private void RadiceQuadrataButton(object sender, EventArgs e)
        {
            try
            {
                calc.Sqrt();
                resultText.Text = calc.DisplayValueString;
            } catch (Exception ex)
            {
                resultText.Text = "ERROR";
            }
            

        }
        private void Negativo(object sender, EventArgs e)
        {
            calc.InvertDisplaySign();
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void PercentualeButton(object sender, EventArgs e)
        {
            calc.Percentage();
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button7(object sender, EventArgs e)
        {
            int n = int.Parse(Sette.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button8(object sender, EventArgs e)
        {
            int n = int.Parse(Otto.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button9(object sender, EventArgs e)
        {
            int n = int.Parse(Nove.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button4(object sender, EventArgs e)
        {
            int n = int.Parse(Quattro.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button5(object sender, EventArgs e)
        {
            int n = int.Parse(Cinque.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button6(object sender, EventArgs e)
        {
            int n = int.Parse(Sei.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button1(object sender, EventArgs e)
        {
            int n = int.Parse(Uno.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button2(object sender, EventArgs e)
        {
            int n = int.Parse(Due.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button3(object sender, EventArgs e)
        {
            int n = int.Parse(Tre.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void Button0(object sender, EventArgs e)
        {
            int n = int.Parse(Zero.Text);
            calc.NumberPressed(n);
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void PuntoButton(object sender, EventArgs e)
        {
            calc.InsertDecimalSeparator();
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void DivisioneButton(object sender, EventArgs e)
        {
            try
            {
                calc.Divide();
                CurrentCalculation.Text = "÷";
            }
            catch (Exception ex)
            {
                resultText.Text = "ERROR";
            }
        }

        private void MoltiplicatoreButton(object sender, EventArgs e)
        {
            calc.Multiply();
            CurrentCalculation.Text = "X";
        }

        private void SottrazioneButton(object sender, EventArgs e)
        {
            calc.Minus();
            CurrentCalculation.Text = "-";
        }

        private void SommaButton(object sender, EventArgs e)
        {
            calc.Plus();
            CurrentCalculation.Text = "+";
        }

        private void UgualeButton(object sender, EventArgs e)
        {
            try
            {
                calc.Equals();
                resultText.Text = calc.DisplayValueString;
                CurrentCalculation.Text = string.Empty;
                string n = resultText.Text;
                (App.Current as App).cronologia.Add(n);
            } catch (Exception ex)
            {
                resultText.Text = "ERROR";
            }
            

        }

        private void BackSpace(object sender, EventArgs e)
        {
            calc.NumberBack();
            CurrentCalculation.Text = calc.DisplayValueString;
        }

        private void MemoryClearButton(object sender, EventArgs e)
        {
            calc.MemoryClearValue();
            
        }

        private void MemoryCallButton(object sender, EventArgs e)
        {
           calc.MemoryRecallValue();     
           resultText.Text = calc.DisplayValueString;
        }

        private void MemoryAddButton(object sender, EventArgs e)
        {
            calc.AddToMemory(calc.DisplayValue);
            
        }

        private void MemoryStoreButton(object sender, EventArgs e)
        {
            if (resultText.Text != null)
            {
                string stringa = resultText.Text;
                double n = double.Parse(stringa);
                (App.Current as App).memoria.Add(n);
            }
        }

        private void MemorySubButton(object sender, EventArgs e)
        {
            calc.SubFromMemory(calc.DisplayValue);

        }

        private async void MemoryButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MemoriaPage());
        }
    }

}
