using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleProject.Infrastructure.Framework.Commons;

namespace SampleProject.Infrastructure.Framework.UnitTests.Commons
{
    [TestClass]
    public class RangeValueTest
    {
        [TestMethod]
        public void IsEmpty_True()
        {
            var target = new RangeValue<DateTime>();
            Assert.IsTrue(target.IsEmpty);
        }

        [TestMethod]
        public void IsIncomplete_True()
        {
            var target = new RangeValue<DateTime>();
            target.StartValue = DateTime.UtcNow;
            Assert.IsTrue(target.IsIncomplete);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void StartValue_GreaterThanEndValue_Exception()
        {
            var target = new RangeValue<DateTime>();
            target.EndValue = DateTime.UtcNow;
            target.StartValue = target.EndValue.Value.AddDays(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void EndValue_LesserThanStartValue_Exception()
        {
            var target = new RangeValue<DateTime>();
            target.StartValue = DateTime.UtcNow;
            target.EndValue = target.StartValue.Value.AddDays(-1);
        }

        [TestMethod]
        public void OperatorEquals_RangeValue_True()
        {
            var date = DateTime.UtcNow.Date;
            var target = new RangeValue<DateTime>();
            target.StartValue = date;
            target.EndValue = date.AddDays(1);

            var other = new RangeValue<DateTime>();
            other.StartValue = date;
            other.EndValue = date.AddDays(1);

            Assert.IsTrue(target == other);
        }

        [TestMethod]
        public void OperatorEquals_NullableRangeValue_True()
        {
            var target = new RangeValue<DateTime>?();
            var other = new RangeValue<DateTime>?();

            Assert.IsTrue(target == other);
        }

        [TestMethod]
        public void OperatorEquals_Nullable_NonNulableRangeValue_False()
        {
            var target = new RangeValue<DateTime>();
            RangeValue<DateTime>? other = null;

            Assert.IsFalse(target == other);
        }

        [TestMethod]
        public void OperatorEquals_RangeValue_False()
        {
            var date = DateTime.UtcNow.Date;
            var target = new RangeValue<DateTime>();
            target.StartValue = date;
            target.EndValue = date.AddDays(1);

            var other = new RangeValue<DateTime>();
            other.StartValue = date;
            other.EndValue = date.AddDays(2);

            Assert.IsTrue(target != other);
        }

        [TestMethod]
        public void Equals_Null_False()
        {
            var target = new RangeValue<DateTime>();

            Assert.IsFalse(target.Equals(null));
        }

        [TestMethod]
        public void Equals_DifferentType_False()
        {
            var target = new RangeValue<DateTime>();

            var other = new RangeValue<int>();

            Assert.IsFalse(target.Equals(other));
        }

        [TestMethod]
        public void GetHashCode_XorTwoDateTimesHashCode()
        {
            var target = new RangeValue<DateTime>();
            DateTime? startValue = DateTime.UtcNow.Date;
            DateTime? endValue = startValue.Value.AddDays(10);

            target.StartValue = startValue;
            target.EndValue = endValue;

            var expected = startValue.GetHashCode() ^ endValue.GetHashCode();

            var actual = target.GetHashCode();

            Assert.AreEqual(expected, actual);
        }
    }
}
