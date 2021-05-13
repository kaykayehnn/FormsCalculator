using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCalculator
{
    public class Calculator
    {
        // Notes:
        // When pressing on an operator it should keep the same operand but we
        // will need a "Touched" field for current operand to know whether we need
        // to override or append.
        // When a new operator is selected set it as the right operand of equation
        // and if it is already populated make a new Operation and set Equation as
        // its left operand. TODO: how will this work with priorities
        // If next chosen operator has higher priority we need to rotate this ^
        // and move current rightOperand to a new Operation's left and set the new
        // one to previous' RightOperand.

        // Operation needs a ToString which must work even if right operand is not
        // already populated.

        // We need to handle repeatedly pressing equals and think how it should be
        // displayed in the equation bar.
        private const string DEFAULT_OPERAND = "0";
        private const string DEFAULT_DECIMAL_SEPARATOR = ".";
        private const string DEFAULT_THOUSANDS_SEPARATOR = ",";

        private readonly string decimalSeparator;
        private readonly string thousandsSeparator;

        private string currentOperand;
        private bool isOperandTouched;
        private bool showEquals;
        // TODO: this cannot be string because we wil eventually lose precision.
        // has to be a data structure
        // The equation field holds a tree-like data structure, where we need
        // to establish a few rules so that our application state remains valid.
        // Equations can consist of one, two or more operands and some
        // operations have a higher priority than others. When we append a new
        // node to our tree, depending on its priority compared to the current
        // node we are operating on:
        // 1) If new operation has higher priority than current one, we take
        //    the last operand from the current equation and then we create a
        //    new equation, and set it as the previous operation's right child.
        // 2) If current operation has same or equal priority

        private Operation equation;
        private PriorityMode priorityMode;

        public Calculator()
        {
            currentOperand = DEFAULT_OPERAND;
            isOperandTouched = false;
            showEquals = false;

            this.decimalSeparator = DEFAULT_DECIMAL_SEPARATOR;
            this.thousandsSeparator = DEFAULT_THOUSANDS_SEPARATOR;
        }

        public Calculator(CultureInfo cultureInfo) : this()
        {
            this.decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
            this.thousandsSeparator = cultureInfo.NumberFormat.NumberGroupSeparator;
        }


        public void SetPriorityMode(PriorityMode newMode)
        {
            this.priorityMode = newMode;
            this.Clear();
        }

        public void EnterKey(char character)
        {
            if (showEquals)
            {
                Clear();
            }
            if (character >= '0' && character <= '9')
            {
                // If the operand is 0, the key pressed overrides it completely.
                if (currentOperand == "0")
                {
                    currentOperand = character.ToString();
                }
                else
                {
                    if (isOperandTouched)
                    {
                        currentOperand += character;
                    }
                    else
                    {
                        currentOperand = character.ToString();
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid character received: " + character);
            }


            isOperandTouched = true;
            showEquals = false;
        }

        public void EnterDecimalSeparator()
        {
            if (!isOperandTouched)
            {
                // If not touched, enter leading zero.
                currentOperand = $"0{decimalSeparator}";
            }
            else
            {
                if (currentOperand.Contains(decimalSeparator))
                {
                    // Do nothing. Already contains one decimal separator.
                }
                else
                {
                    currentOperand += decimalSeparator;
                }
            }

            isOperandTouched = true;
            showEquals = false;
        }

        public void InvertSign()
        {
            // TODO: can we do this without using strings?
            if (this.currentOperand == DEFAULT_OPERAND) return;

            if (this.showEquals)
            {
                // TODO: delete previous history and invert sign
            }

            var operand = double.Parse(this.currentOperand);
            var inverted = -operand;

            this.currentOperand = inverted.ToString();
        }

        public void EnterOperator(Operator op)
        {
            // TODO: handle sqrt
            // TODO: handle percent sign
            // Base case
            if (equation == null)
            {
                var leftValue = double.Parse(this.currentOperand);
                var leftOperand = new Operand(leftValue);
                var eq = new Operation(op, leftOperand);

                this.equation = eq;
            }
            else
            {
                var deepestOperation = this.FindDeepestOperation();
                var rightValue = double.Parse(this.currentOperand);
                Operator currentOperator = deepestOperation.Operator;

                var currentPriority = OperatorHelpers.GetPriority(currentOperator, priorityMode);
                var nextOpPriority = OperatorHelpers.GetPriority(op, priorityMode);

                // If not touched, overwrite operator
                // TODO: after equals maybe this needs to be a special case
                if (!isOperandTouched)
                {
                    if (showEquals)
                    {
                        var result = new Operand(rightValue);
                        var newEquation = new Operation(op, result);
                        this.equation = newEquation;
                    }
                    else
                    {
                        if (nextOpPriority < currentPriority)
                        {
                            //throw new NotImplementedException();
                        }
                        else
                        {
                            deepestOperation.Operator = op;
                            // Append to equation
                            //throw new NotImplementedException();
                        }
                    }
                }
                else
                {
                    if (nextOpPriority > currentPriority)
                    {
                        // this might not be correct
                        var innerLeftOperand = new Operand(rightValue);
                        var newEquation = new Operation(op, innerLeftOperand);

                        deepestOperation.RightOperand = newEquation;
                    }
                    else
                    {
                        // should calculate only current tree, not everything
                        this.Calculate();
                        // TODO: evaluate and append operator to expression tree
                        // we need to append operand at the left side of the tree so that it
                        // is calculated in the correct order
                        // Replace rightOperand with new equation
                        //rightmostOperation.RightOperand = null;

                        //var deepestClone = new Operation(currentOperator, deepestOperation.LeftOperand);
                        //deepestClone.RightOperand = deepestOperation.RightOperand;

                        // TODO: update current operand with evaluated thing
                        //var newEquation = new Operation(currentOperator, deepestClone);
                        var newEquation = new Operation(currentOperator, deepestOperation.LeftOperand);
                        newEquation.RightOperand = deepestOperation.RightOperand;

                        deepestOperation.LeftOperand = newEquation;
                        deepestOperation.RightOperand = null;
                        deepestOperation.Operator = op;
                    }
                }
            }

            isOperandTouched = false;
            showEquals = false;
        }

        public void Percentify()
        {
            // TODO: the initial implementation of this operation will simply
            // divide the current operand by 100. Later we can improve this to
            // act as a modifier in other operations.

            // TODO: can we do this without using strings?
            if (this.showEquals)
            {
                // TODO: delete previous history and invert sign
            }

            var operand = double.Parse(this.currentOperand);
            var percent = operand / 100;

            this.currentOperand = percent.ToString();
        }

        public double Calculate()
        {
            if (equation == null)
            {
                var currentValue = double.Parse(currentOperand);
                // This is handled as a special case in GetEquation().

                showEquals = true;
                return currentValue;
            }
            else if (!isOperandTouched && equation.RightOperand != null)
            {
                // TODO: repeatedly pressing equals should perform same operation
                // replace all which is not leftmost with result and perform calculation
                Console.WriteLine(123);
                this.equation = FindDeepestOperation();

                var resultValue = double.Parse(currentOperand);
                var resultOperand = new Operand(resultValue);
                equation.LeftOperand = resultOperand;

                var result = equation.Evaluate();
                // TODO: save result somewhere in equation to prevent loss of precision
                currentOperand = result.ToString();
                showEquals = true;
                return result;
            }
            else
            {
                // TODO: Explain this
                // Append current operand to the final node of the tree.
                Operation deepestOperation = FindIncompleteNode();

                var rightValue = double.Parse(currentOperand);
                var rightmostOperand = new Operand(rightValue);
                deepestOperation.RightOperand = rightmostOperand;

                var result = equation.Evaluate();
                // TODO: save result somewhere to prevent loss of precision
                currentOperand = result.ToString();
                isOperandTouched = false;
                showEquals = true;
                return result;
            }
        }

        public void ClearEntry()
        {
            currentOperand = DEFAULT_OPERAND;
            if (showEquals)
            {
                // If operation is complete, clear equation as well.
                equation = null;
            }
            showEquals = false;
        }

        public void Clear()
        {
            equation = null;
            this.ClearEntry();
        }

        public void Backspace()
        {
            if (!isOperandTouched || currentOperand == DEFAULT_OPERAND)
            {
                return;
            }

            // Single-digit number, replace with zero.
            if (currentOperand.Length == 1)
            {
                currentOperand = DEFAULT_OPERAND;
            }
            else
            {
                // Trim last character from current.
                currentOperand = currentOperand.Substring(0, currentOperand.Length - 1);
            }
        }

        public string GetCurrentOperand()
        {
            string operand = currentOperand;

            // TODO: can we just use string.Format with current culture here?
            int wholePartLength = DecimalSeparatorIndex();
            if (wholePartLength == -1) wholePartLength = operand.Length;

            // Add thousands separators where needed
            int index = wholePartLength - 3;
            int endIndex = currentOperand.StartsWith("-") ? 1 : 0;
            while (index > endIndex)
            {
                operand = operand.Substring(0, index) + thousandsSeparator + operand.Substring(index);
                index -= 3;
            }

            return operand;
        }

        public string GetEquation()
        {
            if (equation == null)
            {
                if (showEquals)
                {
                    return currentOperand + " =";
                }

                return "";
            }

            var equationString = equation.ToString();
            if (showEquals) equationString += " =";

            return equationString;
        }

        private Operation FindDeepestOperation()
        {
            // TODO: handle if equation is empty
            if (equation == null)
            {
                throw new NotImplementedException("Equation is empty");
            }
            // 4 + 7 - 2 * 5
            //    -
            //  +   *
            // 4 7 2 5
            //

            // The following diagram shows how the equation tree changes
            // when entering operations of different priorities:
            // 4 + 7 - 2 * 5 ^ 2
            //
            //  +  ->   -  ->    -     ->     -
            // 4 7     + 2     +   *        +    *
            //        4 7     4 7 2 5      4 7  2  ^
            //                                    5 2

            // 2^5 * 7
            //    *
            //  ^  7
            // 2 5
            // TODO: explain this method
            Operation deepestOperation = equation;
            while (deepestOperation.RightOperand is Operation)
            {
                deepestOperation = (Operation)deepestOperation.RightOperand;
            }

            return deepestOperation;
        }

        private Operation FindIncompleteNode()
        {
            return FindIncompleteNode(this.equation);
        }

        // We need to traverse the whole tree here:
        private Operation FindIncompleteNode(Operation operation)
        {
            if (operation == null)
            {
                return null;
            }
            // If right operand is null, we have found the incomplete node.
            if (operation.RightOperand == null)
            {
                return operation;
            }

            // Search recursively in left tree
            var node = FindIncompleteNode(operation.LeftOperand as Operation);
            if (node == null)
            {
                // If not found, search recursively in right tree
                node = FindIncompleteNode(operation.RightOperand as Operation);
            }

            return node;
        }

        private int DecimalSeparatorIndex()
        {
            return currentOperand.IndexOf(decimalSeparator);
        }
    }
}
