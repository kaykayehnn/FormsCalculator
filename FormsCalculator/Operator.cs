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
    }

    static class OperatorHelpers
    {
        public static int GetPriority(Operator op, PriorityMode priorityMode)
        {
            if(priorityMode == PriorityMode.LeftToRight)
            {
                // All operations have equal priority in LeftToRight mode, so
                // we return a constant priority for all of them.
                return 0;
            }

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