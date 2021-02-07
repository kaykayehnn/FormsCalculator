using System;

namespace FormsCalculator
{
    public enum Operator
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Power,
        Sqrt,
        NegateSign,
        Percent
    }

    static class OperatorHelpers
    {
        public static int GetPriority(Operator op)
        {
            switch (op)
            {
                case Operator.Addition:
                case Operator.Subtraction:
                    return 1;
                case Operator.Multiplication:
                case Operator.Division:
                    return 2;
                case Operator.Power:
                case Operator.Sqrt:
                    return 3;
                default:
                    throw new Exception("Invalid operator: " + op);
            }
        }
    }
}