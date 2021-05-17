namespace FormsCalculator
{
    public class Memory
    {
        private double memory;

        public Memory()
        {
            this.memory = 0;
            this.IsSet = false;
        }

        public bool IsSet { get; private set; }

        public double Read()
        {
            return memory;
        }

        public void Clear()
        {
            this.memory = 0;
            this.IsSet = false;
        }

        public void Add(double value)
        {
            this.Set(memory + value);
        }

        public void Subtract(double value)
        {
            this.Set(memory - value);
        }

        private void Set(double value)
        {
            this.memory = value;
            this.IsSet = true;
        }
    }
}
