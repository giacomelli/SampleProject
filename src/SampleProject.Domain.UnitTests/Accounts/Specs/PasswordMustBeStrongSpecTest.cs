using System;
using HelperSharp;
using SampleProject.Domain.Accounts;
using SampleProject.Domain.Accounts.Specs;
using SampleProject.Infrastructure.Framework.Domain;
using SampleProject.Infrastructure.Framework.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Skahal.Infrastructure.Framework.Repositories;

namespace SampleProject.Domain.UnitTests.Accounts.Specs
{
    [TestClass]
    public class PasswordMustBeStrongSpecTest
    {
        #region Fields
        private PasswordMustBeStrongSpec m_target;
        private IUnitOfWork m_unitOfWork;
        private IUserService m_userService;
        private IRepository<UserPasswordHistory> m_historyRepository;
        private IUserPasswordService m_userPasswordService;
        #endregion

        #region Initialize
        [TestInitialize]
        public void InitializeTest()
        {
            m_unitOfWork = new MemoryUnitOfWork();
            m_userService = MockRepository.GenerateMock<IUserService>();

            var historyId = 0;
            m_historyRepository = new MemoryRepository<UserPasswordHistory>(m_unitOfWork, (o) => ++historyId);
            
            m_userPasswordService = new UserPasswordService(null, m_historyRepository, m_userService);

            m_target = new PasswordMustBeStrongSpec(1, m_userPasswordService);

        }
        #endregion

        #region Tests
        [TestMethod]
        public void IsSatisfiedBy_LessThan8Chars_False()
        {
            Assert.IsFalse(m_target.IsSatisfiedBy(null));
            Assert.IsFalse(m_target.IsSatisfiedBy(""));
            Assert.IsFalse(m_target.IsSatisfiedBy("1"));
            Assert.IsFalse(m_target.IsSatisfiedBy("1234567"));
            Assert.AreEqual(Texts.PasswordMustHaveAtLeastChars.With(8), m_target.NotSatisfiedReason);
        }

        [TestMethod]
        public void IsSatisfiedBy_NoUppercase_False()
        {
            Assert.IsFalse(m_target.IsSatisfiedBy("abcdefgf"));
            Assert.AreEqual(Texts.PasswordMustHaveAtLeastUppercaseChars.With(1), m_target.NotSatisfiedReason);
        }

        [TestMethod]
        public void IsSatisfiedBy_NoSpecialChar_False()
        {
            Assert.IsFalse(m_target.IsSatisfiedBy("Abcdefgf"));
            Assert.AreEqual(Texts.PasswordMustHaveAtLeastSpecialChars.With(1), m_target.NotSatisfiedReason);
        }

        [TestMethod]
        public void IsSatisfiedBy_NoNumber_False()
        {
            Assert.IsFalse(m_target.IsSatisfiedBy("A*cdefgf"));
            Assert.AreEqual(Texts.PasswordMustHaveAtLeastNumber.With(1), m_target.NotSatisfiedReason);
        }

        [TestMethod]
        public void IsSatisfiedBy_EqualAnyLast20Passwords_False()
        {
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef1*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-21)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 2, Password = m_userPasswordService.Encrypt("Abcdef222*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-20)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef2*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-19)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef3*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-18)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef4*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-17)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef5*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-16)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef6*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-15)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef7*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-14)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef8*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-13)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef9*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-12)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef10*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-11)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef11*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-10)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef12*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-9)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef13*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-8)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef14*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-7)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef15*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-6)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef16*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-5)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef17*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-4)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef18*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-3)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef19*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-2)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef20*"), Created = new ActionStamp(DateTime.UtcNow.AddDays(-1)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = m_userPasswordService.Encrypt("Abcdef21*"), Created = new ActionStamp(DateTime.UtcNow) });
            m_unitOfWork.Commit();

            Assert.IsFalse(m_target.IsSatisfiedBy("Abcdef21*"));
            Assert.IsFalse(m_target.IsSatisfiedBy("Abcdef20*"));
            Assert.IsFalse(m_target.IsSatisfiedBy("Abcdef2*"));
            Assert.AreEqual(Texts.PasswordMustBeDiffThanLastPasswords.With(20), m_target.NotSatisfiedReason);

            Assert.IsTrue(m_target.IsSatisfiedBy("Abcdef222*"));
            Assert.IsTrue(m_target.IsSatisfiedBy("Abcdef1*"));
        }

        [TestMethod]
        public void IsSatisfiedBy_IsStrong_True()
        {
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = "Abcdef1*" });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = "Abcdef2*" });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = "Abcdef3*" });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = "Abcdef4*" });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1, Password = "Abcdef5*" });
            m_unitOfWork.Commit();

            Assert.IsTrue(m_target.IsSatisfiedBy("123Abcd*"));
            Assert.IsTrue(m_target.IsSatisfiedBy("/*xabA4465#32kj2lk2kl2kjç"));
        }
        #endregion
    }
}
