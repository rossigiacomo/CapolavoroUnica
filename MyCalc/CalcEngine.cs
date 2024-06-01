using System.Globalization;

namespace MyCalc
{
    public class CalcEngine
    {
        // The value in the display.
        public double DisplayValue { get; private set; }
        //The String being entered on the display
        public string DisplayValueString { get; private set; }
        // The previous operator typed (+, -, etc.).
        private char previousOperator;
        // The left operand to previousOperator.
        private double leftOperand;
        //True if a new value is in input
        public bool IsNewValue { get; private set; }
        //True if a new calculus in in place - it is true at startup and whenever a calculation is completed
        //e.g. after equals or after 'C' (ClearCalculation)
        public bool IsNewCalculation { get; private set; }
        //True if a dot was inserted
        private bool decimalSeparatorInserted;
        //True if a value was inserted before the operand is pressed
        private bool previousValueWasInserted;
        private readonly CultureInfo culture;
        private readonly string decimalSeparator;
        //the memory used to store previous values
        public List<double> MemoryValues { get; private set; } = new List<double>();

        public CalcEngine(CultureInfo? culture = null)
        {
            ClearCalculation();
            MemoryClearValue();
            decimalSeparatorInserted = false;
            DisplayValueString = "0";
            this.culture = culture ??
                CultureInfo.CurrentCulture;
            decimalSeparator = culture?.NumberFormat.NumberDecimalSeparator ??
                CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        }

        /// <summary>
        /// This method tries to perform rounding of the DisplayValueString when the result has to be returned back to the user. 
        /// This happens in case of unary operator like, sqrt, reciprocal, square, etc. or in case of the equals sign
        /// </summary>
        private void FormatAndRoundDisplayValueStringAfterResult()
        {
            //check if the number is integer or not
            //if the number is integer it is displayed withouth the . sign
            double valAsInt = Math.Floor(DisplayValue);
            double valRounded = Math.Round(DisplayValue, 14);
            double epsilon = 1e-14;
            bool isAlmostLong = Math.Abs(DisplayValue - valAsInt) < epsilon
                && !double.IsInfinity(DisplayValue)
                && Math.Abs(DisplayValue) < long.MaxValue; ;
            //check if the number can be rounded with a fewer number of digits
            bool canBeRounded = Math.Abs(valRounded - DisplayValue) < epsilon && !double.IsInfinity(valRounded);
            DisplayValueString = isAlmostLong ? ((long)DisplayValue).ToString("N0", culture)
                : canBeRounded ? valRounded.ToString(culture)
                    : DisplayValue.ToString("N15", culture);

        }
        /// <summary>
        /// This method tries to perform formatting of the DisplayValueString as long as the input is written, before
        /// the result is returned back to the user. This method is used to provide proper digit separator according
        /// to the current culture
        /// </summary>
        private void FormatDisplayValueStringBeforeResult()
        {
            double valAsInt = Math.Floor(DisplayValue);
            double epsilon = 5e-16;
            bool isAlmostLong = Math.Abs(DisplayValue - valAsInt) < epsilon
                && !double.IsInfinity(DisplayValue) //no overflow occurred
                && Math.Abs(DisplayValue) < long.MaxValue; //the value can be represented within the interval of a long
            DisplayValueString = isAlmostLong ? ((long)DisplayValue).ToString("N0", culture) : DisplayValueString;
            decimalSeparatorInserted = DisplayValueString.Contains(decimalSeparator);
        }
        /// <summary>
        /// Reset the status of display in the CalcEngine. The memory is not affected.
        /// If a DisplayValue is provided it is loaded in the display
        /// </summary>
        /// <param name="DisplayValue">The new DisplayValue to load in the CalcEngine
        /// </param>
        public void ResetDisplayStatus(double? DisplayValue = null)
        {
            ClearCalculation();
            if (DisplayValue != null)
            {
                this.DisplayValue = DisplayValue.Value;
                DisplayValueString = this.DisplayValue.ToString(culture);
            }

        }
        /// <summary>
        /// A number button was pressed.
        /// </summary>
        /// <param name="number">The single digit</param>
        public void NumberPressed(int number)
        {
            if (!IsNewValue)
            {
                DisplayValueString += "" + number;
            }
            else
            {//it's a new value
                if (number != 0)
                { //if first digit is not 0
                    IsNewValue = false;
                }
                DisplayValueString = "" + number;
            }
            DisplayValue = double.Parse(DisplayValueString, culture);
            previousValueWasInserted = true;
            if (IsNewCalculation)
            {
                IsNewCalculation = false;
            }
            FormatDisplayValueStringBeforeResult();
        }

