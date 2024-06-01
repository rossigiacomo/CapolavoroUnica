using Expression = org.matheval.Expression;
using org.matheval.Functions;
using System.Collections.ObjectModel;
using System.Text;
using System.Globalization;

namespace MyCalc;

public partial class ScientificaPage : ContentPage
{
    string temp = "";
    StringBuilder sb = new StringBuilder();
    public ScientificaPage()
    {
        InitializeComponent();
    }


    private void ClearButton(object sender, EventArgs e)
    {
        sb.Clear();
        resultText.Text = "0";
        CurrentCalculation.Text = string.Empty;
    }

    private void BackSpace(object sender, EventArgs e)
    {
        string currentInput = sb.ToString();
        if (currentInput.Length > 0)
        {
            // Remove the last character from the current input string
            currentInput = currentInput.Substring(0, currentInput.Length - 1);

            // Update the result label with the modified input
            CurrentCalculation.Text = currentInput;
        }
    }

    private void RadiceQuadrataButton(object sender, EventArgs e)
    {
        double n = double.Parse(sb.ToString());
        if (n > 0)
        {
            double risultato = Math.Sqrt(n);
            resultText.Text = risultato.ToString();

        }

    }

    private void Button7(object sender, EventArgs e)
    {
        int n = int.Parse(Sette.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void Button8(object sender, EventArgs e)
    {
        int n = int.Parse(Otto.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void Button9(object sender, EventArgs e)
    {
        int n = int.Parse(Nove.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void Button4(object sender, EventArgs e)
    {
        int n = int.Parse(Quattro.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void Button5(object sender, EventArgs e)
    {
        int n = int.Parse(Cinque.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void Button6(object sender, EventArgs e)
    {
        int n = int.Parse(Sei.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void Button1(object sender, EventArgs e)
    {
        int n = int.Parse(Uno.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void Button2(object sender, EventArgs e)
    {
        int n = int.Parse(Due.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void Button3(object sender, EventArgs e)
    {
        int n = int.Parse(Tre.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void Negativo(object sender, EventArgs e)
    {
        sb.Insert(0, "-");
        CurrentCalculation.Text = sb.ToString();
    }

    private void Button0(object sender, EventArgs e)
    {
        int n = int.Parse(Zero.Text);
        sb.Append(n);
        CurrentCalculation.Text = sb.ToString();
    }

    private void PuntoButton(object sender, EventArgs e)
    {
        sb.Append(",");
        CurrentCalculation.Text = sb.ToString();
        sb.Clear();
    }

    private void DivisioneButton(object sender, EventArgs e)
    {
        sb.Append("/");
        CurrentCalculation.Text = sb.ToString();
    }

    private void MoltiplicatoreButton(object sender, EventArgs e)
    {
        sb.Append("*");
        CurrentCalculation.Text = sb.ToString();
    }

    private void SottrazioneButton(object sender, EventArgs e)
    {
        sb.Append("-");
        CurrentCalculation.Text = sb.ToString();
    }

    private void SommaButton(object sender, EventArgs e)
    {
        sb.Append("+");
        CurrentCalculation.Text = sb.ToString();
    }

    private void UgualeButton(object sender, EventArgs e)
    {
        try
        {
            if (sb.Length != 0)
            {
                string calcolo = sb.ToString();
                CurrentCalculation.Text = calcolo + " = ";
                Expression exp = new(calcolo);
                string risultato = exp.Eval().ToString();
                sb.Clear();
                sb.Append(risultato);
                resultText.Text = sb.ToString();

                if (!string.IsNullOrEmpty(CurrentCalculation.Text))
                {

                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private void MemoryClearButton(object sender, EventArgs e)
    {

    }

    private void MemoryCallButton(object sender, EventArgs e)
    {

    }

    private void MemoryAddButton(object sender, EventArgs e)
    {

    }

    private void MemorySubButton(object sender, EventArgs e)
    {

    }

    private void MemoryStoreButton(object sender, EventArgs e)
    {

    }

    private void MemoryButton(object sender, EventArgs e)
    {

    }

    private void DivisioneModuloButton(object sender, EventArgs e)
    {

    }

    private void ExpButton(object sender, EventArgs e)
    {

    }

    private void ModuloButton(object sender, EventArgs e)
    {
        double n = double.Parse(sb.ToString());
        double risultato = Math.Abs(n);
        resultText.Text = risultato.ToString();
    }

    private void ReciprocoButton(object sender, EventArgs e)
    {
        double risultato = 1 / (double.Parse(sb.ToString()));
        resultText.Text = risultato.ToString();
    }

    private void eButton(object sender, EventArgs e)
    {
        sb.Append(Math.E);
        CurrentCalculation.Text = sb.ToString();
    }

    private void PiGrecoButton(object sender, EventArgs e)
    {
        sb.Append(Math.PI);
        CurrentCalculation.Text = sb.ToString();
    }

    private void PotenzaButton(object sender, EventArgs e)
    {
        double n = double.Parse(sb.ToString());

        resultText.Text = Math.Pow(n, 2).ToString();
    }

    private void ParentesiAperta(object sender, EventArgs e)
    {
        sb.Append("(");
        CurrentCalculation.Text = sb.ToString();
    }

    private void fattoriale(object sender, EventArgs e)
    {
        string currentInput = sb.ToString();
        if (string.IsNullOrEmpty(currentInput))
        {
            return;
        }

        CurrentCalculation.Text = "fact(" + sb.ToString() + ")"; 
        int numero = int.Parse(currentInput);
        int fattoriale = CalcolaFattoriale(numero);

        // Aggiorna l'etichetta del risultato
        resultText.Text = fattoriale.ToString();
    }

    private int CalcolaFattoriale(int n)
    {
        if (n == 0)
        {
            return 1;
        }
        else
        {
            int result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
    }

    private void ParentesiChiusa(object sender, EventArgs e)
    {
        sb.Append(")");
        CurrentCalculation.Text = sb.ToString();
    }

    private void XallaY(object sender, EventArgs e)
    {
        
    }

    private void Logaritmo(object sender, EventArgs e)
    {
        string currentInput = sb.ToString();
        if (string.IsNullOrEmpty(currentInput))
        {
            return;
        }


        CurrentCalculation.Text = "Log(" + sb.ToString() + ")";
        double risultato;




        risultato = Math.Log10(double.Parse(currentInput));



        // Aggiorna l'etichetta del risultato
        resultText.Text = risultato.ToString();
    }

    private void ln(object sender, EventArgs e)
    {
        string currentInput = sb.ToString();
        if (string.IsNullOrEmpty(currentInput))
        {
            return;
        }


        CurrentCalculation.Text = "Log(" + sb.ToString() + ")";
        double risultato;




        risultato = Math.Log(double.Parse(currentInput));



        // Aggiorna l'etichetta del risultato
        resultText.Text = risultato.ToString();
    }

    private void DieciAllaX(object sender, EventArgs e)
    {
        double n = double.Parse(sb.ToString());
        double risultato = Math.Pow(10, n);
        resultText.Text = sb.ToString();
    }
}