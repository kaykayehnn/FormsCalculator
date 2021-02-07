using System;

namespace FormsCalculator
{
    class Operation : IEvaluatable
    {
        public Operation(Operator op, IEvaluatable leftOperand)
        {
            Operator = op;
            LeftOperand = leftOperand;
        }

        public Operator Operator { get; set; }
        public IEvaluatable LeftOperand { get; set; }
        public IEvaluatable RightOperand { get; set; }

        public double Evaluate()
        {
            var leftValue = LeftOperand.Evaluate();
            var rightValue = RightOperand.Evaluate();

            switch (Operator)
            {
                case Operator.Addition:
                    return leftValue + rightValue;
                case Operator.Subtraction:
                    return leftValue - rightValue;
                case Operator.Multiplication:
                    return leftValue * rightValue;
                case Operator.Division:
                    return leftValue / rightValue;
                case Operator.Power:
                    return Math.Pow(leftValue, rightValue);
                case Operator.Sqrt:
                    // TODO: is this correct??
                    return Math.Sqrt(leftValue);
                default:
                    throw new Exception("Invalid operation");
            }
        }

        public override string ToString()
        {
            var left = LeftOperand.ToString();
            var right = RightOperand?.ToString() ?? string.Empty;
            switch (Operator)
            {
                case Operator.Addition:
                    return $"{left} + {right}";
                case Operator.Subtraction:
                    return $"{left} - {right}";
                case Operator.Multiplication:
                    return $"{left} × {right}";
                case Operator.Division:
                    return $"{left} ÷ {right}";
                case Operator.Power:
                    return $"{left}^{right}";
                case Operator.Sqrt:
                    return $"√({left})";
                default:
                    throw new Exception("Invalid operation");
            }
        }
    }
}
