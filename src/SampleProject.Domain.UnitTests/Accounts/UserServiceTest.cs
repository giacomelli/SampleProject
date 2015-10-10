using System;
using System.Linq;
using HelperSharp;
using KissSpecifications;
using SampleProject.Domain.Accounts;
using SampleProject.Infrastructure.Framework.Globalization;
using SampleProject.Infrastructure.Framework.Runtime;
using SampleProject.Infrastructure.Repositories.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Skahal.Infrastructure.Framework.Repositories;
using TestSharp;

namespace SampleProject.Domain.UnitTests.Accounts
{
    [TestClass]
    public class UserServiceTest
    {
        #region Fields
        private UserService m_target;
        private IRepository<User> m_userRepository;
        private IRepository<Role> m_roleRepository;
        private IRoleService m_roleService;
        private IUnitOfWork m_unitOfWork;
        #endregion

        #region Initialize
        [TestInitialize]
        public void InitializeTest()
        {
            m_unitOfWork = new MemoryUnitOfWork();

            m_roleRepository = new MemoryRoleRepository(m_unitOfWork);

            m_roleRepository.Add(new Role { Id = 1 });

            m_roleService = new RoleService(m_roleRepository, m_unitOfWork, null);

            m_userRepository = new MemoryUserRepository(m_unitOfWork);

            m_target = new UserService(m_userRepository, m_unitOfWork, m_roleService);

            m_unitOfWork.Commit();
        }
        #endregion

        #region Tests
        [TestMethod]
        public void Save_NullUser_Exception()
        {
            ExceptionAssert.IsThrowing(new ArgumentNullException("entity"), () =>
            {
                m_target.Save(null);
            });
        }

        [TestMethod]
        public void Save_New_Created()
        {
            m_target.Save(new User() { ExternalUserId = "1", UserName = "TESTE", FullName = "TESTE", Email = "teste@teste.com.br", RoleId = 1 });
            Assert.AreEqual(0, m_userRepository.CountAll());

            m_unitOfWork.Commit();
            Assert.AreEqual(1, m_userRepository.CountAll());
            var actual = m_userRepository.FindFirst();
            Assert.IsNotNull(actual.Role);
        }

        [TestMethod]
        public void Save_ExistsUsername_Exception()
        {
            m_target.Save(new User()
            {
                ExternalUserId = "1",
                UserName = "TESTE",
                FullName = "TESTE",
                Email = "teste@teste.com.br",
                RoleId = 1
            });
            m_unitOfWork.Commit();

            ExceptionAssert.IsThrowing(new SpecificationNotSatisfiedException(Texts.ThereIsOtherEntityWithSameName.With("usuário", "TESTE")), () =>
            {
                m_target.Save(new User()
                {
                    ExternalUserId = "1",
                    UserName = "TESTE",
                    FullName = "TESTE",
                    Email = "teste@teste.com.br",
                    RoleId = 1
                });
            });
        }
            
        [TestMethod]
        public void Save_Old_Updated()
        {
            m_target.Save(new User() { ExternalUserId = "1", UserName = "TESTE", FullName = "TESTE", Email = "teste@teste.com.br", RoleId = 1 });
            m_unitOfWork.Commit();
            Assert.AreEqual(1, m_userRepository.CountAll());

            var user = m_userRepository.FindFirst() as User;

            var toUpdated = user.Clone() as User;
            toUpdated.Id = user.Id;
            toUpdated.FullName = "Updated";

            m_target.Save(toUpdated);
            m_unitOfWork.Commit();

            Assert.AreEqual(1, m_userRepository.CountAll());
            var actual = m_userRepository.FindFirst();

            Assert.AreEqual("Updated", actual.FullName);
        }

        [TestMethod]
        public void GetByFilter_Filter_Users()
        {
            m_userRepository.Add(new User() { FullName = null, UserName = "hikaru", Enabled = true, Role = new Role { Id = 1 } });
            m_userRepository.Add(new User() { FullName = "abc", UserName = "ichijio", Enabled = false, Role = new Role { Id = 1 } });
            m_userRepository.Add(new User() { FullName = "ABCD", UserName = "minmay", Enabled = true, Role = new Role { Id = 2 } });
            m_unitOfWork.Commit();

            var actual = m_target.GetByFilter(0, int.MaxValue, new UserFilter { UserName = "hikaru" });
            Assert.AreEqual(1, actual.TotalCount);

            actual = m_target.GetByFilter(0, int.MaxValue, new UserFilter { UserActiveStatus = new bool[] { true }, RoleIds = new int[] { 1 } });
            Assert.AreEqual(1, actual.TotalCount);

            actual = m_target.GetByFilter(0, int.MaxValue, new UserFilter { FullName = "ABCD" });
            Assert.AreEqual(1, actual.TotalCount);

            actual = m_target.GetByFilter(0, int.MaxValue, new UserFilter());
            Assert.AreEqual(3, actual.TotalCount);

            actual = m_target.GetByFilter(0, int.MaxValue, new UserFilter { UserActiveStatus = new bool[] { true } });
            Assert.AreEqual(2, actual.TotalCount);
        }

        [TestMethod]
        public void GetByFullName_Filter_Users()
        {
            m_userRepository.Add(new User() { FullName = null });
            m_userRepository.Add(new User() { FullName = "abc" });
            m_userRepository.Add(new User() { FullName = "ABCD" });
            m_unitOfWork.Commit();

            var actual = m_target.GetByFullName(0, int.MaxValue, "a");
            Assert.AreEqual(2, actual.TotalCount);

            actual = m_target.GetByFullName(0, int.MaxValue, "D");
            Assert.AreEqual(1, actual.TotalCount);

            actual = m_target.GetByFullName(0, int.MaxValue, "");
            Assert.AreEqual(3, actual.TotalCount);

            Assert.AreEqual("abc", m_target.GetByFullName("abc").FullName);
        }