        /// <summary>
        /// The "left arrow" was pressed
        /// </summary>
        public void NumberBack()
        {
            if (!IsNewValue)
            {
                if (DisplayValueString.Length > 1)
                {
                    DisplayValueString = DisplayValueString[..^1];
                    //check whether we have only a sign without any digit
                    if (DisplayValueString.Length == 1 && (DisplayValueString.Contains('-') || DisplayValueString.Contains('+')))
                        DisplayValueString = "0";
                }
                else//we reached the first digit
                {
                    DisplayValueString = "0";
                    IsNewValue = true;
                }
                DisplayValue = double.Parse(DisplayValueString, culture);
                FormatDisplayValueStringBeforeResult();
            }
        }

        /// <summary>
        /// The decimal separator was inserted
        /// </summary>
        public void InsertDecimalSeparator()
        {
            if (!decimalSeparatorInserted)
            {
                if (!IsNewValue)
                {
                    DisplayValueString += decimalSeparator;
                }
                else//we have a new value starting with . or ,
                {
                    DisplayValueString = "0" + decimalSeparator;
                    IsNewValue = false;
                }
                decimalSeparatorInserted = true;
            }
        }

        /// <summary>
        /// Apply + operator
        /// </summary>
        public void Plus()
        {
            try
            {
                ApplyPreviousOperator();
            }
            finally
            {
                previousOperator = '+';
                IsNewValue = true;
                previousValueWasInserted = false;
            }
        }

        /// <summary>
        /// Apply - operator
        /// </summary>
        public void Minus()
        {
            try
            {
                ApplyPreviousOperator();
            }
            finally
            {
                previousOperator = '-';
                IsNewValue = true;
                previousValueWasInserted = false;
            }
        }

        /// <summary>
        /// Apply * operator
        /// </summary>
        public void Multiply()
        {
            try
            {
                ApplyPreviousOperator();
            }
            finally
            {
                previousOperator = '*';
                IsNewValue = true;
                previousValueWasInserted = false;
            }
        }

        /// <summary>
        /// Apply / operator
        /// </summary>
        public void Divide()
        {
            try
            {
                ApplyPreviousOperator();
            }
            finally
            {
                previousOperator = '/';
                IsNewValue = true;
                previousValueWasInserted = false;
            }
        }

        /// <summary>
        /// Perform the Sqrt of the displayValue.
        /// </summary>
        public void Sqrt()
        {//unary operator
         //since we have unary operator we need to select operator immediately
         //the result is given back upon selection of the Sqrt button and not
         //when Equals button is pressed.
            try
            {
                //verify if previousOperator is different form ' '
                if (previousOperator != ' ')
                {
                    //apply current operation first (sqrt) and then the previous, due to operation precedence
                    //save previous operation state
                    var previousOperatorBack = previousOperator;
                    var leftOperandBack = leftOperand;
                    //perform current operation (sqrt)
                    leftOperand = DisplayValue;
                    previousOperator = 's';
                    ApplyPreviousOperator();
                    //apply previous operator with the result of current operation
                    previousOperator = previousOperatorBack;
                    leftOperand = leftOperandBack;
                    ApplyPreviousOperator();
                }
                else
                {
                    previousOperator = 's'; //select Square
                    ApplyPreviousOperator();//perform Square
                }
                FormatAndRoundDisplayValueStringAfterResult();
            }
            finally
            {
                previousOperator = ' ';//set previous to null
                IsNewValue = true;
            }
        }



        /// <summary>
        /// Perform the Square of the displayValue. Unary operator
        /// </summary>
        public void Square()
        {//unary operator
         //since we have unary operator we need to select operator immediately
         //the result is given back upon selection of the Square button and not
         //when Equals button is pressed.
            try
            {
                //verify if previousOperator is different form ' '
                if (previousOperator != ' ')
                {
                    //apply current operation first and then the previous, due to 
                    //operation precedence
                    //save previous operation state
                    var previousOperatorBack = previousOperator;
                    var leftOperandBack = leftOperand;
                    //perform current operation
                    leftOperand = DisplayValue;
                    previousOperator = 'q';
                    ApplyPreviousOperator();
                    //apply previous operator with the result of current operation
                    previousOperator = previousOperatorBack;
                    leftOperand = leftOperandBack;
                    ApplyPreviousOperator();
                }
                else
                {
                    previousOperator = 'q'; //select Square
                    ApplyPreviousOperator();//perform Square
                }
                FormatAndRoundDisplayValueStringAfterResult();
            }
            finally
            {
                previousOperator = ' ';//set previous to null
                IsNewValue = true;
            }
        }

