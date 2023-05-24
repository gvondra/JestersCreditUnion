using JestersCreditUnion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Test
{
    [TestClass]
    public class LoanNumberGeneratorTest
    {
        [TestMethod]
        public void GenerateTest()
        {
            string year = DateTime.Today.ToString("yy");
            LoanNumberGenerator generator = new LoanNumberGenerator();
            string number = generator.Generate();
            Assert.IsNotNull(number);
            Assert.IsTrue(Regex.IsMatch(number, @"^" + year + @"[0-9]{7}$", RegexOptions.None, TimeSpan.FromMilliseconds(200)));
        }

        [TestMethod]
        public void GenerateMultipleTest()
        {
            void InnerGenerate(
                LoanNumberGenerator generator, 
                SortedSet<string> numbers)
            {
                foreach (int i in Enumerable.Range(0, 10000))
                {
                    Assert.IsTrue(
                        numbers.Add(generator.Generate()));
                }
            }
            SortedSet<string> numbers = new SortedSet<string>();
            LoanNumberGenerator generator = new LoanNumberGenerator();
            Task generateTask = Task.Run(() => InnerGenerate(generator, numbers));
            InnerGenerate(generator, numbers);
            generateTask.Wait();
        }
    }
}
