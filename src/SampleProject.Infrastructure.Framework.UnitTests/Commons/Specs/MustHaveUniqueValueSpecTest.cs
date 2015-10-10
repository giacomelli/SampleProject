using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleProject.Infrastructure.Framework.Commons.Specs;

namespace SampleProject.Infrastructure.Framework.UnitTests.Commons.Specs
{
    [TestClass]
    public class MustHaveUniqueValueSpecTest
    {
        #region Tests

        [TestMethod]
        public void IsSatisfiedBy_EnumProperty_NotUnique()
        {
            var spec = new MustHaveUniqueValueSpec<UniqueValueEnumPropertyTestClass, TestEnum>(x => x.Test, x => new UniqueValueEnumPropertyTestClass { Test = x });

            var testSubject = new UniqueValueEnumPropertyTestClass() { Test = TestEnum.TestValue };

            string name = typeof(UniqueValueEnumPropertyTestClass).Name.ToLower();
            string value = TestEnum.TestValue.ToString();

            Assert.IsFalse(spec.IsSatisfiedBy(testSubject));
        }

        [TestMethod]
        public void IsSatisfiedBy_EnumProperty_Unique()
        {
            var spec = new MustHaveUniqueValueSpec<UniqueValueEnumPropertyTestClass, TestEnum>(x => x.Test, x => null);

            var testSubject = new UniqueValueEnumPropertyTestClass() { Test = TestEnum.TestValue2 };

            string name = typeof(UniqueValueEnumPropertyTestClass).Name.ToLower();
            string value = TestEnum.TestValue.ToString();

            Assert.IsTrue(spec.IsSatisfiedBy(testSubject));
        }

        #endregion
    }
}