        [TestMethod]
        public void Get_NoArgs_AllUsers()
        {
            Assert.AreEqual(0, m_target.Get(0, int.MaxValue, f => true).TotalCount);
            m_userRepository.Add(new User());
            m_unitOfWork.Commit();

            Assert.AreEqual(1, m_target.Get(0, int.MaxValue, f => true).TotalCount);
        }

        [TestMethod]
        public void GetUserById_Id_User()
        {
            Assert.AreEqual(0, m_target.Get(0, int.MaxValue, f => true).TotalCount);
            m_userRepository.Add(new User() { UserName = "TEST1" });
            m_userRepository.Add(new User() { UserName = "TEST2" });
            m_unitOfWork.Commit();

            var expected = m_userRepository.FindFirst(u => u.UserName.Equals("TEST2"));

            Assert.AreEqual(expected, m_target.GetById(expected.Id));
        }

        [TestMethod]
        public void GetByExternalUserId_ExternalUserId_User()
        {
            Assert.AreEqual(0, m_target.Get(0, int.MaxValue, f => true).TotalCount);
            m_userRepository.Add(new User() { ExternalUserId = "TEST1" });
            m_userRepository.Add(new User() { ExternalUserId = "TEST2" });
            m_unitOfWork.Commit();
            Assert.AreEqual("TEST2", m_target.GetByExternalUserId("TEST2").ExternalUserId);
        }

        [TestMethod]
        public void GetByUserName_UserName_User()
        {
            Assert.AreEqual(0, m_target.Get(0, int.MaxValue, f => true).TotalCount);
            m_userRepository.Add(new User() { UserName = "TEST1" });
            m_userRepository.Add(new User() { UserName = "TEST2" });
            m_unitOfWork.Commit();

            Assert.AreEqual("TEST2", m_target.GetByUserName("TEST2").UserName);
            Assert.AreEqual("TEST1", m_target.GetByUserName("TEST1").UserName);
            Assert.IsNull(m_target.GetByUserName("TEST3"));
        }

        [TestMethod]
        public void GetByEmail_Email_User()
        {
            Assert.AreEqual(0, m_target.Get(0, int.MaxValue, f => true).TotalCount);
            m_userRepository.Add(new User() { Email = "TEST1" });
            m_userRepository.Add(new User() { Email = "TEST2" });
            m_unitOfWork.Commit();

            Assert.AreEqual("TEST2", m_target.GetByEmail("TEST2").Email);
            Assert.AreEqual("TEST1", m_target.GetByEmail("TEST1").Email);
            Assert.IsNull(m_target.GetByEmail("TEST3"));
        }
    
        [TestMethod]
        public void RemoveUser_User_Removed()
        {
            m_userRepository.Add(new User() { UserName = "TEST1" });
            m_userRepository.Add(new User() { UserName = "TEST2" });
            m_userRepository.Add(new User() { UserName = "TEST3" });
            m_unitOfWork.Commit();

            m_target.Remove(3);
            m_unitOfWork.Commit();

            Assert.IsNotNull(m_userRepository.FindBy(1));
            Assert.IsNotNull(m_userRepository.FindBy(2));
            Assert.IsNull(m_userRepository.FindBy(3));
        }

        [TestMethod]
        public void Count_Filter_Count()
        {
            Assert.AreEqual(0, m_target.Count(f => true));
            m_userRepository.Add(new User() { UserName = "TEST1" });
            m_userRepository.Add(new User() { UserName = "TEST2" });
            m_unitOfWork.Commit();

            Assert.AreEqual(2, m_target.Count(f => true));
            Assert.AreEqual(1, m_target.Count(f => f.UserName.Equals("TEST1")));
        }   
       
        [TestMethod]
        public void CanSendEmailNotificationTo_UserWithEmailNotFound_False()
        {
            var actual = m_target.CanSendEmailNotificationTo("test@SampleProjecttest.com.br");
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void CanSendEmailNotificationTo_UserDisabled_False()
        {
            var user = new User() { Email = "test@SampleProjecttest.com.br", Enabled = false, Role = new Role() { EmailNotificationEnabled = true } };
            m_userRepository.Add(user);
            m_unitOfWork.Commit();

            var actual = m_target.CanSendEmailNotificationTo("test@SampleProjecttest.com.br");
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void CanSendEmailNotificationTo_RoleEmailNotificationDisabled_False()
        {
            var user = new User() { Email = "test@SampleProjecttest.com.br", Enabled = true, Role = new Role() { EmailNotificationEnabled = false } };
            m_userRepository.Add(user);
            m_unitOfWork.Commit();

            var actual = m_target.CanSendEmailNotificationTo("test@SampleProjecttest.com.br");
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void CanSendEmailNotificationTo_RoleEmailNotificationEnabled_True()
        {
            var user = new User() { Email = "test@SampleProjecttest.com.br", Enabled = true, Role = new Role() { EmailNotificationEnabled = true } };
            m_userRepository.Add(user);
            m_unitOfWork.Commit();

            var actual = m_target.CanSendEmailNotificationTo("test@SampleProjecttest.com.br");
            Assert.IsTrue(actual);
        }
        #endregion
    }
}
