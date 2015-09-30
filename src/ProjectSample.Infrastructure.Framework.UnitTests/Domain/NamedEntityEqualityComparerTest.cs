using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectSample.Infrastructure.Framework.Domain;
using Rhino.Mocks;

namespace ProjectSample.Infrastructure.Framework.UnitTests.Domain
{
    [TestClass]
    public class NamedEntityEqualityComparerTest
    {
        [TestMethod]
        public void Equals_AnyNull_False()
        {
            var target = new NamedEntityEqualityComparer();
            Assert.IsFalse(target.Equals(null, MockRepository.GenerateMock<INamedEntity>()));
            Assert.IsFalse(target.Equals(MockRepository.GenerateMock<INamedEntity>(), null));
            Assert.IsFalse(target.Equals(null, null));
        }

        [TestMethod]
        public void Equals_DiffName_False()
        {
            var target = new NamedEntityEqualityComparer();
            var a = MockRepository.GenerateMock<INamedEntity>();
            a.Expect(e => e.Name).Return("a");

            var b = MockRepository.GenerateMock<INamedEntity>();
            b.Expect(e => e.Name).Return("b");

            Assert.IsFalse(target.Equals(a, b));
        }

        [TestMethod]
        public void Equals_EqualsName_True()
        {
            var target = new NamedEntityEqualityComparer();
            var a = MockRepository.GenerateMock<INamedEntity>();
            a.Expect(e => e.Name).Return("a");

            var b = MockRepository.GenerateMock<INamedEntity>();
            b.Expect(e => e.Name).Return("a");

            Assert.IsTrue(target.Equals(a, b));
        }

        [TestMethod]
        public void GetHashCode_NameNull_Zero()
        {
            var target = new NamedEntityEqualityComparer();
            var a = MockRepository.GenerateMock<INamedEntity>();
            a.Expect(e => e.Name).Return(null);

            Assert.AreEqual(0, target.GetHashCode(a));
        }

        [TestMethod]
        public void GetHashCode_Name_NameHashCode()
        {
            var target = new NamedEntityEqualityComparer();
            var a = MockRepository.GenerateMock<INamedEntity>();
            a.Expect(e => e.Name).Return("a");

            Assert.AreEqual("a".GetHashCode(), target.GetHashCode(a));
        }
    }
}
