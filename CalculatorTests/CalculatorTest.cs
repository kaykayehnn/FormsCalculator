using System;
using FormsCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorTests
{
    [TestClass]
    public class CalculatorTest
    {
        private Calculator calc;

        [TestInitialize]
        public void InitializeCalculator()
        {
            calc = new Calculator();
        }

        [TestMethod]
        public void TestAddition()
        {
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('6');

            var result = calc.Calculate();
            var expected = 5 + 6;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestSubtraction()
        {
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Subtraction);
            calc.EnterKey('6');

            var result = calc.Calculate();
            var expected = 5 - 6;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestMultiplication()
        {
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('6');

            var result = calc.Calculate();
            var expected = 5 * 6;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestDivision()
        {
            calc.EnterKey('1');
            calc.EnterKey('2');
            calc.EnterOperator(Operator.Division);
            calc.EnterKey('3');

            var result = calc.Calculate();
            var expected = 12 / 3;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestPower()
        {
            calc.EnterKey('2');
            calc.EnterOperator(Operator.Power);
            calc.EnterKey('4');

            var result = calc.Calculate();
            var expected = Math.Pow(2, 4);

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestPowerZero()
        {
            calc.EnterKey('2');
            calc.EnterOperator(Operator.Power);
            calc.EnterKey('0');

            var result = calc.Calculate();
            var expected = Math.Pow(2, 0);

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestSqrt()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Sqrt);

            var result = calc.Calculate();
            var expected = Math.Sqrt(4);

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestComplexSqrt()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('4'); 
            calc.EnterOperator(Operator.Sqrt);

            var result = calc.Calculate();
            var expected = 4 + Math.Sqrt(4);

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestPercentSign()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Percent);

            var result = calc.Calculate();
            var expected = 0.04;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestInvertSign()
        {
            calc.EnterKey('4');
            calc.InvertSign();

            var result = calc.Calculate();
            var expected = -4;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestDecimalSeparator()
        {
            calc.EnterKey('2');
            calc.EnterDecimalSeparator();
            calc.EnterKey('3');
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('1');

            var result = calc.Calculate();
            var expected = 2.34 + 1;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestMultipleAddition()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('1');

            var result = calc.Calculate();
            var expected = 4 + 7 + 1;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestIntermediateAddition()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Addition);

            var result = calc.GetCurrentOperand();
            var expected = "11";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestEqualsBeforeCompletion()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Addition);
            calc.Calculate();

            var result = calc.GetEquation();
            var expected = "4 + 7 + 11 =";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestAdditionAndSubtraction()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Subtraction);
            calc.EnterKey('1');

            var result = calc.Calculate();
            var expected = 4 + 7 - 1;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestMultipleEqualsAddition()
        {
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('5');

            calc.Calculate(); // 10
            calc.Calculate(); // 15
            var result = calc.Calculate(); // 20
            var expected = 5 + 5 + 5 + 5;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestAdditionAndSubtractionHistory()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Subtraction);
            calc.EnterKey('1');
            calc.Calculate();

            var result = calc.GetEquation();
            var expected = "4 + 7 - 1 =";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Priority")]
        public void TestAdditionAndMultiplication()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('2');

            var result = calc.Calculate();
            var expected = 4 + 7 * 2;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestMultipleAdditions()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('6');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('8');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('9');

            var result = calc.Calculate();
            var expected = 4 + 5 + 6 + 7 + 8 + 9;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("History")]
        public void TestMultipleAdditionsHistory()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('6');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('8');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('9');

            calc.Calculate();

            var result = calc.GetEquation();
            var expected = "4 + 5 + 6 + 7 + 8 + 9 =";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Priority")]
        public void TestMultipleOperations()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('6');
            calc.EnterOperator(Operator.Division);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Subtraction);
            calc.EnterKey('8');
            calc.EnterOperator(Operator.Power);
            calc.EnterKey('9');

            var result = calc.Calculate();
            var expected = 4 + 5 * 6 / 7d - Math.Pow(8, 9);

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Priority")]
        public void TestIncreasingPriority()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('6');
            calc.EnterOperator(Operator.Power);
            calc.EnterKey('7');

            var result = calc.Calculate();
            var expected = 4 + 5 * Math.Pow(6, 7);

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Priority")]
        public void TestDecreasingPriority()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Power);
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('6');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');

            var result = calc.Calculate();
            var expected = Math.Pow(4, 5) * 6 + 7;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Priority")]
        public void TestMultipleOperationsMultipleEquals()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Power);
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('6');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('7');

            calc.Calculate();
            var result = calc.Calculate();
            var expected = (Math.Pow(4, 5) * 6 + 7) + 7;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Priority")]
        public void TestOperatorOverrideHigherPriority()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Division);
            calc.EnterOperator(Operator.Power);
            calc.EnterKey('2');

            var result = calc.Calculate();
            var expected = 4 * Math.Pow(5, 2);

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestTypeAfterEqualsShouldClear()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('5');
            calc.Calculate();
            calc.EnterKey('6');

            var result = calc.GetCurrentOperand();
            var expected = "6";

            Assert.AreEqual(result, expected);
            Assert.AreEqual(calc.GetEquation(), "");
        }

        [TestMethod]
        public void TestMultipleEquations()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('5');
            calc.Calculate(); // 4 * 5
            calc.EnterOperator(Operator.Subtraction);
            calc.EnterKey('6');

            var result = calc.Calculate();
            var expected = (4 * 5) - 6;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("History")]
        public void TestMultipleOperationsHistory()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('5');
            calc.EnterOperator(Operator.Multiplication);
            calc.EnterKey('6');
            calc.EnterOperator(Operator.Division);
            calc.EnterKey('7');
            calc.EnterOperator(Operator.Subtraction);
            calc.EnterKey('8');
            calc.EnterOperator(Operator.Power);
            calc.EnterKey('9');
            calc.Calculate();

            var result = calc.GetEquation();
            var expected = "4 + 5 × 6 ÷ 7 - 8^9 =";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestSimpleEquals()
        {
            calc.EnterKey('4');

            var result = calc.Calculate();
            var expected = 4;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestClearEntry()
        {
            calc.EnterKey('4');
            calc.ClearEntry();

            var result = calc.GetCurrentOperand();
            var expected = "0";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Clear")]
        public void TestClearEntryAfterOperator()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);

            calc.ClearEntry();

            var result = calc.GetCurrentOperand();
            var expected = "0";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Clear")]
        public void TestClearEntryAfterEquals()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('5');
            calc.Calculate();

            calc.ClearEntry();

            var result = calc.GetEquation();
            var expected = "";

            Assert.AreEqual(result, expected, "History was not cleared");
            Assert.AreEqual(calc.GetCurrentOperand(), "0", "Current operand was not cleared");
        }

        [TestMethod]
        [TestCategory("Clear")]
        public void TestClearAfterEquals()
        {
            calc.EnterKey('4');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('5');
            calc.Calculate();

            calc.Clear();

            var result = calc.GetEquation();
            var expected = "";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Clear")]
        public void TestSimpleEqualsHistory()
        {
            calc.EnterKey('4');
            calc.Calculate();

            var result = calc.GetEquation();
            var expected = "4 =";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [TestCategory("Formatting")]
        // This test needs to be locale-independent
        public void TestPositiveFormatting()
        {
            calc.EnterKey('1');
            calc.EnterKey('2');
            calc.EnterKey('3');
            calc.EnterKey('4');
            calc.EnterKey('5');
            calc.EnterDecimalSeparator();
            calc.EnterKey('6');
            calc.EnterKey('7');

            var result = calc.GetCurrentOperand();
            var expected = 12345.67.ToString("N");

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void ChangePriorityClearsState()
        {
            calc.EnterKey('1');
            calc.EnterKey('2');
            calc.SetPriorityMode(PriorityMode.Algebraic);

            var result = calc.GetCurrentOperand();
            var expected = "0";

            Assert.AreEqual(result, expected);
        }

        // TODO: add invert sign tests after equals
        [TestMethod]
        public void InvertSign()
        {
            calc.EnterKey('1');
            calc.InvertSign();

            var result = calc.GetCurrentOperand();
            var expected = "-1";

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void InvertSignInOperation()
        {
            calc.EnterKey('1');
            calc.EnterOperator(Operator.Addition);
            calc.EnterKey('4');
            calc.InvertSign();

            var result = calc.Calculate();
            var expected = -3;

            Assert.AreEqual(result, expected);
        }
    }
}
