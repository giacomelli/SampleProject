using System;
using System.Linq;
using SampleProject.Domain.Accounts;
using SampleProject.Infrastructure.Framework.Domain;
using SampleProject.Infrastructure.Framework.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Skahal.Infrastructure.Framework.Repositories;
using TestSharp;

namespace SampleProject.Domain.UnitTests.Accounts
{
    [TestClass]
    public class UserPasswordServiceTest
    {
        #region Fields
        private UserPasswordService m_target;
        private IUnitOfWork m_unitOfWork;
        private IRepository<UserPasswordResetToken> m_tokenRepository;
        private IRepository<UserPasswordHistory> m_historyRepository;
        private IUserService m_userService;
        #endregion

        #region Initialize
        [TestInitialize]
        public void InitializeTest()
        {
            UserPasswordService.PasswordResetTokenMinutesTimeout = 10;
            m_unitOfWork = new MemoryUnitOfWork();
            m_userService = MockRepository.GenerateMock<IUserService>();
            var tokenId = 0;
            m_tokenRepository = new MemoryRepository<UserPasswordResetToken>(m_unitOfWork, (o) => ++tokenId);
            var token = new UserPasswordResetToken() { Code = Guid.NewGuid(), UserId = 3 };
            m_tokenRepository.Add(token);

            var historyId = 0;
            m_historyRepository = new MemoryRepository<UserPasswordHistory>(m_unitOfWork, (o) => ++historyId);

            m_target = new UserPasswordService(m_tokenRepository, m_historyRepository, m_userService);

            m_userService.Expect(e => e.GetById(1)).Return(new User() { Id = 1, Password = m_target.Encrypt("11"), Email = "test1@test.com" });
            m_userService.Expect(e => e.GetById(2)).Return(new User() { Id = 2, Password = m_target.Encrypt("22"), Email = "test2@test.com" });
            m_userService.Expect(e => e.GetById(3)).Return(new User() { Id = 3, Password = m_target.Encrypt("33"), Email = "test3@test.com" });

            m_userService.Expect(e => e.GetByEmail("test1@test.com")).Return(new User() { Id = 1, Password = m_target.Encrypt("11"), Email = "test1@test.com" });
            m_userService.Expect(e => e.GetByEmail("test2@test.com")).Return(new User() { Id = 2, Password = m_target.Encrypt("22"), Email = "test2@test.com" });
            m_userService.Expect(e => e.GetByEmail("test3@test.com")).Return(new User() { Id = 3, Password = m_target.Encrypt("33"), Email = "test3@test.com" });

            m_unitOfWork.Commit();
        }
        #endregion

        #region Tests
        [TestMethod]
        public void GeneratePasswordResetToken_Null_Exception()
        {
            ExceptionAssert.IsThrowing(new ArgumentNullException("email"), () =>
            {
                m_target.GeneratePasswordResetToken(null);
            });
        }

        [TestMethod]
        public void GeneratePasswordResetToken_UserNotFound_Exception()
        {
            ExceptionAssert.IsThrowing(new ArgumentException(Texts.ThereIsNoUserWithThisEmail), () =>
            {
                m_target.GeneratePasswordResetToken("t@t.com");
            });
        }

        [TestMethod]
        public void GeneratePasswordResetToken_ThereNoOtherTokenForUser_GenerateNewOne()
        {
            var user = new User() { Id = 1, Email = "test1@test.com" };
            m_tokenRepository.Add(new UserPasswordResetToken() { UserId = 2 });
            m_unitOfWork.Commit();
            var originalTokenCount = m_tokenRepository.CountAll();

            var token = m_target.GeneratePasswordResetToken(user.Email);
            m_unitOfWork.Commit();
            Assert.AreEqual(1, token.UserId);
            Assert.AreEqual(DateTime.UtcNow.Date, token.Created.Date.Date);
            Assert.IsNotNull(token.Code);

            Assert.AreEqual(originalTokenCount + 1, m_tokenRepository.CountAll());
        }

        [TestMethod]
        public void GeneratePasswordResetToken_ThereIsOtherTokenForUser_RemoveTheOldOneAndGenerateNewOne()
        {
            var user = new User() { Id = 1, Email = "test1@test.com" };
            m_tokenRepository.Add(new UserPasswordResetToken() { UserId = user.Id });
            m_tokenRepository.Add(new UserPasswordResetToken() { UserId = 2 });
            m_unitOfWork.Commit();
            var originalTokenCount = m_tokenRepository.CountAll();
            var originalToken = m_tokenRepository.FindFirst(t => t.UserId.Equals(user.Id));

            var token = m_target.GeneratePasswordResetToken(user.Email);
            m_unitOfWork.Commit();

            Assert.AreEqual(1, token.UserId);
            Assert.AreEqual(DateTime.UtcNow.Date, token.Created.Date.Date);
            Assert.IsNotNull(token.Code);

            Assert.AreEqual(originalTokenCount, m_tokenRepository.CountAll());
            Assert.IsNull(m_tokenRepository.FindBy(originalToken.Id));
        }

        [TestMethod]
        public void ResetPassword_InvalidTokenCode_Exception()
        {
            var token = m_tokenRepository.FindFirst();

            ExceptionAssert.IsThrowing(new ArgumentException(Texts.UserPasswordResetTokenExpired), () =>
            {
                m_target.ResetPassword(Guid.NewGuid(), "c");
            });
        }

        [TestMethod]
        public void ResetPassword_TokenExpired_Exception()
        {
            var token = m_tokenRepository.FindFirst();
            token.Expired = true;
            ExceptionAssert.IsThrowing(new ArgumentException(Texts.UserPasswordResetTokenExpired), () =>
            {
                m_target.ResetPassword(token.Code, "c");
            });
        }

