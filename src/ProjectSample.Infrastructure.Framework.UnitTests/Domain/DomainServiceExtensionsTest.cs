using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectSample.Domain.Accounts;
using ProjectSample.Infrastructure.Framework.Domain;
using Rhino.Mocks;

namespace ProjectSample.Infrastructure.Framework.UnitTests.Domain
{
    [TestClass]
    public class DomainServiceExtensionsTest
    {
        [TestMethod]
        public void Get_OffsetAndLimit_Query()
        {
            var service = MockRepository.GenerateStrictMock<IDomainService<User>>();
            service.Expect(s => s.Get(1, 2, null)).Return(null);

            DomainServiceExtensions.Get(service, 1, 2);

            service.VerifyAllExpectations();
        }

        [TestMethod]
        public void Get_Filter_Query()
        {
            var service = MockRepository.GenerateStrictMock<IDomainService<User>>();
            Expression<Func<User, bool>> filter = (f) => f.Id == 1;
            service.Expect(s => s.Get(0, int.MaxValue, filter)).Return(null);

            DomainServiceExtensions.Get(service, filter);

            service.VerifyAllExpectations();
        }

        [TestMethod]
        public void GetAll_None_Query()
        {
            var service = MockRepository.GenerateStrictMock<IDomainService<User>>();
            service.Expect(s => s.Get(0, int.MaxValue, null)).Return(null);

            DomainServiceExtensions.GetAll(service);

            service.VerifyAllExpectations();
        }

        [TestMethod]
        public void GetAscending_OffsetLimitOrderBy_Query()
        {
            var service = MockRepository.GenerateStrictMock<IDomainService<User>>();
            Expression<Func<User, string>> orderBy = (f) => f.FullName;

            service.Expect(s => s.GetAscending(1, 2, null, orderBy)).Return(null);

            DomainServiceExtensions.GetAscending(service, 1, 2, orderBy);

            service.VerifyAllExpectations();
        }

        [TestMethod]
        public void GetAscending_OrderBy_Query()
        {
            var service = MockRepository.GenerateStrictMock<IDomainService<User>>();
            Expression<Func<User, string>> orderBy = (f) => f.FullName;

            service.Expect(s => s.GetAscending(0, int.MaxValue, null, orderBy)).Return(null);

            DomainServiceExtensions.GetAscending(service, orderBy);

            service.VerifyAllExpectations();
        }

        [TestMethod]
        public void GetDescending_OffsetLimitOrderBy_Query()
        {
            var service = MockRepository.GenerateStrictMock<IDomainService<User>>();
            Expression<Func<User, string>> orderBy = (f) => f.FullName;

            service.Expect(s => s.GetDescending(1, 2, null, orderBy)).Return(null);

            DomainServiceExtensions.GetDescending(service, 1, 2, orderBy);

            service.VerifyAllExpectations();
        }

        [TestMethod]
        public void GetDescending_OrderBy_Query()
        {
            var service = MockRepository.GenerateStrictMock<IDomainService<User>>();
            Expression<Func<User, string>> orderBy = (f) => f.FullName;

            service.Expect(s => s.GetDescending(0, int.MaxValue, null, orderBy)).Return(null);

            DomainServiceExtensions.GetDescending(service, orderBy);

            service.VerifyAllExpectations();
        }

        [TestMethod]
        public void GetDescending_FilterAndOrderBy_Query()
        {
            var service = MockRepository.GenerateStrictMock<IDomainService<User>>();
            Expression<Func<User, bool>> filter = f => f.UserName.Equals("a");
            Expression<Func<User, string>> orderBy = (f) => f.FullName;

            service.Expect(s => s.GetDescending(0, int.MaxValue, filter, orderBy)).Return(null);

            DomainServiceExtensions.GetDescending(service, filter, orderBy);

            service.VerifyAllExpectations();
        }
    }
}
