namespace FormsCalculator
{
    class Operand : IEvaluatable
    {
        private double value;
        public Operand(double value)
        {
            this.value = value;
        }
        
        public double Evaluate()
        {
            return value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}