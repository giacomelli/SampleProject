using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleProject.Infrastructure.Framework.Domain;
using SampleProject.Infrastructure.Framework.Globalization;
using TestSharp;

namespace SampleProject.Infrastructure.Framework.UnitTests.Domain
{
    [TestClass]
    public class MoneyTest
    {
        [TestMethod]
        public void Constructor_NoArgs_Defaults()
        {
            var target = new Money();
            Assert.AreEqual(0, target.Amount);
            Assert.AreEqual("R$", target.Currency);
        }

        [TestMethod]
        public void Constructor_Amount_DefaultCurrency()
        {
            var target = new Money(1);
            Assert.AreEqual(1, target.Amount);
            Assert.AreEqual("R$", target.Currency);
        }

        [TestMethod]
        public void Reais_Amount_RightAmountAndCurrency()
        {
            var actual = Money.Reais(113.45m);
            Assert.AreEqual(113.45m, actual.Amount);
            Assert.AreEqual("BRL", actual.Currency);
        }

        [TestMethod]
        public void USDollars_Amount_RightAmountAndCurrency()
        {
            var actual = Money.USDollars(113.45m);
            Assert.AreEqual(113.45m, actual.Amount);
            Assert.AreEqual("USD", actual.Currency);
        }

        [TestMethod]
        public void ToString_Reais_RightFormat()
        {
            var actual = Money.Reais(113.45m);
            Assert.AreEqual("R$ 113,45", actual.ToString());
        }

        [TestMethod]
        public void ToString_Dollars_RightFormat()
        {
            var actual = Money.USDollars(113.45m);
            Assert.AreEqual("$113.45", actual.ToString());
        }

        [TestMethod]
        public void OperatorSum_Reais_Added()
        {
            Money left = Money.Reais(10m);

            Money right = Money.Reais(7.23m);

            Money result = left + right;

            Assert.AreEqual(17.23m, result.Amount);
            Assert.AreEqual("BRL", result.Currency);
        }

        [TestMethod]
        public void OperatorSub_Reais_Subtracted()
        {
            Money left = Money.Reais(17.24m);

            Money right = Money.Reais(7.23m);

            Money result = left - right;

            Assert.AreEqual(10.01m, result.Amount);
            Assert.AreEqual("BRL", result.Currency);
        }

        [TestMethod]
        public void OperatorMult_ReaisInt_Multiplied()
        {
            Money left = Money.Reais(10.25m);

            int right = 4;

            Money result = left * right;

            Assert.AreEqual(41m, result.Amount);
            Assert.AreEqual("BRL", result.Currency);
        }

        [TestMethod]
        public void OperatorMult_ReaisDecimal_Multiplied()
        {
            Money left = Money.Reais(10.25m);

            decimal right = 1.5m;

            Money result = left * right;

            Assert.AreEqual(15.38m, result.Amount);
            Assert.AreEqual("BRL", result.Currency);
        }

        [TestMethod]
        public void OperatorSum_DifferentCurrencies_Exception()
        {
            Money left = Money.Reais(10m);

            Money right = Money.USDollars(7.23m);

            ExceptionAssert.IsThrowing(new InvalidOperationException(Texts.InvalidMoneyOperationDifferentCurrencies), () =>
            {
                Money result = left + right;
            });
        }

        [TestMethod]
        public void OperatorSub_DifferentCurrencies_Exception()
        {
            Money left = Money.Reais(17.24m);

            Money right = Money.USDollars(7.23m);

            ExceptionAssert.IsThrowing(new InvalidOperationException(Texts.InvalidMoneyOperationDifferentCurrencies), () =>
            {
                Money result = left - right;
            });
        }

        [TestMethod]
        public void SameCurrency_Reais_KeepsSameCurrency()
        {
            Money original = Money.Reais(10m);

            Money sameCurrency = Money.SameCurrency(original, 1m);

            Assert.AreEqual("BRL", sameCurrency.Currency);
            Assert.AreEqual(1m, sameCurrency.Amount);
        }

