using HelperSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleProject.Domain.Accounts;
using SampleProject.Domain.Accounts.Specs;
using SampleProject.Infrastructure.Framework.Globalization;

namespace SampleProject.Domain.UnitTests.Accounts.Specs
{
    [TestClass]
    public class UserCanBeRemovedSpecTest
    {
        [TestMethod]
        public void IsSatisfiedBy_UserDoesNotExists_False()
        {
            var target = new UserCanBeRemovedSpec(1);
            Assert.IsFalse(target.IsSatisfiedBy(null));
            Assert.AreEqual(Texts.UserWithIdDoNotExists.With(1), target.NotSatisfiedReason);
        }

        [TestMethod]
        public void IsSatisfiedBy_UserExists_True()
        {
            var target = new UserCanBeRemovedSpec(1);
            Assert.IsTrue(target.IsSatisfiedBy(new User() { UserName = "123" }));
        }

        [TestMethod]
        public void IsSatisfiedBy_UserIsAdmin_False()
        {
            var target = new UserCanBeRemovedSpec(1);
            Assert.IsFalse(target.IsSatisfiedBy(new User() { Id = 1 }));
            Assert.AreEqual(Texts.SpecialUserCannotBeRemoved.With(UserService.AdminUserName), target.NotSatisfiedReason);
        }

        [TestMethod]
        public void IsSatisfiedBy_UserIsBackgroundworkerUser_False()
        {
            var target = new UserCanBeRemovedSpec(2);
            Assert.IsFalse(target.IsSatisfiedBy(new User() { Id = 2 }));
            Assert.AreEqual(Texts.SpecialUserCannotBeRemoved.With(UserService.BackgroundWokerUserName), target.NotSatisfiedReason);
        }
    }
}
