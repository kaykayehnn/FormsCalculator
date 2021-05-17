using FormsCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorTests
{
    [TestClass]
    public class MemoryTest
    {
        private Memory memory;

        [TestInitialize]
        public void InitializeMemory()
        {
            memory = new Memory();
        }

        [TestMethod]
        public void TestAdd()
        {
            memory.Add(5);

            var result = memory.Read();
            var expected = 5;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestSubtract()
        {
            memory.Subtract(5);

            var result = memory.Read();
            var expected = -5;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestInitialIsSet()
        {
            var result = memory.IsSet;
            var expected = false;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestIsSet()
        {
            memory.Add(5);

            var result = memory.IsSet;
            var expected = true;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestClearUpdatesIsSet()
        {
            memory.Add(5);
            memory.Clear();

            var result = memory.IsSet;
            var expected = false;

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void TestAddAfterClear()
        {
            memory.Add(5);
            memory.Clear();
            memory.Add(6);

            var result = memory.Read();
            var expected = 6;

            Assert.AreEqual(result, expected);
        }
    }
}