        /// <summary>
        /// Apply 1/x operator. Unary operator
        /// </summary>
        public void Reciprocal()
        {//unary operator
            try
            {
                //verify if previousOperator is different form ' '
                if (previousOperator != ' ')
                {
                    //apply current operation first and then the previous, due to 
                    //operation precedence
                    //save previous operation state
                    var previousOperatorBack = previousOperator;
                    var leftOperandBack = leftOperand;
                    //perform current operation
                    leftOperand = DisplayValue;
                    previousOperator = 'r';
                    ApplyPreviousOperator();
                    //apply previous operator with the result of current operation
                    previousOperator = previousOperatorBack;
                    leftOperand = leftOperandBack;
                    ApplyPreviousOperator();
                }
                else
                {
                    previousOperator = 'r'; //select Square
                    ApplyPreviousOperator();//perform Square
                }
                FormatAndRoundDisplayValueStringAfterResult();
            }
            finally
            {
                previousOperator = ' ';//set previous to null
                IsNewValue = true;
            }
        }

        /// <summary>
        /// Apply % operator
        /// </summary>
        public void Percentage()
        {
            //https://devblogs.microsoft.com/oldnewthing/20080110-00/?p=23853
            char previousOperatorBack = previousOperator;
            try
            {
                previousOperator = '%';
                ApplyPreviousOperator();
            }
            finally
            {
                previousOperator = previousOperatorBack;
                IsNewValue = true;
            }


        }

        /// <summary>
        /// Apply = operator
        /// </summary>
        public void Equals()
        {
            try
            {
                ApplyPreviousOperator();
                FormatAndRoundDisplayValueStringAfterResult();
            }
            finally
            {
                leftOperand = 0;
                previousOperator = ' ';
                IsNewValue = true;
                IsNewCalculation = true;
                decimalSeparatorInserted = false;
                previousValueWasInserted = false;
            }
        }

        /// <summary>
        /// Apply the 'C' (ClearCalculation). Clears all input to the calculator
        /// </summary>
        public void ClearCalculation()
        {
            DisplayValue = 0;
            DisplayValueString = "0";
            leftOperand = 0;
            previousOperator = ' ';
            IsNewValue = true;
            IsNewCalculation = true;
            decimalSeparatorInserted = false;
            previousValueWasInserted = false;
        }

        /// <summary>
        /// Clears the most recent entry (CE) in display
        /// </summary>
        public void ClearLastOperation()
        {
            IsNewValue = true;
            decimalSeparatorInserted = false;
            DisplayValue = 0;
            DisplayValueString = "0";
        }

        /// <summary>
        /// Stores valueToStore to memory. The Memory Store (MS) button click can be implemented by
        /// storing current display value to memory. 
        /// The last value inserted is placed on top (0 index) in the list
        /// </summary>
        /// <param name="valueToStore">The value to store in memory</param>
        public void MemoryStoreValue(double valueToStore)
        {
            MemoryValues.Insert(0, valueToStore);
            IsNewValue = true;//after operation we start as if we had a new value
        }

        /// <summary>
        /// Clear all data from memory.This corresponds to Memory Clear (MC) button click. 
        /// </summary>
        public void MemoryClearValue()
        {
            MemoryValues.Clear();
        }

        /// <summary>
        /// Memory Recall (MR). Recalls last value from the memory and store it in display.
        /// A value must be present in the memory, otherwise nothing is recalled
        /// </summary>
        /// <returns>true if a value was recalled from memory, false otherwise</returns>
        public bool MemoryRecallValue()
        {
            if (MemoryValues.Count > 0)
            {
                //we consider MemoryRecall as insertion of a new number which is taken from the store
                //DisplayValue = double.Parse(DisplayValueString, culture);
                DisplayValue = MemoryValues[0];//the first in the list is the last inserted (LIFO)
                IsNewValue = false;
                previousValueWasInserted = true;
                FormatDisplayValueStringBeforeResult();
                return true;
            }
            return false;

        }

