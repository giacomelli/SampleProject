using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectSample.Domain.Accounts;
using ProjectSample.Infrastructure.Framework.Domain;

namespace ProjectSample.Domain.UnitTests.Accounts
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void Clone_Properties_PropertiesCloned()
        {
            var target = new User()
            {
                Email = "test1@test.com",
                Enabled = true,
                FullName = "test fullname",
                Id = 1,
                Role = new Role() { Id = 2 },
                UserName = "test username"
            };

            var cloned = target.Clone() as User;

            Assert.IsFalse(object.ReferenceEquals(target, cloned));
            Assert.AreEqual("test1@test.com", cloned.Email);
            Assert.IsTrue(cloned.Enabled);
            Assert.AreEqual("test fullname", cloned.FullName);
            Assert.AreEqual(0, cloned.Id);
            Assert.AreEqual(2, cloned.Role.Id);
            Assert.AreEqual("test username", cloned.UserName);
            Assert.AreEqual(cloned.FullName, ((INamedEntity)cloned).Name);
        }

        [TestMethod]
        public void KeepImmutableValues_OldUser_KeepUsername()
        {
            var oldUser = new User()
            {
                UserName = "Old username"
            };

            var target = new User()
            {
                UserName = "New username"
            };

            target.KeepImmutableValues(oldUser);
            Assert.AreEqual("New username", target.UserName);

            target.Id = 1;
            target.KeepImmutableValues(oldUser);
            Assert.AreEqual("Old username", target.UserName);
        }
    }
}

