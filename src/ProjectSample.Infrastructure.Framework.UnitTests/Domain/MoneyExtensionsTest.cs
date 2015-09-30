using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectSample.Infrastructure.Framework.Domain;

namespace ProjectSample.Infrastructure.Framework.UnitTests.Domain
{
    [TestClass]
    public class MoneyExtensionsTest
    {
        [TestMethod]
        public void Sum_Empty_DefaultMoney()
        {
            var source = new List<MoneyStub>();

            var actual = source.Sum(s => s.Value);

            Assert.AreEqual(0, actual.Amount);
            Assert.AreEqual("BRL", actual.Currency);
        }

        [TestMethod]
        public void Sum_Source_Money()
        {
            var source = new List<MoneyStub>()
            {
                new MoneyStub() { Value = Money.Reais(1) },
                new MoneyStub() { Value = Money.Reais(2) },
                new MoneyStub() { Value = Money.Reais(3) }
            };

            var actual = source.Sum(s => s.Value);

            Assert.AreEqual(6, actual.Amount);
            Assert.AreEqual("BRL", actual.Currency);
        }

        [TestMethod]
        public void Average_Empty_DefaultMoney()
        {
            var source = new List<MoneyStub>();

            var actual = source.Average(s => s.Value);

            Assert.AreEqual(0, actual.Amount);
            Assert.AreEqual("BRL", actual.Currency);
        }

        [TestMethod]
        public void Average_Source_Money()
        {
            var source = new List<MoneyStub>()
            {
                new MoneyStub() { Value = Money.Reais(1) },
                new MoneyStub() { Value = Money.Reais(2) },
                new MoneyStub() { Value = Money.Reais(3) }
            };

            var actual = source.Average(s => s.Value);

            Assert.AreEqual(2, actual.Amount);
            Assert.AreEqual("BRL", actual.Currency);
        }
    }
}