        /// <summary>
        /// Add current valueToAdd to memory. The M+ button can be implemented by 
        /// adding current display value to memory
        /// </summary>
        /// <param name="valueToAdd">The value to add to memory</param>
        public void AddToMemory(double valueToAdd)
        {
            if (MemoryValues.Count > 0)
            {
                MemoryValues[0] += valueToAdd;
            }
            else //MemoryValues is empty --> the valueToAdd is inserted in the first position
            {
                MemoryValues.Insert(0, valueToAdd);
            }
            IsNewValue = true;//after operation we start as if we had a new value
        }

        /// <summary>
        /// Subtracts current valueToSubtract from memory. The M- button can be implemented by 
        /// subtracting current display value from memory
        /// </summary>
        /// <param name="valueToSubtract">The value to subtract from memory</param>
        public void SubFromMemory(double valueToSubtract)
        {
            if (MemoryValues.Count > 0)
            {
                MemoryValues[0] -= valueToSubtract;
            }
            else//MemoryValues is empty --> the valueToSubtract is inserted in the first position
            {
                MemoryValues.Insert(0, -valueToSubtract);
            }
            IsNewValue = true;//after operation we start as if we had a new value
        }

        /// <summary>
        /// Apply the immediately preceding operator to calculate an intermediate result.
        /// This will form the left operand of the new operator. This method is usually 
        /// called when an operator button has been pressed. 
        /// </summary>
        /// <exception cref="ArithmeticException">If calculation produces ArithmeticException</exception>
        /// <exception cref="OverflowException">If calculation produces OverflowException</exception>
        private void ApplyPreviousOperator()
        {
            switch (previousOperator)
            {
                case '+':
                    if (previousValueWasInserted)
                    {
                        leftOperand += DisplayValue;
                        DisplayValue = leftOperand;
                        previousValueWasInserted = false;
                    }
                    break;
                case '-':
                    if (previousValueWasInserted)
                    {
                        leftOperand -= DisplayValue;
                        DisplayValue = leftOperand;
                        previousValueWasInserted = false;
                    }
                    break;
                case '*':
                    if (previousValueWasInserted)
                    {
                        leftOperand *= DisplayValue;
                        DisplayValue = leftOperand;
                        previousValueWasInserted = false;
                    }
                    break;
                case '/'://we manage the case of division by 0, since with double it is legal
                    if (previousValueWasInserted)
                    {
                        if (DisplayValue != 0)
                        {
                            leftOperand /= DisplayValue;
                            DisplayValue = leftOperand;
                        }
                        else
                        {
                            ClearLastOperation();//as we didn't do the division
                            throw new ArithmeticException("Cannot divide by zero");
                        }
                        //not needed since we clear last operation
                        //previousValueWasInserted= false;
                    }
                    break;
                case 'q'://Square
                    leftOperand = Math.Pow(DisplayValue, 2);
                    DisplayValue = leftOperand;
                    break;
                case 's'://Sqrt
                    if (DisplayValue >= 0)
                    {
                        leftOperand = Math.Sqrt(DisplayValue);
                        DisplayValue = leftOperand;
                    }
                    else
                    {
                        ClearLastOperation();//as we didn't do the Sqrt
                        throw new ArithmeticException("Invalid input");
                    }
                    break;
                case 'r': //Reciprocal
                    if (DisplayValue != 0)
                    {
                        leftOperand = 1.0 / DisplayValue;
                        DisplayValue = leftOperand;
                    }
                    else
                    {
                        ClearLastOperation();//as we didn't do the division
                        throw new ArithmeticException("Cannot divide by zero");
                    }
                    break;
                case '%': //Percentage
                    DisplayValue = (DisplayValue / 100.0) * leftOperand;
                    break;
                default:// There was no preceding operator.
                    leftOperand = DisplayValue;
                    break;
            }
            if (IsNewCalculation)
            {
                IsNewCalculation = false;
            }
            //checked keyword only works for integral numbers. With floating point numbers you have to check IsInfinity in order to
            //catch overflow https://stackoverflow.com/questions/33760972/determine-if-operation-will-result-in-overflow
            bool overflowed = double.IsInfinity(leftOperand) || double.IsInfinity(DisplayValue);
            if (overflowed)
            {
                throw new OverflowException("Overflow");
            }
            FormatDisplayValueStringBeforeResult();
        }

        /// <summary>
        /// Invert the sign of displayValue
        /// </summary>
        public void InvertDisplaySign()
        {
            DisplayValue = -1 * DisplayValue;
            if (DisplayValueString.StartsWith('-'))
            {
                DisplayValueString = DisplayValueString[1..];
            }
            else
            {
                DisplayValueString = "-" + DisplayValueString;
            }
            FormatDisplayValueStringBeforeResult();
        }

    }
}