        [TestMethod]
        public void ResetPassword_ArgsOk_PasswordChangedAndTokenUsed()
        {
            var token = m_tokenRepository.FindFirst();
            m_target.ResetPassword(token.Code, "Abc123*A");
            m_unitOfWork.Commit();

            var user = m_userService.GetById(3);
            Assert.AreEqual(m_target.Encrypt("Abc123*A"), user.Password);

            Assert.IsTrue(token.Used);
        }

        [TestMethod]
        public void ChangePassword_OldPasswordDoesNotMatch_Exception()
        {
            ExceptionAssert.IsThrowing(new InvalidOperationException(Texts.OldPasswordDoesNotMatch), () =>
            {
                m_target.ChangePassword(3, "3", "Abc123*A");
            });
        }

        [TestMethod]
        public void ChangePassword_OldPasswordMatchByUserDisabeld_Exception()
        {
            var user = m_userService.GetById(3);
            user.Enabled = false;
            m_userService.Save(user);

            user = m_userService.GetById(3);

            ExceptionAssert.IsThrowing(new InvalidOperationException(Texts.UserDisabledCannotChangePassword), () =>
            {
                m_target.ChangePassword(3, "33", "Abc123*A");
            });
        }

        [TestMethod]
        public void ChangePassword_OldPasswordMatch_Changed()
        {
            m_target.ChangePassword(3, "33", "Abc123*A");
            m_unitOfWork.Commit();

            var user = m_userService.GetById(3);
            Assert.AreEqual(m_target.Encrypt("Abc123*A"), user.Password);

            var histories = m_historyRepository.FindAll(f => f.UserId == 3).ToList();
            Assert.AreEqual(1, histories.Count);
            Assert.AreEqual(m_target.Encrypt("Abc123*A"), histories[0].Password);
        }

        [TestMethod]
        public void ChangePassword_NewPasswordLessThan8Chars_Exception()
        {
            m_target.ChangePassword(3, "33", "Abc123*A");
            m_unitOfWork.Commit();

            var user = m_userService.GetById(3);
            Assert.AreEqual(m_target.Encrypt("Abc123*A"), user.Password);

            var histories = m_historyRepository.FindAll(f => f.UserId == 3).ToList();
            Assert.AreEqual(1, histories.Count);
            Assert.AreEqual(m_target.Encrypt("Abc123*A"), histories[0].Password);
        }

        [TestMethod]
        public void GetPasswordResetTokensNotSent_SomeNotSent_OnlyNotSent()
        {
            var token2 = new UserPasswordResetToken() { Code = Guid.NewGuid(), UserId = 2 };
            m_tokenRepository.Add(token2);

            var token3 = new UserPasswordResetToken() { Code = Guid.NewGuid(), UserId = 3 };
            m_tokenRepository.Add(token3);

            m_unitOfWork.Commit();

            var actual = m_target.GetPasswordResetTokensNotSent().ToList();
            Assert.AreEqual(3, actual.Count);

            m_target.MarkPasswordResetTokenAsSent(token2.Id);
            actual = m_target.GetPasswordResetTokensNotSent().ToList();
            Assert.AreEqual(2, actual.Count);

            token3.Expired = true;
            actual = m_target.GetPasswordResetTokensNotSent().ToList();
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void GetPasswordResetTokensUsedNotNotified_SomeNotNotified_OnlyNotNotified()
        {
            var token2 = new UserPasswordResetToken() { Code = Guid.NewGuid(), UserId = 2 };
            m_tokenRepository.Add(token2);

            var token3 = new UserPasswordResetToken() { Code = Guid.NewGuid(), UserId = 3 };
            m_tokenRepository.Add(token3);

            m_unitOfWork.Commit();

            var actual = m_target.GetPasswordResetTokensUsedNotNotified().ToList();
            Assert.AreEqual(0, actual.Count);

            token2.Used = true;
            actual = m_target.GetPasswordResetTokensUsedNotNotified().ToList();
            Assert.AreEqual(1, actual.Count);

            token3.Used = true;
            actual = m_target.GetPasswordResetTokensUsedNotNotified().ToList();
            Assert.AreEqual(2, actual.Count);

            m_target.MarkPasswordResetTokenAsNotifiedUse(token3.Id);
            actual = m_target.GetPasswordResetTokensUsedNotNotified().ToList();
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void IsPasswordExpired_NotExpired_False()
        {
            UserPasswordService.PasswordExpirationDays = 3;
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1 });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 3, Created = new ActionStamp(DateTime.UtcNow.AddDays(-2)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 3, Created = new ActionStamp(DateTime.UtcNow.AddDays(-1)) });
            m_unitOfWork.Commit();

            Assert.IsFalse(m_target.IsPasswordExpired(3));
        }

        [TestMethod]
        public void IsPasswordExpired_Expired_True()
        {
            UserPasswordService.PasswordExpirationDays = 3;
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 1 });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 3, Created = new ActionStamp(DateTime.UtcNow.AddDays(-5)) });
            m_historyRepository.Add(new UserPasswordHistory() { UserId = 3, Created = new ActionStamp(DateTime.UtcNow.AddDays(-4)) });
            m_unitOfWork.Commit();

            Assert.IsTrue(m_target.IsPasswordExpired(3));
        }

        [TestMethod]
        public void Encrypt_NotEncrypted_Encrypted()
        {
            var original = "abc123*";
            Assert.AreNotEqual(original, m_target.Encrypt(original));
        }
        #endregion
    }
}
