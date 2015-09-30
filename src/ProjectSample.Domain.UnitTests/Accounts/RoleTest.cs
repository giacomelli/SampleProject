using ProjectSample.Domain.Accounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectSample.Domain.UnitTests.Accounts
{
    [TestClass]
    public class RoleTest
    {
        [TestMethod]
        public void Clone_Properties_PropertiesCloned()
        {
            var target = new Role()
            {
                Name = "test name",
                Id = 1,
                Description = "test description"
            };

            var cloned = target.Clone() as Role;

            Assert.IsFalse(object.ReferenceEquals(target, cloned));
            Assert.AreEqual("test name", cloned.Name);
            Assert.AreEqual(0, cloned.Id);
            Assert.AreEqual("test description", cloned.Description);
        }

        [TestMethod]
        public void CanAccess_NoControlerActionPermission_False()
        {
            var target = new Role()
            {
                Permissions = new Permission[] 
                {
                    new Permission() { ControllerName = "C1", ActionName = "A1"},
                    new Permission() { ControllerName = "C1", ActionName = "A2"}
                }
            };

            Assert.IsFalse(target.CanAccess("C1", "A3"));
            Assert.IsFalse(target.CanAccess("C2", "A1"));
        }

        [TestMethod]
        public void CanAccess_ControlerActionPermission_True()
        {
            var target = new Role()
            {
                Permissions = new Permission[] 
                {
                    new Permission() { ControllerName = "C1", ActionName = "A1"},
                    new Permission() { ControllerName = "C1", ActionName = "A2"},
                    new Permission() { ControllerName = "C2", ActionName = "A3"}
                }
            };

            Assert.IsTrue(target.CanAccess("C1", "A1"));
            Assert.IsTrue(target.CanAccess("C1", "A2"));
            Assert.IsTrue(target.CanAccess("C2", "A3"));
        }
    }
}

