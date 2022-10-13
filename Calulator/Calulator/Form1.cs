using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calulator
{
    public partial class Form1 : Form
    {
        private List<string> inputs = new List<string>();
        private string inputText;
        private List<string> splitInputs = new List<string>();

        bool errorCode = false;
        bool finOutput = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region Calculation Logic Methods
        private string Calculate(List<string> input)
        {
            if (SecondaryErrorChecks(out string error))
            {
                txtDisplay.Text = error;
                return null;
            }



            input.Reverse();
            Stack<string> givenStack = new Stack<string>(input);


            List<string> outputList = new List<string>();
            List<string> tempList = new List<string>();

            int mainCount = givenStack.Count;

            bool startTempStore = false;
            bool noBrackets = false;


            while (givenStack.Count != 1 || givenStack.Peek().EndsWith('!') == true)
            {
                // Check if input has any brackets
                if (NumberOfParenthesesPairs(new List<string>(givenStack)) <= 0)
                    noBrackets = true;

                for (int i = 0; i < mainCount; i++)
                {
                    string currVal = givenStack.Pop();

                    // Initiates storing of values within parentheses
                    if (currVal == "(")
                    {
                        startTempStore = true;

                        // Adds all the values within the parentheses (including the parentheses)
                        // into outputList to be calculated
                        if (tempList.Count > 0)
                        {
                            for (int j = 0; j < tempList.Count; j++)
                            {
                                outputList.Add(tempList[j]);
                            }
                            tempList.Clear();
                        }
                    }
                    if (currVal == "{")
                    {
                        startTempStore = true;

                        // Adds all the values within the parentheses (including the parentheses)
                        // into outputList to be calculated
                        if (tempList.Count > 0)
                        {
                            for (int j = 0; j < tempList.Count; j++)
                            {
                                outputList.Add(tempList[j]);
                            }
                            tempList.Clear();
                        }
                    }

                    // Stores all values within the parentheses (including the parentheses)
                    if (startTempStore)
                        tempList.Add(currVal);

                    // Begins calculation of values within the parentheses
                    if (currVal == ")" || currVal == "}" && startTempStore)
                    {
                        // Stores values in a stack and creates an empty stack
                        startTempStore = false;
                        List<string> list = new List<string>(tempList);

                        // First, do exponent calculation
                        Stack<string> firstOutput = ApplyLogAndLn(list.ToList());
                        // Secondly, do logarithms and ln 
                        Stack<string> secondOutput = ApplyExponent(firstOutput.ToList());
                        // Thirdly, do cosine, sine and tangent
                        Stack<string> thirdOutput = ApplyCosSinTan(secondOutput.ToList());
                        // Fourthly, do multiplication and division
                        Stack<string> fourthOutput = ApplyMultiplicationOrDivision(thirdOutput.ToList());
                        // Finally, do addition and subtraction
                        Stack<string> finalOutput = ApplyAdditionOrSubtraction(fourthOutput.ToList());

                        // finished calculating the temp list (within bracket calculations)
                        tempList.Clear();

                        // Return the final result
                        currVal = finalOutput.Pop();
                    }
                    else if (noBrackets)
                    {
                        givenStack.Push(currVal);
                        List<string> list = new List<string>(givenStack);

                        // First, do exponent calculation
                        Stack<string> firstOutput = ApplyLogAndLn(list.ToList());
                        // Secondly, do logarithms and ln 
                        Stack<string> secondOutput = ApplyExponent(firstOutput.ToList());
                        // Thirdly, do cosine, sine and tangent
                        Stack<string> thirdOutput = ApplyCosSinTan(secondOutput.ToList());
                        // Fourthly, do multiplication and division
                        Stack<string> fourthOutput = ApplyMultiplicationOrDivision(thirdOutput.ToList());
                        // Finally, do addition and subtraction
                        Stack<string> finalOutput = ApplyAdditionOrSubtraction(fourthOutput.ToList());

                        // Return the final result
                        return finalOutput.Pop();
                    }

                    if (startTempStore)
                        continue;

                    outputList.Add(currVal);
                }

                outputList.Reverse();
                givenStack = new Stack<string>(outputList);
                mainCount = givenStack.Count;
                outputList.Clear();
            }

            return givenStack.Pop();
        }
        private Stack<string> ApplyAdditionOrSubtraction(List<string> input)
        {
            Stack<string> inputStack = new Stack<string>(input);
            Stack<string> outputStack = new Stack<string>();
            int count = inputStack.Count;
            string currVal;
            string nextVal;
            string prevVal;
            double sum;

            for (int j = 0; j < count; j++)
            {
                currVal = inputStack.Pop();

                // always will hit this code block!!!!
                if (currVal == "(" || currVal == ")")
                {
                    if (currVal == "(")
                        continue;

                    return outputStack;
                }

                if (currVal == "{" || currVal == "}")
                {
                    if (currVal == "{")
                        continue;

                    return outputStack;
                }

                if (currVal == "+" || currVal == "-")
                {
                    if (outputStack.Count <= 0 || inputStack.Count <= 0 )
                    {
                        outputStack.Push(currVal);
                        continue;
                    }

                    prevVal = outputStack.Pop();
                    nextVal = inputStack.Pop();

                    if (currVal == "+")
                        if (nextVal == "-")
                        {
                            nextVal = inputStack.Pop();
                            sum = double.Parse(prevVal) - double.Parse(nextVal);
                            j++;
                        }
                        else 
                            sum = double.Parse(prevVal) + double.Parse(nextVal);
                    else
                        sum = double.Parse(prevVal) - double.Parse(nextVal);

                    outputStack.Push(sum.ToString());
                    j++;
                    continue;
                }

                outputStack.Push(currVal);
            }

            return outputStack;
        }

        /// Returns the stack of string values remaining after performing multiplication/division.
        private Stack<string> ApplyMultiplicationOrDivision(List<string> input)
        {
            // Stores values in a stack and creates an empty stack
            Stack<string> inputStack = new Stack<string>(input);
            Stack<string> outputStack = new Stack<string>();
            int count = inputStack.Count;
            string currVal;
            string nextVal;
            string prevVal;
            double sum;

            for (int j = 0; j < count; j++)
            {
                currVal = inputStack.Pop();

                // always will hit this code block!!!!
                if (currVal == "(" || currVal == ")")
                {
                    if (currVal == "(" )
                        continue;

                    return outputStack;
                }

                if (currVal == "{" || currVal == "}")
                {
                    if (currVal == "{")
                        continue;

                    return outputStack;
                }

                if (currVal == "*" || currVal == "/")
                {
                    prevVal = outputStack.Pop();
                    nextVal = inputStack.Pop();

                    if (currVal == "*")
                        sum = double.Parse(prevVal) * double.Parse(nextVal);
                    else
                        sum = double.Parse(prevVal) / double.Parse(nextVal);

                    outputStack.Push(sum.ToString());
                    j++;
                    continue;
                }

                outputStack.Push(currVal);
            }

            return outputStack;
        }


        /// Returns the stack of string values remaining after performing log and ln.
        private Stack<string> ApplyLogAndLn(List<string> input)
        {

            // Stores values in a stack and creates an empty stack
            input.Reverse();
            Stack<string> inputStack = new Stack<string>(input);
            Stack<string> outputStack = new Stack<string>();
            int count = inputStack.Count;
            string currVal;
            string nextVal;
            double sum;

            for (int j = 0; j < count; j++)
            {
                currVal = inputStack.Pop();

                // always will hit this code block!!!!
                if (currVal == "(" || currVal == ")")
                {
                    if (currVal == "(" )
                        continue;

                    return outputStack;
                }
                if (currVal == "{" || currVal == "}")
                {
                    if (currVal == "{")
                        continue;

                    return outputStack;
                }

                if (currVal == "log" || currVal == "ln")
                {
                    nextVal = inputStack.Pop();

                    if (currVal == "log")
                        sum = Math.Log10(double.Parse(nextVal));
                    else
                        sum = Math.Log(double.Parse(nextVal));

                    outputStack.Push(sum.ToString());
                    j++;
                    continue;
                }

                outputStack.Push(currVal);
            }

            return outputStack;
        }

        /// Returns the stack of string values remaining after performing cos, sin and tan calculations.
        private Stack<string> ApplyCosSinTan(List<string> input)
        {
            // Stores values in a stack and creates an empty stack
            Stack<string> inputStack = new Stack<string>(input);
            Stack<string> outputStack = new Stack<string>();
            int count = inputStack.Count;
            string currVal;
            string nextVal;
            double sum;

            for (int j = 0; j < count; j++)
            {
                currVal = inputStack.Pop();

                // always will hit this code block!!!!
                if (currVal == "(" || currVal == ")")
                {
                    if (currVal == "(")
                        continue;

                    return outputStack;
                }
                if (currVal == "{" || currVal == "}")
                {
                    if (currVal == "{")
                        continue;

                    return outputStack;
                }
                if (currVal == "sin" || currVal == "cos" || currVal == "tan" || currVal =="cot")
                {
                    nextVal = inputStack.Pop();


                    float val = float.Parse(nextVal);

                    if (currVal == "sin")
                        sum = MathF.Sin(val);
                    else if (currVal == "cos")
                        sum = MathF.Cos(val);
                    else if (currVal == "tan")
                        sum = MathF.Tan(val);
                    else
                        sum = MathF.Cos(val)/MathF.Sin(val);


                    outputStack.Push(sum.ToString());
                    j++;
                    continue;
                }

                outputStack.Push(currVal);
            }

            return outputStack;
        }

        /// Returns the stack of string values remaining after performing exponent calculations.
        private Stack<string> ApplyExponent(List<string> input)
        {
            // Stores values in a stack and creates an empty stack
            Stack<string> inputStack = new Stack<string>(input);
            Stack<string> outputStack = new Stack<string>();
            int count = inputStack.Count;
            string currVal;
            string nextVal;
            string prevVal;
            double sum=1;

            for (int j = 0; j < count; j++)
            {
                currVal = inputStack.Pop();


                // always will hit this code block!!!!
                if (currVal == "(" || currVal == ")")
                {
                    if (currVal == "(")
                        continue;

                    return outputStack;
                }

                if (currVal == "{" || currVal == "}")
                {
                    if (currVal == "{")
                        continue;

                    return outputStack;
                }

                if (currVal == "^")
                {
                    prevVal = outputStack.Pop();
                    nextVal = inputStack.Pop();

                    for (int k = 0; k < double.Parse(nextVal); k++)
                        sum *= double.Parse(prevVal);
                    //sum = Math.Pow(double.Parse(prevVal), double.Parse(nextVal));

                    outputStack.Push(sum.ToString());
                    j++;
                    continue;
                }

                outputStack.Push(currVal);
            }

            return outputStack;
        }


        #endregion
        #region Error Check Methods


        /// Outputs any errors before applying calculation.
        private bool PrimaryErrorCheck(List<string> input, out string error)
        {
            error = null;
            if (input[0].Equals("+") || input[0].Equals("*") || input[0].Equals("/") ||
              input[0].Equals("^"))
            {
                errorCode = true;
                error = "Format Error - cannot start calculation with operators";
                return true;
            }
            if (input[0].Equals("-") && input.Count <= 1)
            {
                errorCode = true;
                error = "Format Error - need to insert values";
                return true;
            }
            return false;
        }


        /// Outputs any errors due to invalid entries.
        private bool SecondaryErrorChecks(out string error)
        {
            error = null;

            bool hasNumber = false;
            bool unevenBrackets = false;
            bool startsWithOperator = false;
            bool noFollowingValueAfterOperator = false;
            int needClosingBracket = 0;

            int leftBracCount = inputs.FindAll(x => x == "(").Count +
                inputs.FindAll(x => x == "log(").Count + inputs.FindAll(x => x == "ln(").Count +
                inputs.FindAll(x => x == "cos(").Count + inputs.FindAll(x => x == "sin(").Count +
                inputs.FindAll(x => x == "tan(").Count + inputs.FindAll(x => x == "cot(").Count;
            int rightBracCount = inputs.FindAll(x => x == ")").Count;

            int leftCurlCount = inputs.FindAll(x => x == "{").Count;
            int rightCurlCount = inputs.FindAll(x => x == "}").Count;


            bool isOperator = false;


            // ensures input is not empty
            if (splitInputs.Count <= 0) return true;
            if (IsOperator(splitInputs[0]))
                startsWithOperator = true;
            if (splitInputs.Contains("(") || splitInputs.Contains(")") || splitInputs.Contains("{") || splitInputs.Contains("}"))
                if (leftBracCount != rightBracCount && leftCurlCount != rightCurlCount)
                    unevenBrackets = true;

            for (int i = 0; i < splitInputs.Count; i++)
            {
             
                if (IsNumber(splitInputs[i]))
                {
                    hasNumber = true;
                    isOperator = false;
                }
                if (IsOperator(splitInputs[i]) || splitInputs[i] == "^")
                {
                    if (isOperator)
                    {
                        if (splitInputs[i] == "-" && splitInputs[i - 1] == "+" && noFollowingValueAfterOperator != true)
                            noFollowingValueAfterOperator = false;
                    }
                    else if (splitInputs[i] == "^" && i + 1 != splitInputs.Count)
                        if (IsNumber(splitInputs[i + 1]))
                            noFollowingValueAfterOperator = false;
                        else
                            noFollowingValueAfterOperator = true;
                    else
                        noFollowingValueAfterOperator = true;
                    if (i < splitInputs.Count - 1)
                        isOperator = true;
                }
                if (IsNumber(splitInputs[i]) || splitInputs[i] == "(" || splitInputs[i] == ")" || splitInputs[i] == "{" || splitInputs[i] == "}")
                    isOperator = false;
                if (splitInputs[i] == "(")
                {
                    needClosingBracket++;
                    if (i + 1 >= splitInputs.Count)
                    {
                        errorCode = true;
                        error = "Format Error - must contain number(s) between brackets";
                        return true;
                    }
                    if (splitInputs[i + 1] == ")")
                    {
                        errorCode = true;
                        error = "Format Error - must contain number(s) between brackets";
                        return true;
                    }
                }
                if (splitInputs[i] == "{")
                {
                    needClosingBracket++;
                    if (i + 1 >= splitInputs.Count)
                    {
                        errorCode = true;
                        error = "Format Error - must contain number(s) between brackets";
                        return true;
                    }
                    if (splitInputs[i + 1] == "}")
                    {
                        errorCode = true;
                        error = "Format Error - must contain number(s) between brackets";
                        return true;
                    }
                }
                if (splitInputs[i] == ")" && needClosingBracket <= 0)
                {
                    errorCode = true;
                    error = "Format Error - need an opening bracket for each closing bracket";
                    return true;
                }
                if (splitInputs[i] == ")"  && needClosingBracket > 0)
                    needClosingBracket--;
                if (splitInputs[i] == "}" && needClosingBracket <= 0)
                {
                    errorCode = true;
                    error = "Format Error - need an opening bracket for each closing bracket";
                    return true;
                }
                if (splitInputs[i] == "}" && needClosingBracket > 0)
                    needClosingBracket--;
            }



            if (!hasNumber)
            {
                errorCode = true;
                error = "Format Error - must contain numbers";
                return true;
            }
            else if (startsWithOperator)
            {
                errorCode = true;
                error = "Format Error - cannot start calculation with an operator";
                return true;
            }
            else if (unevenBrackets || needClosingBracket > 0)
            {
                errorCode = true;
                error = "Format Error - missing bracket(s)";
                return true;
            }
            else if (noFollowingValueAfterOperator || isOperator)
            {
                errorCode = true;
                error = "Format Error - need a valid value after an operator";
                return true;
            }

            return false;
        }

        #endregion
        #region Helper Methods
        /// Returns true if the given string value is a number.
        private bool IsNumber(string input)
        {
            bool check = float.TryParse(input, out float r);
            return check;
        }


        /// Returns true if the given string value is an operator.
        private bool IsOperator(string val)
        {
            if (val == "+" || val == "-" || val == "/" || val == "*" || val == "^")
                return true;
            return false;
        }


        /// Returns the inputted values as fragments which can then be calculated.
        private List<string> SplitInput(string input)
        {
            if (input.Length <= 0)
                return null;

            string fragment = null;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '+' || input[i] == '-' || input[i] == '/' || input[i] == '*' ||
                  input[i] == '(' || input[i] == ')' || input[i] == '{' || input[i] == '}' || input[i] == '^' || input[i] == '%' )
                {
                    if (fragment != null && fragment != "")
                    {
                        splitInputs.Add(fragment);
                        fragment = null;
                    }

                    splitInputs.Add(input[i].ToString());
                    continue;
                }

                if (IsNumber(input[i].ToString()) && i + 1 < input.Length)
                {
                    if (IsNumber(input[i + 1].ToString()) == false && input[i + 1].ToString() != "." && input[i + 1].ToString() != "!")
                    {
                        fragment += input[i];
                        splitInputs.Add(fragment);
                        fragment = null;
                        continue;
                    }
                }

                fragment += input[i];

            }

            if (fragment != null && fragment != "")
                splitInputs.Add(fragment);


            return splitInputs;
        }

        /// Adds multiplication symbol in between variable/numbers with missing operators. Ex. pipi => pi*pi
        private void ReformatInput(List<string> inputList)
        {
            for (int i = 0; i < inputList.Count; i++)
            {
                // adds * before
                if (inputList[i] == "ln" && i - 1 >= 0
                  || inputList[i] == "log" && i - 1 >= 0
                  || inputList[i] == "sin" && i - 1 >= 0
                  || inputList[i] == "cos" && i - 1 >= 0
                  || inputList[i] == "tan" && i - 1 >= 0
                  || inputList[i] == "cot" && i - 1 >= 0)
                {
                    if (!IsOperator(inputList[i - 1]) && inputList[i - 1] != "(" && inputList[i - 1] != "{")
                        inputList.Insert(i, "*");
                }
                if (inputList[i] == "(" && i - 1 >= 0)
                {
                    if (inputList[i - 1] != "ln" && inputList[i - 1] != "log"
                      && inputList[i - 1] != "cos" && inputList[i - 1] != "sin" && inputList[i - 1] != "tan"
                      && inputList[i - 1] != "cot" && inputList[i - 1] != "^" && !IsOperator(inputList[i - 1]))
                        inputList.Insert(i, "*");
                }
                // adds * after
                if (inputList[i] == ")" && i + 1 < inputList.Count)
                {
                    if (!IsOperator(inputList[i + 1]) && inputList[i + 1] != ")")
                        inputList.Insert(i + 1, "*");
                }
                if (inputList[i] == "}" && i + 1 < inputList.Count)
                {
                    if (!IsOperator(inputList[i + 1]) && inputList[i + 1] != "}")
                        inputList.Insert(i + 1, "*");
                }
                if (inputList[i] == "-" && i + 1 < inputList.Count)
                {
                    if (inputList[i + 1] == "(" || inputList[i + 1] == "{")
                        inputList.Insert(i + 1, "1");

                }
                if (inputList[0] == "-" && i + 1 < inputList.Count)
                {
                    if (IsNumber(inputList[1]))
                    {
                        inputList[0] = "-" + inputList[1];
                        inputList.RemoveAt(1);
                    }
                }
            }
        }

        /// Returns string with decimal places applied.
        private string ApplyDecimalPlace(double val, int place, bool isNegative)
        {
            int counter = place - 1;
            string result = "";

            if (isNegative)
            {
                result = "0.";
                while (counter > 0)
                {
                    result += "0";
                    counter--;
                }
                return result += val;
            }

            counter = place;
            result = val.ToString();

            while (counter > 0)
            {
                result += "0";
                counter--;
            }
            return result;
        }


        /// Returns the number of paranthesis pairs as an int value within the given string values. 
        /// Returns Format Error if there is not a closing tag for each opening tag.
        private int NumberOfParenthesesPairs(List<string> inputs)
        {
            // Check if there are even amounts of "(" and ")" 
            if (inputs.Contains("(") && inputs.Contains(")"))
            {
                int openParaCount = inputs.FindAll(p => p == "(").Count + inputs.FindAll(p => p == "{").Count;
                int closeParaCont = inputs.FindAll(p => p == ")").Count + inputs.FindAll(p => p == "}").Count;

                if (openParaCount == closeParaCont)
                {
                    Console.WriteLine("Paranthesis Pairs Count: " + openParaCount);
                    return openParaCount;
                }
                else
                    throw new FormatException("Each opening paranthesis should be closed with the closing paranthesis!");
            }
            else
                return 0;
        }
        private void ClearOutput()
        {
            errorCode = false;
            txtDisplay.Text = "";
            inputs.Clear();
            splitInputs.Clear();
        }




        #endregion

        #region Button Mapping

        private void button_Decimal(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            Button num = (Button)sender;
            txtDisplay.Text += num.Text;
            inputs.Add(".");

        }

        private void button_Negative(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "-"; 
            inputs.Add("-");

        }

        private void button_Equal(object sender, EventArgs e)
        {
            string inputText = txtDisplay.Text;
            if (inputText.Length <= 0) return;

            List<string> input = SplitInput(inputText);

            ReformatInput(input);
            if (PrimaryErrorCheck(input, out inputText))
            {
                txtDisplay.Text = inputText;
                return;
            }

            string result = Calculate(input);

            if (result == null)
                return;

            ClearOutput();

            string calculation = new StringBuilder(this.inputText + " = " + result).ToString();

            txtDisplay.Text = result;
            finOutput = true;


        }

        private void button_Plus(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if(finOutput==true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "+";
            inputs.Add("+");

        }

        private void button_Minus(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "-";
            inputs.Add("-");

        }

        private void button_Mult(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "*";
            inputs.Add("*");

        }

        private void button_Div(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }


            txtDisplay.Text += "/";
            inputs.Add("/");

        }

        private void button_Back(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            if (inputs.Count <= 0) return;

            inputs.RemoveAt(inputs.Count - 1);
            txtDisplay.Text = "";

            for (int i = 0; i < inputs.Count; i++)
            {
                txtDisplay.Text += inputs[i];
            }
        }

        private void button_Clear(object sender, EventArgs e)
        {
            ClearOutput();
        }

        private void button_power(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "^"; 
            inputs.Add("^");

        }

        private void button_Nat_Log(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "ln(";
            inputs.Add("ln(");

        }

        private void button_Log(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "log(";
            inputs.Add("log(");

        }

        private void button_Cos(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }


            txtDisplay.Text += "cos(";
            inputs.Add("cos(");
        }

        private void button_Sin(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "sin(";
            inputs.Add("sin(");

        }

        private void button_Tan(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "tan(";
            inputs.Add("tan(");

        }

        private void button_Cot(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "cot(";
            inputs.Add("cot(");

        }

        private void button_Left_Curl(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "{";
            inputs.Add("{");

        }

        private void button_Right_Curl(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "}";
            inputs.Add("}");
        }

        private void button_Left_Para(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += "(";
            inputs.Add("(");

        }

        private void button_Right_Para(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            txtDisplay.Text += ")";
            inputs.Add(")");
        }

        private void button_Num(object sender, EventArgs e)
        {
            if (errorCode)
                ClearOutput();
            if (finOutput == true)
            {
                ClearOutput();
                finOutput = false;
            }

            Button num = (Button)sender;
            txtDisplay.Text += num.Text;
            inputs.Add(num.Text);

        }
        #endregion

    }
}