        [TestMethod]
        public void Equals_Money_True()
        {
            Money a = Money.Reais(15m);

            Money b = Money.Reais(15m);

            Assert.IsTrue(a.Equals(a));

            Assert.IsTrue(a.Equals(b));

            Assert.IsTrue(b.Equals(a));
        }

        [TestMethod]
        public void Equals_Money_False()
        {
            Money a = Money.Reais(15m);

            Money b = Money.Reais(15.5m);

            Assert.IsFalse(a.Equals(null));

            Assert.IsFalse(a.Equals(new object()));

            Assert.IsFalse(a.Equals(b));

            Assert.IsFalse(b.Equals(a));
        }

        [TestMethod]
        public void OpEquality_Money_True()
        {
            Money a = Money.Reais(15m);
            var aa = a;

            Money b = Money.Reais(15m);

            Assert.IsTrue(a == aa);

            Assert.IsTrue(a == b);

            Assert.IsTrue(b == a);
        }

        [TestMethod]
        public void OpEquality_Money_False()
        {
            Money a = Money.Reais(15m);

            Money b = Money.Reais(15.5m);

            Assert.IsFalse(a == null);

            Assert.IsFalse(a == b);

            Assert.IsFalse(b == a);
        }

        [TestMethod]
        public void OpInequality_Money_True()
        {
            Money a = Money.Reais(15m);

            Money b = Money.Reais(15.5m);

            Assert.IsTrue(a != null);

            Assert.IsTrue(a != b);

            Assert.IsTrue(b != a);
        }

        [TestMethod]
        public void OpInequality_Money_False()
        {
            Money a = Money.Reais(15m);

            Money b = Money.Reais(15m);

            Money c = a;

            Assert.IsFalse(a != c);

            Assert.IsFalse(a != b);

            Assert.IsFalse(b != a);

            Assert.IsFalse((Money)null != (Money)null);
        }

        [TestMethod]
        public void GetHashCode_NoArgs_Hashcode()
        {
            Money a = Money.Reais(15m);

            Money b = Money.Reais(15m);

            Money c = Money.Reais(15.5m);

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());

            Assert.AreNotEqual(a.GetHashCode(), c.GetHashCode());
        }

        [TestMethod]
        public void Add_Reais_Added()
        {
            Money left = Money.Reais(10m);

            Money right = Money.Reais(7.23m);

            Money result = left.Add(right);

            Assert.AreEqual(17.23m, result.Amount);
            Assert.AreEqual("BRL", result.Currency);
        }

        [TestMethod]
        public void Subtract_Reais_Subtracted()
        {
            Money left = Money.Reais(17.24m);

            Money right = Money.Reais(7.23m);

            Money result = left.Subtract(right);

            Assert.AreEqual(10.01m, result.Amount);
            Assert.AreEqual("BRL", result.Currency);
        }

        [TestMethod]
        public void Multiply_ReaisInt_Multiplied()
        {
            Money left = Money.Reais(10.25m);

            int right = 4;

            Money result = left.Multiply(right);

            Assert.AreEqual(41m, result.Amount);
            Assert.AreEqual("BRL", result.Currency);
        }

        [TestMethod]
        public void Multiply_ReaisDecimal_Multiplied()
        {
            Money left = Money.Reais(10.25m);

            decimal right = 1.5m;

            Money result = left.Multiply(right);

            Assert.AreEqual(15.38m, result.Amount);
            Assert.AreEqual("BRL", result.Currency);
        }

        [TestMethod]
        public void Divide_ReaisDecimal_Divided()
        {
            Money left = Money.Reais(10m);

            decimal right = 5m;

            Money result = left.Divide(right);

            Assert.AreEqual(2m, result.Amount);
            Assert.AreEqual("BRL", result.Currency);
        }
    }
}
